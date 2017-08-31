using System.Collections.Generic;
using System.IO;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.Modules;
using Wyam.ImageExtender.FigHtmlTag;

namespace Wyam.ImageExtender
{
    public class ImageExtender : IModule
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
            foreach (var figTag in foundFigTags)
            {
                var renderedPicture =
                    new FigTagRenderer(figTag, ordinal++, @"d:\Projekty\Taurit.Blog\input\").Render();
                newContent = newContent.Replace(figTag.RawHtml, renderedPicture);
            }
            return newContent;
        }
    }
}