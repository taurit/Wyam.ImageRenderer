using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyam.ImageExtender.FigHtmlTag;

namespace Wyam.ImageExtender.Tests
{
    [TestClass]
    public class TagFinderTests
    {
        private const string HtmlFragment = @"some content before <fig width='6'
            src='/assets/img/posts/pavlovs-dogs-mark-stivers.png'
            alt='Gadżet ułatwiający rozpoczęcie pracy: zwykły minutnik kuchenny'
            caption='Pies Pawłowa'
            source='Mark Stivers'
            source-link = 'http://www.markstivers.com/wordpress/'
                            /> some content after <b>tag</b>";


        [TestMethod]
        public void WhenTagIsPresent_ItShouldBeFoundAndParsed()
        {
            var sut = new FigTagFinder(HtmlFragment);
            var tags = sut.FoundTags;
            Assert.AreEqual(1, tags.Count);
            Assert.AreEqual(6, tags[0].Width);
            Assert.AreEqual("Gadżet ułatwiający rozpoczęcie pracy: zwykły minutnik kuchenny", tags[0].Alt);
            Assert.AreEqual("Pies Pawłowa", tags[0].Caption);
            Assert.AreEqual("Mark Stivers", tags[0].Source);
            Assert.AreEqual("http://www.markstivers.com/wordpress/", tags[0].SourceLink);
        }

        [TestMethod]
        public void WhenWidthIsNotPresent_DefaultValueIs12()
        {
            var sut = new FigTagFinder("<fig />");
            var tags = sut.FoundTags;
            Assert.AreEqual(1, tags.Count);
            Assert.AreEqual(12, tags[0].Width);
        }

        [TestMethod]
        public void WhenTwoTagsArePresents_BothShouldBeFound()
        {
            var sut = new FigTagFinder("<fig width='1'/><fig width='2' />");
            var tags = sut.FoundTags;
            Assert.AreEqual(2, tags.Count);
            Assert.AreEqual(1, tags[0].Width);
            Assert.AreEqual(2, tags[1].Width);
        }


        [TestMethod]
        public void TagsShouldContainTheirRawHtmlRepresentation()
        {
            var sut = new FigTagFinder("<fig width='1'/><fig aaa width='2' />");
            var tags = sut.FoundTags;
            Assert.AreEqual(2, tags.Count);
            Assert.AreEqual("<fig width='1'/>", tags[0].RawHtml);
            Assert.AreEqual("<fig aaa width='2' />", tags[1].RawHtml);
        }
    }
}