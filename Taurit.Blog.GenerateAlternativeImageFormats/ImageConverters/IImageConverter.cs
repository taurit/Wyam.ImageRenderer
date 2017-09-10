using JetBrains.Annotations;

namespace Taurit.Blog.GenerateAlternativeImageFormats.ImageConverters
{
    internal interface IImageConverter
    {
        /// <summary>
        ///     Synchronously converts file using ImageMagick converter
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="quality"></param>
        void ConvertToWebP([NotNull] string inputFile, [NotNull] string outputFile,
            int quality);
    }
}