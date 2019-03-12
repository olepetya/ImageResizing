using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ImageResizing.BusinessLogic {
    public class ResizeBitmap {
        private readonly ILogger log;

        public ResizeBitmap(ILogger<ResizeBitmap> logger) {
            log = logger;
        }

        public bool ResizeAndSaveToFile(string file, int width, int height) {
            try {
                using (FileStream pngStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                    using (var image = new Bitmap(pngStream)) {
                        var resized = new Bitmap(width, height);

                        // Console.WriteLine($"New bitmp created: width {width} height:{height}");
                        log.LogInformation($"New bitmp created: width {width} height:{height}");
                        using (var graphics = Graphics.FromImage(resized)) {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(image, 0, 0, width, height);

                            log.LogInformation($"Try to save");
                            resized.Save(
                                $"{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}resized-{Path.GetFileName(file)}",
                                ImageFormat.Jpeg);
                            log.LogInformation(
                                $"Saved {Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}resized{Path.GetFileName(file)} thumbnail");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) {
                log.LogError(e.Message);
                return false;
            }
        }
    }
}