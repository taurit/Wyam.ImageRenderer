using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyam.ImageExtender.FigHtmlTag;

namespace Wyam.ImageExtender.Tests
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