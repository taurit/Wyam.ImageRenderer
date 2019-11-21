using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wyam.ImageRenderer.FigHtmlTag
{
    public class ImageFinder
    {
        public List<ImageInstance> GetAllFormatsOfImage(string pathToRootInputDirectory, string relativePath)
        {
            var allInstances = new List<ImageInstance>(2); // expected: original jpg/gif and webp

            // try some paths by convention (in preferred order)
            var potential = GetPotentialInstances(pathToRootInputDirectory, relativePath);
            allInstances.AddRange(potential.Where(x => File.Exists(x.FullPath)));

            // add original - last, as it's the most common but also least compressed format
            var baseImage = new ImageInstance(pathToRootInputDirectory, relativePath, false);
            if (File.Exists(baseImage.FullPath))
                allInstances.Add(baseImage);
            
            return allInstances;
        }

        public List<ImageInstance> GetPotentialInstances(string pathToRootInputDirectory, string relativePath)
        {
            relativePath = relativePath.Replace(".hq.", ".");

            var potentialPaths = new List<ImageInstance>(4);
            var relativeDirectory = relativePath.Substring(0, relativePath.LastIndexOf('/'));
            var fileNameWithExtension = new FileInfo(relativePath).Name;
            var originalExtensionWithDot = new FileInfo(relativePath).Extension;
            var fileNameWithoutExtension = fileNameWithExtension.Substring(0, fileNameWithExtension.Length - originalExtensionWithDot.Length);

            // order: from the most to the least preferred

            // Eg. /assets/img/image.jpg -> /assets/img/webp/image.webp
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, $"{relativeDirectory}/webp/{fileNameWithoutExtension}.webp", false));

            // Eg. /assets/img/image.jpg -> /assets/img/webp/image.jpg.webp
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, $"{relativeDirectory}/webp/{fileNameWithoutExtension}{originalExtensionWithDot}.webp", false));

            // Eg. /assets/img/image.jpg -> /assets/img/image.jpg.webp
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, relativePath + ".webp", false));

            // Eg. /assets/img/image.jpg -> /assets/img/image.webp
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, $"{relativeDirectory}/{fileNameWithoutExtension}.webp", false));

            // Eg. /assets/img/image.jpg -> /assets/img/jpeg/image.jpg
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, $"{relativeDirectory}/jpeg/{fileNameWithoutExtension}.jpg", true));

            // Eg. /assets/img/image.jpg -> /assets/img/png/image.png
            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, $"{relativeDirectory}/png/{fileNameWithoutExtension}.png", true));

            potentialPaths.Add(new ImageInstance(pathToRootInputDirectory, relativePath, true));

            return potentialPaths;
        }


    }
}