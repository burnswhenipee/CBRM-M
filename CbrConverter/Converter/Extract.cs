using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace CbrConverter
{
    public enum Format { Comic, Pdf, ImageFolder }

    public class Extract
    {
        string temporaryDir;
        string outputBase; //output path without extension
        public event UpdateCurrentBar evnt_UpdateCurBar;
        public delegate void UpdateCurrentBar();
        public event UpdateTotalBar evnt_UpdatTotBar;
        public delegate void UpdateTotalBar(Extract m, EventArgs e);
        public event UpdateFileName evnt_UpdateFileName;
        public delegate void UpdateFileName(Extract m, EventArgs e);
        public event ErrorNotify evnt_ErrorNotify;
        public delegate void ErrorNotify(Extract m, string e);
        public event ErrorNotify evnt_WarningNotify;
        public EventArgs e = null;
        Format _From, _To;
        bool _JoinImages;

        // image types that can be put in a PDF
        static readonly string[] PdfImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff" };
        // other image types that may be found in comics, reported when they can't be put in a PDF
        static readonly string[] OtherImageExtensions = { ".webp", ".avif", ".jxl", ".heic", ".jp2" };

        /// <summary>
        /// Convert the selected file, or the items of the "from" format directly inside the selected folder:
        /// its CBR/CBZ files, its PDF files, or its subfolders of images (nothing deeper is included)
        /// </summary>
        public void BeginExtraction(Format from, Format to, bool joinImages)
        {
            _From = from;
            _To = to;
            _JoinImages = joinImages;

            string source = DataAccess.Instance.g_WorkingDir;
            List<string> files;
            if (File.Exists(source))
                files = new List<string> { source };
            else
            {
                if (from == Format.ImageFolder)
                    files = Directory.GetDirectories(source).ToList();
                else
                    files = Directory.GetFiles(source).Where(f => from == Format.Comic ? IsComic(f) : IsPdf(f)).ToList();
                files = files.OrderBy(f => f, StringComparer.CurrentCultureIgnoreCase).ToList();
                if (files.Count == 0)
                    evnt_WarningNotify(this, source + " : nothing to convert in this folder");
            }

            DataAccess.Instance.g_FileIndex = 0;
            DataAccess.Instance.g_FileCount = files.Count;

            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                foreach (string file in files)
                {
                    if (!DataAccess.Instance.g_Processing) //stop button pressed
                        break;

                    DataAccess.Instance.g_WorkingFile = file;
                    DataAccess.Instance.g_FileIndex++;
                    SetProgress(0);
                    evnt_UpdateFileName(this, e);
                    try
                    {
                        ConvertFile(file);
                    }
                    catch (Exception ex)
                    {
                        evnt_ErrorNotify(this, file + " : " + ex.Message);
                    }
                }
            };
            bw.RunWorkerCompleted += (sender, args) =>
            {
                //finished, update the ui
                DataAccess.Instance.g_Processing = false;
                DataAccess.Instance.g_totProgress = 0;
                DataAccess.Instance.g_curProgress = 0;
                DataAccess.Instance.g_WorkingFile = string.Empty;
                DataAccess.Instance.g_FileIndex = 0;
                evnt_UpdateFileName(this, e);
                evnt_UpdatTotBar(this, e);
                evnt_UpdateCurBar();
            };
            bw.RunWorkerAsync();
        }

        private static bool IsComic(string file)
        {
            string ext = Path.GetExtension(file).ToLower();
            return ext == ".cbr" || ext == ".cbz";
        }

        private static bool IsPdf(string file)
        {
            return Path.GetExtension(file).ToLower() == ".pdf";
        }

        /// <summary>
        /// Unpack the file into a working folder, then write each selected output from it
        /// </summary>
        private void ConvertFile(string file)
        {
            if (_From == Format.ImageFolder)
            {
                ConvertImageFolder(file);
                return;
            }

            outputBase = Path.Combine(DataAccess.Instance.g_Output_dir, Path.GetFileNameWithoutExtension(file));
            Directory.CreateDirectory(DataAccess.Instance.g_Output_dir);
            temporaryDir = CreateWorkingDir();
            try
            {
                if (_From == Format.Comic)
                    ExtractArchive(file, 50);
                else
                    ExtractPdf(file, 80);

                if (DataAccess.Instance.g_Processing)
                {
                    if (_To == Format.Pdf)
                        GeneratePdf(temporaryDir, 50);
                    else if (_To == Format.ImageFolder)
                        CopyToImageFolder(temporaryDir);
                    else
                        ZipFile.CreateFromDirectory(temporaryDir, GetAvailableOutputPath(".cbz"));
                }
                if (DataAccess.Instance.g_Processing)
                    SetProgress(100);
            }
            finally
            {
                DeleteWorkingDir();
            }
        }

        /// <summary>
        /// Turn the images directly inside a folder into a CBZ or PDF named after the folder
        /// </summary>
        private void ConvertImageFolder(string folder)
        {
            var images = Directory.GetFiles(folder)
                .Where(f => PdfImageExtensions.Contains(Path.GetExtension(f).ToLower()) || OtherImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                .OrderBy(f => f, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
            if (images.Count == 0)
            {
                evnt_WarningNotify(this, string.Format("{0} : no images found in this folder, so no {1} was made", folder, _To == Format.Pdf ? "PDF" : "CBZ"));
                return;
            }

            outputBase = Path.Combine(DataAccess.Instance.g_Output_dir, Path.GetFileName(folder));
            Directory.CreateDirectory(DataAccess.Instance.g_Output_dir);
            if (_To == Format.Pdf)
            {
                GeneratePdf(folder, 100);
                return;
            }

            string savedfile = GetAvailableOutputPath(".cbz");

            double step = 100.0 / images.Count;
            using (var zip = ZipFile.Open(savedfile, ZipArchiveMode.Create))
            {
                foreach (string image in images)
                {
                    if (!DataAccess.Instance.g_Processing)
                        break;
                    // images are already compressed, storing them as they are is much faster
                    zip.CreateEntryFromFile(image, Path.GetFileName(image), CompressionLevel.NoCompression);
                    SetProgress(DataAccess.Instance.g_curProgress + step);
                }
            }

            // stopped halfway: don't leave an incomplete CBZ behind
            if (!DataAccess.Instance.g_Processing)
                File.Delete(savedfile);
        }

        /// <summary>
        /// Extract every file of a CBR/CBZ into the working folder
        /// </summary>
        private void ExtractArchive(string file, double progressShare)
        {
            using var archive = ArchiveFactory.OpenArchive(file, new ReaderOptions());
            var entries = archive.Entries.Where(entry => !entry.IsDirectory).ToList();

            // pages can sit in subfolders (e.g. one per chapter): drop the folder they all share and
            // fold the rest into the file name, so pages with the same name don't overwrite each other
            var keys = entries.Select(entry => entry.Key.Replace('\\', '/')).ToList();
            string commonDir = CommonDirectory(keys);

            var options = new ExtractionOptions
            {
                Overwrite = true,
                PreserveAttributes = false,
                PreserveFileTime = true
            };

            double step = entries.Count > 0 ? progressShare / entries.Count : 0;
            for (int i = 0; i < entries.Count && DataAccess.Instance.g_Processing; i++)
            {
                string name = keys[i].Substring(commonDir.Length).Replace('/', '_');
                entries[i].WriteToFile(Path.Combine(temporaryDir, name), options);
                SetProgress(DataAccess.Instance.g_curProgress + step);
            }
        }

        /// <summary>
        /// Longest folder path ("a/b/") shared by all the keys
        /// </summary>
        private static string CommonDirectory(List<string> keys)
        {
            if (keys.Count == 0)
                return string.Empty;
            string common = keys[0].Contains('/') ? keys[0].Substring(0, keys[0].LastIndexOf('/') + 1) : string.Empty;
            foreach (string key in keys)
                while (common.Length > 0 && !key.StartsWith(common, StringComparison.Ordinal))
                    common = common.Substring(0, common.TrimEnd('/').LastIndexOf('/') + 1);
            return common;
        }

        /// <summary>
        /// Extract the images of a PDF into the working folder, warning about sliced pages
        /// </summary>
        private void ExtractPdf(string file, int progressShare)
        {
            var result = PdfFunctions.PDF_ExportImage(file, temporaryDir, progressShare, true, _JoinImages);

            // report sliced pages even when they were merged back together
            if (result.ImagesBeforeMerge != result.Pages)
            {
                if (result.ImagesAfterMerge != result.ImagesBeforeMerge)
                    evnt_WarningNotify(this, string.Format("{0} : {1} images for {2} pages! Merged into {3} images.", file, result.ImagesBeforeMerge, result.Pages, result.ImagesAfterMerge));
                else
                    evnt_WarningNotify(this, string.Format("{0} : {1} images for {2} pages!", file, result.ImagesBeforeMerge, result.Pages));
            }
        }

        /// <summary>
        /// Generate the pdf from the images, one page per image at its original size
        /// </summary>
        private void GeneratePdf(string imageDir, double progressShare)
        {
            var imageFiles = Directory.GetFiles(imageDir).ToList();
            imageFiles.Sort();

            foreach (string skipped in imageFiles.Where(f => OtherImageExtensions.Contains(Path.GetExtension(f).ToLower())))
                evnt_WarningNotify(this, string.Format("{0} : {1} was left out of the PDF, this image type can't be put in a PDF", DataAccess.Instance.g_WorkingFile, Path.GetFileName(skipped)));
            imageFiles = imageFiles.Where(f => PdfImageExtensions.Contains(Path.GetExtension(f).ToLower())).ToList();
            if (imageFiles.Count == 0)
                throw new Exception("no images found to put in the PDF");

            string filename = GetAvailableOutputPath(".pdf");
            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.Open();

            double step = progressShare / imageFiles.Count;
            foreach (string imageFile in imageFiles)
            {
                if (!DataAccess.Instance.g_Processing)
                    break;
                try
                {
                    var img = iTextSharp.text.Image.GetInstance(imageFile);
                    document.SetPageSize(new iTextSharp.text.Rectangle(img.Width, img.Height));
                    document.NewPage();
                    img.SetAbsolutePosition(0, 0);
                    writer.DirectContent.AddImage(img);
                }
                catch (Exception ex)
                {
                    evnt_ErrorNotify(this, DataAccess.Instance.g_WorkingFile + " : " + Path.GetFileName(imageFile) + " : " + ex.Message);
                }
                SetProgress(DataAccess.Instance.g_curProgress + step);
            }

            document.Close();

            // stopped halfway: don't leave an incomplete PDF behind
            if (!DataAccess.Instance.g_Processing)
                File.Delete(filename);
        }

        /// <summary>
        /// Copy the extracted images into a folder named after the source file
        /// </summary>
        private void CopyToImageFolder(string imageDir)
        {
            string folder = GetAvailableOutputPath(string.Empty);
            Directory.CreateDirectory(folder);
            foreach (string file in Directory.GetFiles(imageDir))
                File.Copy(file, Path.Combine(folder, Path.GetFileName(file)));
        }

        private void SetProgress(double value)
        {
            DataAccess.Instance.g_curProgress = Math.Min(value, 100);
            evnt_UpdateCurBar();
        }

        /// <summary>
        /// Create an empty working folder in the system temp folder, so nothing next to the output is ever touched
        /// </summary>
        private string CreateWorkingDir()
        {
            string dir = Path.Combine(Path.GetTempPath(), "CbrConverter", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private void DeleteWorkingDir()
        {
            try
            {
                if (Directory.Exists(temporaryDir))
                    Directory.Delete(temporaryDir, true);
            }
            catch (Exception ex)
            {
                evnt_WarningNotify(this, "Could not delete temporary folder " + temporaryDir + " : " + ex.Message);
            }
        }

        /// <summary>
        /// Output path for the current file (a folder when the extension is empty); if it already exists,
        /// add " (1)", " (2)"... instead of overwriting
        /// </summary>
        private string GetAvailableOutputPath(string extension)
        {
            string path = outputBase + extension;
            if (!File.Exists(path) && !Directory.Exists(path))
                return path;

            int i = 1;
            string candidate;
            do
            {
                candidate = string.Format("{0} ({1}){2}", outputBase, i++, extension);
            } while (File.Exists(candidate) || Directory.Exists(candidate));

            evnt_WarningNotify(this, string.Format("{0} already exists, renaming to {1}", path, Path.GetFileName(candidate)));
            return candidate;
        }
    }
}
