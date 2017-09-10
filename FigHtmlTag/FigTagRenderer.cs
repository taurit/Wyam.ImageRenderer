using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Wyam.ImageRenderer.FigHtmlTag
{
    public class FigTagRenderer
    {
        private readonly FigTag _tagToRender;
        private readonly ImageFinder _alternativeImageFormatFinder;

        public FigTagRenderer([NotNull] FigTag tagToRender, ImageFinder alternativeImageFormatFinder)
        {
            _tagToRender = tagToRender;
            _alternativeImageFormatFinder = alternativeImageFormatFinder;
        }

        public string Render(int ordinal, [NotNull] string pathToRootInputDirectory)
        {
            // validate input parameters
            if (_tagToRender.Width < 0 || _tagToRender.Width > 12)
                return RenderError("Width is not valid, should be from 1-12 range (in bootstrap columns unit)");
            if (_tagToRender.Width % 2 != 0)
                return RenderError("Width is not valid, should be even to allow center image");
            if (_tagToRender.Src == null)
                return RenderError("Src must not be null");
            if (ordinal < 1)
                return RenderError("Figure number (ordinal) must be >= 1");

            var caption = RenderCaption(ordinal); // render caption and source if present
            var picture =
                RenderPicture(pathToRootInputDirectory,
                    _tagToRender.Src); // render picture element with <img> inside and alternative formats like WebP

            //language=html
            var renderedTag = $@"
            <div class='row' aria-hidden='true'>
                <figure class='col-md-{_tagToRender.Width} col-md-offset-{(12 - _tagToRender.Width) / 2}'>
                    <div class='thumbnail'>
                        {picture}
                        {caption}
                    </div>
                </figure>
            </div>";
            return renderedTag;
        }

        internal string RenderPicture(string pathToRootInputDirectory, string src)
        {
            Contract.Assert(pathToRootInputDirectory != null);
            Contract.Assert(Directory.Exists(pathToRootInputDirectory));
            Contract.Assert(!string.IsNullOrWhiteSpace(src));

            List<ImageInstance> allFormats = _alternativeImageFormatFinder.GetAllFormatsOfImage(pathToRootInputDirectory, src);

            string picture = "<picture>";
            foreach (var imageFormat in allFormats)
            {
                picture += $@"  <source srcset='{imageFormat.ServerRelativePath}' type='{imageFormat.Mimetype}' />";
            }

            var imageThatCanBeUsedAsFallback = allFormats.Where(image => image.CanBeUsedAsFallbackFormat).ToList();
            if (imageThatCanBeUsedAsFallback.Count == 0)
                throw new ArgumentException($"No image representation was found that can be used as fallback format for '{src}'. Total images found: {allFormats.Count}");

            var defaultImage = imageThatCanBeUsedAsFallback.First(image => image.CanBeUsedAsFallbackFormat); // first in order is the most preferred one

            if (_tagToRender.LinkToFullImage)
            {
                picture += $"<a href='{defaultImage.ServerRelativePath}' class='img-generated-link' target='_blank'>";
            }
            picture += $@"  <img class='img-responsive img-generated' src='{defaultImage.ServerRelativePath}' alt='{_tagToRender.Alt}' />"; // fallback img tag
            if (_tagToRender.LinkToFullImage)
            {
                picture += "</a>";
            }

            picture += "</picture>";

            return picture;
        }


        internal string RenderCaption(int ordinal)
        {
            var source = RenderSource(_tagToRender.Source, _tagToRender.SourceLink);
            var captionWithPunctuationMark = _tagToRender.CaptionWithPunctuationMark;
            var separator = (String.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(captionWithPunctuationMark) ? "" : " ");
            
            return $@"<div class='caption'>
                            <figcaption>Rys. {ordinal}. {captionWithPunctuationMark}{separator}{source}
                            </figcaption>
                        </div>";
        }

        internal string RenderSource(string sourceDescription, string sourceLink)
        {
            var source = "";
            if (!string.IsNullOrWhiteSpace(sourceDescription))
            {
                if (!string.IsNullOrWhiteSpace(sourceLink))
                {
                    source =
                        $@"Źródło: <a href='{_tagToRender.SourceLink}' rel='nofollow' target='_blank'>{
                                _tagToRender.Source
                            }</a>";
                }
                else
                {
                    source = $@"Źródło: {_tagToRender.Source}";
                }
            }

            return source;
        }

        internal string RenderError(string message)
        {
            return $"<p class='bg-danger text-danger render-error'>Image could not be rendered: {message}</p>";
        }
    }
}