﻿using System.IO;
using System.Web;
using GridMvc.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Tests.Pagination
{
    [TestClass]
    public class GridPagerTests
    {
        private GridPager _pager;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            _pager = new GridPager();
        }

        [TestMethod]
        public void PagerPageCountTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            Assert.AreEqual(_pager.PageCount, 93);
        }

        [TestMethod]
        public void PagerDisplayingPagesTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;

            _pager.MaxDisplayedPages = 5;
            _pager.CurrentPage = 40;

            Assert.AreEqual(_pager.TemplateName, "_GridPager");
            Assert.AreEqual(_pager.StartDisplayedPage, 38);
            Assert.AreEqual(_pager.EndDisplayedPage, 42);
        }

        [TestMethod]
        public void PagerCurrentPageTest()
        {
            _pager.ItemsCount = 1200;
            _pager.PageSize = 13;
            _pager.CurrentPage = 1000;

            Assert.AreEqual(_pager.PageCount, _pager.CurrentPage);
        }


    }
}
