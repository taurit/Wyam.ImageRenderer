using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Wyam.ImageRenderer.Tests
{
    [TestClass]
    public class FigTagRendererTests
    {
        [TestMethod]
        public void EmptyTag_ShouldRenderWithNoException()
        {
            var sut = new FigTagRenderer(new FigTag("<fig />"), new ImageFinder());
            var rendered = sut.Render(1, "test/");
            Console.WriteLine(rendered);
        }

        [TestMethod]
        public void WhenCaptionEndsWithDot_NoDotShouldBeAddedBeforeSource()
        {
            var sut = new FigTagRenderer(new FigTag("<fig src='a.png' caption='Description.' source='Example' source-link='http://example.com'/>"), new ImageFinder());

            // Act
            var rendered = sut.RenderCaption(1);
            Console.WriteLine(rendered);

            // Assert153022
            Assert.IsFalse(rendered.Contains(".."));
            Assert.IsTrue(rendered.Contains("Rys. 1. Description. Źródło: <a"));
        }

        [TestMethod]
        public void WhenCaptionEndsWithQuestionMark_DotShouldNotBeAdded()
        {
            var sut = new FigTagRenderer(new FigTag("<fig src='a.png' caption='Description?' source='Example' source-link='http://example.com'/>"), new ImageFinder());

            // Act
            var rendered = sut.RenderCaption(1);
            Console.WriteLine(rendered);

            // Assert
            Assert.IsFalse(rendered.Contains("?."));
            Assert.IsTrue(rendered.Contains("Rys. 1. Description? Źródło: <a"));
        }

        [TestMethod]
        public void WhenCaptionEndsWithExclamationMark_DotShouldNotBeAdded()
        {
            var sut = new FigTagRenderer(new FigTag("<fig src='a.png' caption='Description!' source='Example' source-link='http://example.com'/>"), new ImageFinder());

            // Act
            var rendered = sut.RenderCaption(1);
            Console.WriteLine(rendered);

            // Assert
            Assert.IsFalse(rendered.Contains("?."));
            Assert.IsTrue(rendered.Contains("Rys. 1. Description! Źródło: <a"));
        }


        [TestMethod]
        public void WhenLinkIsNotProvided_DescriptionStillContainsSource()
        {
            var sut = new FigTagRenderer(new FigTag("<fig src='a.png' caption='Description' source='Example' />"), new ImageFinder());

            // Act
            var rendered = sut.RenderCaption(1);
            Console.WriteLine(rendered);

            // Assert
            Assert.IsTrue(rendered.Contains("Rys. 1. Description. Źródło: Example"));
        }


        [TestMethod]
        public void ImageOrdinalNumber_ShouldBeVisibleInCaption()
        {
            var sut = new FigTagRenderer(new FigTag("<fig src='a.png' caption='Description' source='Example' />"), new ImageFinder());

            // Act
            var rendered = sut.RenderCaption(321);
            Console.WriteLine(rendered);

            // Assert
            Assert.IsTrue(rendered.Contains("Rys. 321."));
        }
    }
}