﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
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

        private string RenderPicture(string pathToRootInputDirectory, string src)
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
            

            picture += $@"  <img class='img-responsive' src='{_tagToRender.Src}' alt='{_tagToRender.Alt}' />"; // fallback img tag
            picture += "</picture>";

            return picture;
        }
        

        private string RenderCaption(int ordinal)
        {
            var caption = "";
            var source = "";
            if (!string.IsNullOrWhiteSpace(_tagToRender.Source))
                if (!string.IsNullOrWhiteSpace(_tagToRender.SourceLink))
                    source =
                        $@" Źródło: <a href='{_tagToRender.SourceLink}' rel='nofollow' target='_blank'>{
                                _tagToRender.Source
                            }</a>";
                else
                    source = $@" Źródło: {_tagToRender.Source}";

            if (!string.IsNullOrWhiteSpace(_tagToRender.Caption))
                caption = $@"
                        <div class='caption'>
                            <figcaption>Rys. {ordinal}. {_tagToRender.Caption}.{source}
                            </figcaption>
                        </div>";
            return caption;
        }

        private string RenderError(string message)
        {
            return $"<p class='bg-danger text-danger render-error'>Image could not be rendered: {message}</p>";
        }
    }
}