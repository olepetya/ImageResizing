using System;
using System.IO;
using ImageResizing.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageResizing {
    class Program {
        static void Main(string[] args) {
            int width = 128;
            int height = 128;
            if (args.Length < 1) {
                Console.WriteLine("Enter Image FileName as parameter to resize");
                return;
            }

            var file = args[0];
            Console.WriteLine($"Loading {file}");
            Console.WriteLine(
                $"path: {Path.GetFullPath(file)} directory:{Path.GetDirectoryName(file)}{Path.DirectorySeparatorChar}");
            if (args.Length > 1) {
                if (!int.TryParse(args[1], out width)) {
                    width = 128;
                }

                if (args.Length < 2 || (args.Length > 2 && !int.TryParse(args[2], out height))) {
                    height = 128;
                }

                Console.WriteLine($"New size: width {width} height:{height}");
            }

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            ResizeBitmap worker = serviceProvider.GetService<ResizeBitmap>();

            worker.ResizeAndSaveToFile(file, width, height);
        }

        private static void ConfigureServices(IServiceCollection services) {
            //we will configure logging here

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<ResizeBitmap>();
        }
    }
}