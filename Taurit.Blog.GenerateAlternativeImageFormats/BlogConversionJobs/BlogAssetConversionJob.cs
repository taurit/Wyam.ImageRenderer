using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Taurit.Blog.GenerateAlternativeImageFormats.ImageConverters;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Taurit.Blog.GenerateAlternativeImageFormats.BlogConversionJobs
{
    internal class BlogAssetConversionJob
    {
        private readonly IImageConverter _imageConverter;
        private readonly ImageFinder _imageFinder;

        public BlogAssetConversionJob(ImageFinder imageFinder, IImageConverter imageConverter)
        {
            _imageFinder = imageFinder;
            _imageConverter = imageConverter;
        }
        
        public void Convert(string imagesDirectory, string rootDir, string relativePathPrefix)
        {
            Contract.Assert(imagesDirectory != null);
            Contract.Assert(Directory.Exists(imagesDirectory));

            var files = Directory.GetFiles(imagesDirectory);
            var allConvertibleImagesPaths = files
                .Where(filePath => filePath.EndsWith(".hq.jpg") || filePath.EndsWith(".hq.png"))
                .ToList();

            if (allConvertibleImagesPaths.Count == 0)
                Console.WriteLine($"\tNo HQ files were found, nothing to convert.");

            foreach (var inputImagePath in allConvertibleImagesPaths)
            {
                Console.WriteLine($"\tAnalyzing {inputImagePath}");

                var fileName = new FileInfo(inputImagePath).Name;
                var recognizedWebPPaths = _imageFinder.GetPotentialInstances(rootDir, relativePathPrefix + fileName);
                var preferredWebPPath = recognizedWebPPaths.First();
                var outputImagePath = preferredWebPPath.FullPath.Replace(".hq.", ".");

                if (File.Exists(outputImagePath))
                {
                    Console.WriteLine($"\t\tSkipping, optimized file already exist");
                }
                else
                {
                    Console.WriteLine($"\t\tConverting to {outputImagePath}");
                    _imageConverter.ConvertToWebP(inputImagePath, outputImagePath, 80);
                }
            }
        }
    }
}