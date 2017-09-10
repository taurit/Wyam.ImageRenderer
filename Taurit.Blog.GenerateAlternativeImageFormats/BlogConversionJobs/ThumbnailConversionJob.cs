using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Taurit.Blog.GenerateAlternativeImageFormats.ImageConverters;

namespace Taurit.Blog.GenerateAlternativeImageFormats.BlogConversionJobs
{
    internal class ThumbnailConversionJob
    {
        private readonly IImageConverter _imageConverter;

        public ThumbnailConversionJob(IImageConverter imageConverter)
        {
            _imageConverter = imageConverter;
        }

        internal void Convert([NotNull] string pathToThumbnailsDirectory)
        {
            Contract.Assert(pathToThumbnailsDirectory != null);
            Contract.Assert(Directory.Exists(pathToThumbnailsDirectory));

            var quality = 80;
            Contract.Assert(quality > 0);
            Contract.Assert(quality <= 100);

            var filesInDirectory = GetFilesInDirectory(pathToThumbnailsDirectory);

            foreach (var filePath in filesInDirectory)
            {
                var fileInfo = new FileInfo(filePath);
                var extension = fileInfo.Extension.ToLowerInvariant();
                var isJpeg = extension == ".jpg" || extension == ".jpeg";
                if (isJpeg)
                {
                    System.Console.WriteLine($"\tAnalyzing {filePath}");
                    var webPPath = fileInfo.Directory + "\\" + fileInfo.Name + ".webp";

                    if (File.Exists(webPPath))
                    {
                        // System.Console.WriteLine($"\t\tCompressed file already exists, skipping.");
                        // typical situation, don't output to preserve readability
                    }
                    else
                    {
                        System.Console.WriteLine($"\t\tConverting to WebP...");
                        _imageConverter.ConvertToWebP(filePath, webPPath, quality);
                    }   
                }
            }
        }

        [JetBrains.Annotations.Pure]
        private static List<string> GetFilesInDirectory(string directory)
        {
            if (File.Exists(directory)) return new List<string> {directory};

            return Directory.GetFiles(directory).ToList();
        }
    }
}