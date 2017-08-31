using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Taurit.BlogpostImageConverter
{
    class BlogpostImageConverter
    {
        static void Main(string[] args)
        {
            var converter = new BlogpostImageConverter(@"d:\Projekty\Taurit.Blog\input\assets\img\posts\",
                @"d:\Projekty\Taurit.Blog\input\",
                @"/assets/img/posts/"
                );
        }

        ImageFinder _imageFinder = new ImageFinder();
        private BlogpostImageConverter(string imagesDirectory, string rootDir, string relativePathPrefix)
        {
            Contract.Assert(imagesDirectory != null);
            Contract.Assert(Directory.Exists(imagesDirectory));

            var files = Directory.GetFiles(imagesDirectory);
            var allConvertibleImagesPaths = files.Where(filePath => filePath.EndsWith(".hq.jpg") || filePath.EndsWith(".hq.png"))
                .ToList();
            
            foreach (var inputImagePath in allConvertibleImagesPaths)
            {
                var fileName = new FileInfo(inputImagePath).Name;
                var preferredWebPPath =
                    _imageFinder.GetPotentialInstances(rootDir,relativePathPrefix + fileName).First();
                var outputImagePath = preferredWebPPath.FullPath.Replace(".hq.", ".");
                if (!File.Exists(outputImagePath)) // do not convert/override existing
                {
                    RunConvert(inputImagePath, outputImagePath, 80);
                }
            }

        }
        
        private static void RunConvert([NotNull] string inputFile, [NotNull] string outputFile, int quality)
        {
            Contract.Assert(inputFile != null);
            Contract.Assert(outputFile != null);
            Contract.Assert(quality > 0);

            // this requires imagemagick to be installed and added to windows path
            Process magickProcess = new Process();
            magickProcess.StartInfo.FileName = "magick";
            magickProcess.StartInfo.Arguments = $"convert \"{inputFile}\" -quality {quality} -define webp:lossless=false \"{outputFile}\"";

            magickProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            magickProcess.Start();
            magickProcess.WaitForExit();
        }
    }

}

