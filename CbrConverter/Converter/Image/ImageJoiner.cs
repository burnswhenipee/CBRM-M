using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;


namespace CbrConverter
{
    internal class ImageJoiner
    {
        /// <summary>
        /// if needed, will merge the images base on page in the image names
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="imageNames"></param>
        /// <returns></returns>
        public Dictionary<PageImageIndex, System.Drawing.Image> Merge(Dictionary<PageImageIndex, System.Drawing.Image> imagesList, string outputPath)
        {
            var imageListByPage = imagesList.GroupBy(i => i.Key.PageIndex);
            var newImageList = new Dictionary<PageImageIndex, System.Drawing.Image>();

            foreach (var imagesInfo in imageListByPage)
            {
                // a page made of one image is kept exactly as extracted
                if (imagesInfo.Count() == 1)
                {
                    var single = imagesInfo.First();
                    newImageList.Add(single.Key, single.Value);
                    continue;
                }

                var imagesPage = new Dictionary<PageImageIndex, System.Drawing.Image>();

                foreach (var item in imagesInfo)
                {
                    var image = item.Value;
                    if (item.Value == null)
                    {
                        var imagePath = string.Format(@"{0}\{1}", outputPath, item.Key.ImageName);
                        image = new System.Drawing.Bitmap(imagePath);
                    }

                    imagesPage.Add(item.Key, image);

                }

                //var imagesPage = imagesInfo.Select(i => i).ToDictionary(x => x.Key, x => x.Value);
                var keyValue = MergeGroup(imagesPage, outputPath);
                newImageList.Add(keyValue.Key, keyValue.Value);
            }

            return newImageList;
        }

        /// <summary>
        /// will merge a group of images into one
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="imageNames"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        public KeyValuePair<PageImageIndex, System.Drawing.Image> MergeGroup(Dictionary<PageImageIndex, System.Drawing.Image> groupImages, string outputPath)
        {
            int maxWidth = 0, maxHeight = 0, position = 0;

            foreach (var imageInfo in groupImages)
            {
                maxWidth = Math.Max(imageInfo.Value.Width, maxWidth);
                maxHeight += imageInfo.Value.Height;
            }

            // Create new image with max width and sum of all height
            System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newImage);
            // copy the pixels exactly, without smoothing or resampling
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            // merge all images
            g.Clear(System.Drawing.SystemColors.AppWorkspace);
            foreach (var img in groupImages)
            {
                if (position == 0)
                {
                    g.DrawImage(img.Value, 0, 0, img.Value.Width, img.Value.Height);
                    position++;
                    maxHeight = img.Value.Height;
                }
                else
                {
                    g.DrawImage(img.Value, 0, maxHeight, img.Value.Width, img.Value.Height);
                    maxHeight += img.Value.Height;
                }

                img.Value.Dispose();
                var imagePath = string.Format(@"{0}\{1}", outputPath, img.Key.ImageName);
                File.Delete(imagePath);
            }

            g.Dispose();

            var pageDetails = new PageImageIndex
            {
                PageIndex = groupImages.First().Key.PageIndex,
                ImageIndex = 1,
                ImageName = string.Format("{0:0000}_{1:0000}.png", groupImages.First().Key.PageIndex, 1)
            };

            var newImagePath = string.Format(@"{0}\{1}", outputPath, pageDetails.ImageName);
            newImage.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Png);
            newImage.Dispose();

            return new KeyValuePair<PageImageIndex, System.Drawing.Image>(pageDetails, null);
        }
    }
}
