﻿using System;
using HtmlAgilityPack;

namespace Wyam.ImageRenderer.FigHtmlTag
{
    public class FigTag
    {
        private readonly HtmlNode _figNode;

        public FigTag(string fullHtmlMatch)
        {
            RawHtml = fullHtmlMatch;

            var doc = new HtmlDocument();
            doc.LoadHtml(fullHtmlMatch);
            _figNode = doc.DocumentNode.ChildNodes[0];
        }

        /// <summary>
        ///     Bootstrap width in 'number of columns' unit (range 1-12). Preferably even, so it can be easily centered.
        /// </summary>
        public int Width => Convert.ToInt32(_figNode.Attributes["width"]?.Value ?? "12");

        public string Src => _figNode.Attributes["src"]?.Value;
        public string Alt => _figNode.Attributes["alt"]?.Value ?? ""; // alt="" is valid and better than no alt
        public string Caption => _figNode.Attributes["caption"]?.Value;

        public bool LinkToFullImage
        {
            get
            {
                string userProvidedValue = _figNode.Attributes["link-to-full-image"]?.Value;
                bool result = false;
                Boolean.TryParse(userProvidedValue, out result);

                return result;
            }
        }


    public string CaptionWithPunctuationMark
        {
            get
            {
                var caption = Caption;
                if (String.IsNullOrWhiteSpace(caption))
                    return string.Empty;
                if (!(caption.EndsWith(".") || caption.EndsWith("!") || caption.EndsWith("?")))
                    caption += ".";
                return caption;
            }
        }
        public string Source => _figNode.Attributes["source"]?.Value;
        public string SourceLink => _figNode.Attributes["source-link"]?.Value;
        public string RawHtml { get; }
    }
}