using System.Collections.Generic;
using System.IO;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.Modules;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Wyam.ImageRenderer
{
    public class ImageRenderer : IModule
    {
        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            var documents = new List<IDocument>();

            foreach (var input in inputs)
            {
                var contentBefore = new StreamReader(input.GetStream()).ReadToEnd();
                var contentAfter = FixImages(contentBefore);
                var modifiedContentAsStream = context.GetContentStream(contentAfter);

                var doc = context.GetDocument(input, modifiedContentAsStream, new Dictionary<string, object>());
                documents.Add(doc);
            }

            return documents;
        }

        private string FixImages(string contentBefore)
        {
            var newContent = contentBefore;
            var foundFigTags = new FigTagFinder(contentBefore).FoundTags;
            var ordinal = 1;
            var imageFinder = new ImageFinder();
            foreach (var figTag in foundFigTags)
            {
                var renderer = new FigTagRenderer(figTag, imageFinder);
                var renderedPicture = renderer.Render(ordinal++, @"d:\Projekty\Taurit.Blog\input\");
                newContent = newContent.Replace(figTag.RawHtml, renderedPicture);
            }
            return newContent;
        }
    }
}