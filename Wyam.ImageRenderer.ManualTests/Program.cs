﻿using System;
using System.Diagnostics;
using Wyam.ImageRenderer.FigHtmlTag;

namespace Wyam.ImageRenderer.ManualTests
{
    /// <summary>
    ///     Program for quick manual testing, since running tests in debug mode fails for this project in my VS and I have no
    ///     time to diagnose why.
    /// </summary>
    internal class Program
    {
        private const string HtmlFragment = @"some content before <fig width='6'
            src='/assets/img/posts/pavlovs-dogs-mark-stivers.png'
            alt='Gadżet ułatwiający rozpoczęcie pracy: zwykły minutnik kuchenny'
            caption='Pies Pawłowa'
            source='Mark Stivers'
            source-link = 'http://www.markstivers.com/wordpress/'
                            /> some content after <b>tag</b>";

        private static void Main(string[] args)
        {
            ImageFinder sut = new ImageFinder();
            var alternativePaths = sut.GetPotentialInstances("d:\\Projekty\\Taurit.Blog.input", "/assets/img/posts/pavlovs-dogs-mark-stivers.png");


            //var sut = new FigTagFinder(HtmlFragment);
            //var tags = sut.FoundTags;
            //Debug.Assert(tags.Count == 1);
            //Debug.Assert(tags[0].Width == 6);

            //var renderedHtml = new FigTagRenderer(tags[0], new ImageFinder()).Render(123, "/");
            //Console.WriteLine(renderedHtml);
            //Console.ReadLine();
        }
    }
}