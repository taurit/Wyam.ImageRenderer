using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using JetBrains.Annotations;
using Taurit.Blog.GenerateAlternativeImageFormats.BlogConversionJobs;
using Taurit.Blog.GenerateAlternativeImageFormats.ImageConverters;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Taurit.Blog.GenerateAlternativeImageFormats
{
    internal class GenerateAlternativeImageFormats
    {
        /// <summary>
        ///     Quick and dirty program to create alternative representations for my blog images.
        ///     Most notably, WebP format is generated from source jpeg/gif files (to improve compression and therefore
        ///     page speed in browsers that support it).
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Converting assets...");
            ConvertAssets(new ImageFinder(), new ImageMagickConverter());

            Console.WriteLine("Converting thumbnails...");
            ConvertThumbnails(new ImageMagickConverter());
        }

        /// <summary>
        ///     Convert thumbnails for blog posts into WebP format.
        /// </summary>
        /// <param name="imageConverter"></param>
        private static void ConvertThumbnails(IImageConverter imageConverter)
        {
            var thumbnailConverter = new ThumbnailConversionJob(imageConverter);
            thumbnailConverter.Convert(@"d:\Projekty\Taurit.Blog\input\assets\img\thumbnails\"); // quality range is 1-100 I think
        }

        /// <summary>
        ///     Convert images used in blog posts into WebP format.
        /// </summary>
        /// <param name="imageFinder"></param>
        /// <param name="imageConverter"></param>
        private static void ConvertAssets(ImageFinder imageFinder, IImageConverter imageConverter)
        {
            var blogAssetsConverter = new BlogAssetConversionJob(imageFinder, imageConverter);
            blogAssetsConverter.Convert(@"d:\Projekty\Taurit.Blog\input\assets\img\posts\",
                @"d:\Projekty\Taurit.Blog\input\",
                @"/assets/img/posts/");
        }
    }
}