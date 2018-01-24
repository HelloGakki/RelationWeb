﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Linkman.Domain.Entities;
using Linkman.Domain.Abstract;
using Linkman.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using Linkman.WebUI.Models;
using System;
using System.Web.Mvc;
using Linkman.WebUI.HtmlHelpers;

namespace Linkman.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Pageinate()
        {
            // 准备
            Mock<IPeopleRepository> mock = new Mock<IPeopleRepository>();
            mock.Setup(m => m.People).Returns(new Person[]
                {
                    new Person { PersonID = 1, Name = "P1"},
                    new Person { PersonID = 2, Name = "P2"},
                    new Person { PersonID = 3, Name = "P3"},
                    new Person { PersonID = 4, Name = "P4"},
                    new Person { PersonID = 5, Name = "P5"}
                });
            PersonController controller = new PersonController(mock.Object);
            controller.PageSize = 3;

            // 动作
            IEnumerable<Person> result = (IEnumerable<Person>)controller.List(2).Model;

            // 断言
            Person[] personArray = result.ToArray();
            Assert.IsTrue(personArray.Length == 2);
            Assert.AreEqual(personArray[0].Name, "P4");
            Assert.AreEqual(personArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Link()
        {
            HtmlHelper myHtmlHelper = null;

            // 准备 - 创建paginginfo数据
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItem = 28,
                ItemsPerPage = 10
            };
            // 准备 - lambda表达式建立委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // 动作
            MvcHtmlString result = myHtmlHelper.PageLinks(pagingInfo, pageUrlDelegate);
            System.Diagnostics.Debug.WriteLine("during :" + result.ToString());
            // 断言
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" + 
                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
                @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }
    }
}