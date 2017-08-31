using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Wyam.ImageRenderer.FigHtmlTag
{
    public class ImageInstance
    {

        public ImageInstance(string rootDirectoryPath, string serverRelativePath, bool canBeUsedAsFallbackFormat)
        {
            Contract.Assert(!rootDirectoryPath.Contains("/"), "Path should use backslashes, not forward slashes");

            RootDirectoryPath = rootDirectoryPath;
            ServerRelativePath = serverRelativePath;
            CanBeUsedAsFallbackFormat = canBeUsedAsFallbackFormat;
        }

        public string RootDirectoryPath { get; }
        public string ServerRelativePath { get; }
        public bool CanBeUsedAsFallbackFormat { get; }

        public string Mimetype
        {
            get
            {
                Contract.Assert(ServerRelativePath != null);

                if (ServerRelativePath.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) ||
                    ServerRelativePath.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase))
                    return "image/jpeg";
                if (ServerRelativePath.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
                    return "image/png";
                if (ServerRelativePath.EndsWith("webp", StringComparison.InvariantCultureIgnoreCase))
                    return
                        "image/webp"; // currently not listed in IANA catalogue, https://www.iana.org/assignments/media-types/media-types.xhtml

                throw new NotImplementedException("Image extension not recognized");
            }
        }

        public string FullPath
        {
            get
            {
                var relativePart = ServerRelativePath;
                if (relativePart.StartsWith("/"))
                    relativePart = relativePart.Substring(1);
                relativePart = relativePart.Replace('/', '\\');
                return Path.Combine(RootDirectoryPath, relativePart);
            }
        }
    }
}