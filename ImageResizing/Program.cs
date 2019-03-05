using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageResizing
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 128;
            int height = 128;
            var file = args[0];
            Console.WriteLine($"Loading {file}");
            Console.WriteLine(
                $"path: {Path.GetFullPath(file)} directory:{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}");
            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out width))
                {
                    width = 128;
                }

                if (args.Length < 2 || (args.Length > 2 && !int.TryParse(args[2], out height)))
                {
                    height = 128;
                }

                Console.WriteLine($"New size: width {width} height:{height}");
            }

            using (FileStream pngStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (var image = new Bitmap(pngStream))
            {
                var resized = new Bitmap(width, height);

                Console.WriteLine($"New bitmp created: width {width} height:{height}");
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);

                    Console.WriteLine($"Try to save");
                    resized.Save(
                        $"{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}resized-{Path.GetFileName(file)}",
                        ImageFormat.Jpeg);
                    Console.WriteLine(
                        $"Saved {Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}resized{Path.GetFileName(file)} thumbnail");
                }
            }
        }
    }
}