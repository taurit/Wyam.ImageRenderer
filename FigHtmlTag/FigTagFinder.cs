using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Wyam.ImageRenderer.FigHtmlTag
{
    public class FigTagFinder
    {
        private readonly List<FigTag> _foundTags;

        public FigTagFinder(string htmlContent)
        {
            var tags = FindImageTags(htmlContent);
            _foundTags = tags;
        }

        public IReadOnlyList<FigTag> FoundTags => _foundTags;

        [Pure]
        private List<FigTag> FindImageTags(string htmlContent)
        {
            var foundTags = new List<FigTag>(5); // should be no more than 5 images
            var finder = new Regex(@"\<fig\s.*?\/\>", RegexOptions.Singleline);

            var m = finder.Match(htmlContent);
            while (m.Success)
            {
                var tag = new FigTag(m.Groups[0].Value);
                foundTags.Add(tag);

                m = m.NextMatch();
            }
            return foundTags;
        }
    }
}