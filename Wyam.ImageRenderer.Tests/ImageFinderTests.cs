using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Wyam.ImageRenderer.Tests
{
    [TestClass]
    public class ImageFinderTests
    {
        [TestMethod]
        public void FileWithAddedWebpExtension_ShouldBeFound()
        {
            ImageFinder sut = new ImageFinder();
            var alternativePaths = sut.GetPotentialInstances("d:\\Projekty\\Taurit.Blog\\input\\", "/assets/img/image.jpg");

            Assert.AreEqual(1, alternativePaths.Count(path => path.ServerRelativePath == "/assets/img/image.jpg.webp"));
            Assert.AreEqual(1, alternativePaths.Count(path => path.ServerRelativePath == "/assets/img/image.webp"));
            Assert.AreEqual(1,
                alternativePaths.Count(path => path.ServerRelativePath == "/assets/img/webp/image.jpg.webp"));
            Assert.AreEqual(1,
                alternativePaths.Count(path => path.ServerRelativePath == "/assets/img/webp/image.webp"));

            Console.WriteLine("Found paths:");
            foreach (var path in alternativePaths)
            {
                Console.WriteLine(path.ServerRelativePath);
            }
        }

        [TestMethod]
        public void FullPath_ShouldBeAProperWindowsPath()
        {
            ImageFinder sut = new ImageFinder();
            var alternativePaths =
                sut.GetPotentialInstances("d:\\Projekty\\Taurit.Blog\\input\\", "/assets/img/image.jpg");

            var firstGeneratedPath = alternativePaths.First().FullPath;
            Console.WriteLine(firstGeneratedPath);
            Assert.IsTrue(firstGeneratedPath.StartsWith("d:\\Projekty\\Taurit.Blog\\input\\"));
            Assert.IsTrue(!firstGeneratedPath.Contains("/"));


        }
    }
}
