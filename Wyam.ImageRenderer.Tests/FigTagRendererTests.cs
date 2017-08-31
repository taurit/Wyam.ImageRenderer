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
            var sut = new FigTagRenderer(new FigTag("<fig />"), 666, null);
            var rendered = sut.Render();
            Console.WriteLine(rendered);
        }
    }
}