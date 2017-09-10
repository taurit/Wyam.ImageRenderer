using System.Diagnostics;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace Taurit.Blog.GenerateAlternativeImageFormats.ImageConverters
{

    internal class ImageMagickConverter : IImageConverter
    {
        /// <summary>
        ///     Synchronously converts file using ImageMagick converter
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="quality"></param>
        public void ConvertToWebP([NotNull] string inputFile, [NotNull] string outputFile,
            int quality)
        {
            Contract.Assert(inputFile != null);
            Contract.Assert(outputFile != null);
            Contract.Assert(quality > 0);
            Contract.Assert(quality <= 100);

            // this requires ImageMagick to be installed and added to windows path
            var magickProcess = new Process();
            magickProcess.StartInfo.FileName = "magick";
            magickProcess.StartInfo.Arguments =
                $"convert \"{inputFile}\" -quality {quality} -define webp:lossless=false \"{outputFile}\"";

            magickProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            magickProcess.Start();
            magickProcess.WaitForExit();
        }
    }
}