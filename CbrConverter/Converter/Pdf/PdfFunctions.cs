using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using iTextSharp.text.pdf.parser;
using System.Drawing.Imaging;
using iTextSharp.text.pdf;

namespace CbrConverter
{
    public class PdfFunctions
    {
        //static string _ExtractionDir;
        public static event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();


        /// <summary>
        /// Operators that set fonts and draw text, replaced by ones that do nothing: only the images are
        /// wanted, and loading fonts can fail (e.g. fonts with Mac Roman names need a legacy encoding)
        /// </summary>
        static readonly Dictionary<string, IContentOperator> SkipText = new[] { "Tf", "Tj", "TJ", "'", "\"" }
            .ToDictionary(name => name, name => (IContentOperator)new IgnoreOperator());

        class IgnoreOperator : IContentOperator
        {
            public void Invoke(PdfContentStreamProcessor processor, PdfLiteral oper, List<PdfObject> operands) { }
        }

        public static PdfExtractionResult PDF_ExportImage(string filename, string dirForExtractions, int divider, bool checkResult, bool joinImages)
        {
            var details = new PdfExtractionResult();

            DataAccess.Instance.g_curProgress = 0;
            evnt_UpdateCurBar();

            Dictionary<PageImageIndex, Image> extracted = null;
            PdfReader pdfReader = null;
            try
            {
                pdfReader = new PdfReader(filename);
            }
            catch (Exception ex) when (!(ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is UnauthorizedAccessException))
            {
                // iTextSharp can't open some PDFs, e.g. those using AES-256 revision 6 encryption (Acrobat X and later)
                extracted = ExportWithPdfPig(filename, dirForExtractions, divider, details, ex);
            }

            if (pdfReader != null)
            {
                try
                {
                    // Ask itextsharp to extract image
                    var pdfParser = new PdfReaderContentParser(pdfReader);
                    var pdfListener = new PDFImageListener(dirForExtractions);

                    double pgc = pdfReader.NumberOfPages;
                    double CurOneStep = divider / pgc;
                    details.Pages = (int)pgc;

                    for (int i = 1; i <= pgc; i++)
                    {
                        pdfListener.PageIndex = i;
                        // itextsharp send response to listener, skipping text so fonts are never loaded
                        pdfParser.ProcessContent(i, pdfListener, SkipText);

                        DataAccess.Instance.g_curProgress += CurOneStep;
                        evnt_UpdateCurBar();
                    }
                    extracted = pdfListener.ImagesList;
                }
                catch (Exception ex)
                {
                    // iTextSharp failed partway through: start this PDF again with PdfPig
                    foreach (string partial in Directory.GetFiles(dirForExtractions))
                        File.Delete(partial);
                    DataAccess.Instance.g_curProgress = 0;
                    evnt_UpdateCurBar();
                    extracted = ExportWithPdfPig(filename, dirForExtractions, divider, details, ex);
                }
                finally
                {
                    pdfReader.Close();
                }
            }

            details.ImagesBeforeMerge = extracted.Count;
            details.ImagesAfterMerge = details.ImagesBeforeMerge;

            if (checkResult && details.Pages != details.ImagesBeforeMerge && joinImages)
            {
                ImageJoiner cp = new ImageJoiner();
                details.ImagesAfterMerge = cp.Merge(extracted, dirForExtractions).Count;
            }

            return details;
        }

        /// <summary>
        /// Extract the page images with PdfPig, which can read encryption iTextSharp can't. Files are named like
        /// PDFImageListener names them, so merging works the same way.
        /// </summary>
        private static Dictionary<PageImageIndex, Image> ExportWithPdfPig(string filename, string dirForExtractions, int divider, PdfExtractionResult details, Exception iTextError)
        {
            UglyToad.PdfPig.PdfDocument doc;
            try
            {
                doc = UglyToad.PdfPig.PdfDocument.Open(filename);
            }
            catch (UglyToad.PdfPig.Exceptions.PdfDocumentEncryptedException)
            {
                throw new Exception("this PDF is password-protected, so it can't be converted");
            }
            catch (Exception pigError)
            {
                // neither library can read it: report both reasons
                throw new Exception(iTextError.Message + " (second PDF reader: " + pigError.Message + ")", iTextError);
            }

            var images = new Dictionary<PageImageIndex, Image>();
            using (doc)
            {
                details.Pages = doc.NumberOfPages;
                double step = (double)divider / doc.NumberOfPages;
                Directory.CreateDirectory(dirForExtractions);

                foreach (var page in doc.GetPages())
                {
                    foreach (var image in page.GetImages())
                    {
                        // keep JPEG and JPEG 2000 data exactly as stored; anything else is decoded to PNG
                        byte[] bytes;
                        try
                        {
                            bytes = UnwrapImageData(image);
                        }
                        catch
                        {
                            bytes = image.RawMemory.ToArray();
                        }
                        string ext;
                        if (bytes.Length > 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                            ext = "jpg";
                        else if (bytes.Length > 8 && bytes[4] == 0x6A && bytes[5] == 0x50 && bytes[6] == 0x20 && bytes[7] == 0x20)
                            ext = "jp2";
                        else if (bytes.Length > 2 && bytes[0] == 0xFF && bytes[1] == 0x4F)
                            ext = "j2k";
                        else if (image.TryGetPng(out var png))
                        {
                            bytes = png;
                            ext = "png";
                        }
                        else
                        {
                            details.ImagesSkipped++;
                            continue;
                        }

                        var index = new PageImageIndex
                        {
                            ImageName = string.Format("{0:0000}_{1:0000}.{2}", page.Number, images.Count, ext),
                            ImageIndex = images.Count,
                            PageIndex = page.Number
                        };
                        File.WriteAllBytes(System.IO.Path.Combine(dirForExtractions, index.ImageName), bytes);
                        images.Add(index, null);
                    }

                    DataAccess.Instance.g_curProgress += step;
                    evnt_UpdateCurBar();
                }
            }
            return images;
        }

        /// <summary>
        /// Undo any encodings wrapped around the image data (ASCII85, Flate, ...) but stop before a final
        /// JPEG or JPEG 2000 layer, so that data is kept exactly as stored
        /// </summary>
        private static byte[] UnwrapImageData(UglyToad.PdfPig.Content.IPdfImage image)
        {
            var provider = UglyToad.PdfPig.Filters.DefaultFilterProvider.Instance;
            var filters = provider.GetFilters(image.ImageDictionary);
            var data = image.RawMemory;
            for (int i = 0; i < filters.Count; i++)
            {
                if (filters[i] is UglyToad.PdfPig.Filters.DctDecodeFilter || filters[i] is UglyToad.PdfPig.Filters.JpxDecodeFilter)
                    break;
                data = filters[i].Decode(data, image.ImageDictionary, provider, i);
            }
            return data.ToArray();
        }

//////////////////////////////////////////////// OLD METHOD (bug, sometime find same images for each pages)
        //private static void WriteImages(string outputFolder, Dictionary<PageImageIndex, Image> imageList)
        //{
        //    foreach (var imageInfo in imageList)
        //    {
        //        var imageData = imageInfo.Value;

        //        string tmp = String.Format(@"{0}\{1}", outputFolder, imageInfo.Key.ImageName);
        //        if (imageData == null)
        //            imageData = new Bitmap(tmp);

        //        imageData.Save(tmp);
        //        imageInfo.Value.Dispose();
        //    }

        //    imageList.Clear();
        //}


        //public static bool PDF_ExportImage(string filename, string dirForExtractions, int divider)
        //{
        //    bool isFileValid = true;
        //    iTextSharp.text.pdf.PdfReader.unethicalreading = true;
        //    iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(filename);

        //    DataAccess.Instance.g_curProgress = 0;
            
        //    _ExtractionDir = dirForExtractions;

        //    int imageCount = 0;
        //    double tem0 = divider;
        //    double pgc = pdfReader.NumberOfPages;
        //    double CurOneStep = (double)(tem0 / pgc);


        //    for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
        //    {
        //        PdfReader pdf = pdfReader;
        //        PdfDictionary pg = pdf.GetPageN(pageNumber);
        //        PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
        //        PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
        //        if (xobj != null)
        //        {
        //            var renderInfoList = new List<ImageRenderInfo>();
        //            foreach (iTextSharp.text.pdf.PdfName name in xobj.Keys)
        //            {
        //                iTextSharp.text.pdf.PdfObject obj = xobj.Get(name);
        //                if (obj.IsIndirect())
        //                {
        //                    PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
        //                    if (tg != null)
        //                    {
        //                        //PdfName type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
        //                        //if (PdfName.IMAGE.Equals(type))
        //                        try
        //                        {
        //                            string width = tg.Get(iTextSharp.text.pdf.PdfName.WIDTH).ToString();
        //                            string height = tg.Get(iTextSharp.text.pdf.PdfName.HEIGHT).ToString();
        //                            ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new Matrix(float.Parse(width), float.Parse(height)), (iTextSharp.text.pdf.PRIndirectReference)obj, tg);
        //                            renderInfoList.Add(imgRI);
        //                            RenderImage(renderInfoList, ref imageCount);
        //                            renderInfoList.Clear();
        //                        }
        //                        catch
        //                        {
        //                            isFileValid = false;
        //                        }
        //                    }
        //                }
        //                DataAccess.Instance.g_curProgress += CurOneStep;
        //                evnt_UpdateCurBar();
        //            }
        //            // Images are found from bottom to top
        //            renderInfoList.Reverse();
        //            //RenderImage(renderInfoList, ref imageCount);
        //        }
        //    }

        //    return isFileValid;
        //}

        //private static void RenderImage(List<ImageRenderInfo> renderInfoList, ref int count)
        //{
        //    if (renderInfoList.Count > 1)
        //    {
        //        // merge image
        //        var listImages = renderInfoList.Select(ri => ri.GetImage().GetDrawingImage()).Where(i => i != null).ToList();
        //        var combinedImage = CombineImages(listImages);
        //        // and save new
        //        string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
        //        combinedImage.Save(tmp);
        //    }
        //    else 
        //    {
        //        var renderInfo = renderInfoList.First();
        //        PdfImageObject image = renderInfo.GetImage();
        //        using (var dotnetImg = image.GetDrawingImage())
        //        {
        //            if (dotnetImg != null)
        //            {
        //                string tmp = String.Format("{0}\\{1:d3}.jpeg", _ExtractionDir, ++count);
        //                dotnetImg.Save(tmp);

        //            }
        //        }
        //    }
            
        //}

        //private static Image CombineImages(List<Image> imagesList)
        //{       
        //    int nIndex = 0;
        //    int height = 0;

        //    // List all width
        //    List<int> imageWidth = new List<int>();
        //    foreach (var img in imagesList)
        //    {
        //        imageWidth.Add(img.Width);
        //        height += img.Height;
        //    }
        //    imageWidth.Sort();

        //    // Take max width
        //    int width = imageWidth[imageWidth.Count - 1];

        //    // Create new image with max width and sum of all height
        //    Bitmap img3 = new Bitmap(width, height);
        //    Graphics g = Graphics.FromImage(img3);

        //    // merge all images
        //    g.Clear(SystemColors.AppWorkspace);
        //    foreach (var img in imagesList)
        //    {
        //        if (nIndex == 0)
        //        {
        //            g.DrawImage(img, new Point(0, 0));
        //            nIndex++;
        //            height = img.Height;
        //        }
        //        else
        //        {
        //            g.DrawImage(img, new Point(0, height));
        //            height += img.Height;
        //        }
        //        img.Dispose();
        //    }

        //    g.Dispose();
        //    imagesList.Clear();

        //    return img3;            
        //}
    }
}
