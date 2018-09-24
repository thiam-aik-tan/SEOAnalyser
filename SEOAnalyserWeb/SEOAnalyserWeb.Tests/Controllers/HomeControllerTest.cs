using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SEOAnalyser.Components;
using SEOAnalyserWeb;
using SEOAnalyserWeb.Controllers;
using SEOAnalyserWeb.Models;
using SEOAnalyserWeb.Tests.Helpers;

namespace SEOAnalyserWeb.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = GetHomeController(new UrlUtility());

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Result()
        {
            // Arrange
            var controller = GetHomeController(new UrlUtility());

            // Act
            var result = controller.Result() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RequiredField_ExpectMissingRequiredField()
        {
            // Arrange
            var controller = GetHomeController(new UrlUtility());
            var model = new AnalysisModel
            {
                Text = string.Empty
            };

            // Act
            var result = controller.Index(model) as ActionResult;
            var results = ValidationHelper.Validate(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreNotEqual(0, results.Count);
        }

        [TestMethod]
        public void TextAnalysis_Success()
        {
            // Arrange
            var controller = GetHomeController(new UrlUtility());
            var model = new AnalysisModel
            {
                Text = "This is a test."
            };

            // Act
            var result = controller.Index(model) as ActionResult;
            var results = ValidationHelper.Validate(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual("Result", viewResult.ViewName);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void InvalidUrl_ExpectError()
        {
            // Arrange
            var htmlContent = string.Empty;
            var model = new AnalysisModel
            {
                Text = "http://www.invalid.url"
            };
            var mock = new Mock<IUrlUtility>();
            mock.Setup(m => m.IsValidUrl(model.Text)).Returns(true);
            mock.Setup(m => m.TryGetContent(model.Text, out htmlContent)).Returns(false);
            var controller = GetHomeController(mock.Object);

            // Act
            var result = controller.Index(model) as ActionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.AreEqual("The Url is invalid.", controller.ModelState[""].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void HtmlBodyAnalysis_Success()
        {
            // Arrange
            var htmlContent = "<html><body>This is a test.</body></html>";
            var model = new AnalysisModel
            {
                WordOccurenceOnPage = true,
                Text = "http://www.valid.url"
            };
            var mock = new Mock<IUrlUtility>();
            mock.Setup(m => m.IsValidUrl(model.Text)).Returns(true);
            mock.Setup(m => m.GetContent(model.Text)).Returns(htmlContent);
            mock.Setup(m => m.TryGetContent(model.Text, out htmlContent)).Returns(true);
            var controller = GetHomeController(mock.Object);

            // Act
            var result = controller.Index(model) as ActionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual("Result", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ResultModel));
            var resultModel = viewResult.Model as ResultModel;
            Assert.AreNotEqual(0, resultModel.WordOccurenceOnPage.Count);
        }

        [TestMethod]
        public void HtmlMetaTagAnalysis_Success()
        {
            // Arrange
            var htmlContent = "<html><meta name=\"description\" content=\"This is a test.\" /><body>This is a test.</body></html>";
            var model = new AnalysisModel
            {
                WordOccurenceOnMeta = true,
                Text = "http://www.valid.url"
            };
            var mock = new Mock<IUrlUtility>();
            mock.Setup(m => m.IsValidUrl(model.Text)).Returns(true);
            mock.Setup(m => m.GetContent(model.Text)).Returns(htmlContent);
            mock.Setup(m => m.TryGetContent(model.Text, out htmlContent)).Returns(true);
            var controller = GetHomeController(mock.Object);

            // Act
            var result = controller.Index(model) as ActionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual("Result", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ResultModel));
            var resultModel = viewResult.Model as ResultModel;
            Assert.AreNotEqual(0, resultModel.WordOccurenceOnMeta.Count);
        }

        [TestMethod]
        public void ExternalLinkAnalysis_Success()
        {
            // Arrange
            var htmlContent = "<html><body><a href=\"https://valid.url\">This is a test.</a></body></html>";
            var model = new AnalysisModel
            {
                ExternalLinkOccurence = true,
                Text = "http://www.valid.url"
            };
            var mock = new Mock<IUrlUtility>();
            mock.Setup(m => m.IsValidUrl(model.Text)).Returns(true);
            mock.Setup(m => m.GetContent(model.Text)).Returns(htmlContent);
            mock.Setup(m => m.TryGetContent(model.Text, out htmlContent)).Returns(true);
            var controller = GetHomeController(mock.Object);

            // Act
            var result = controller.Index(model) as ActionResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual("Result", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ResultModel));
            var resultModel = viewResult.Model as ResultModel;
            Assert.AreNotEqual(0, resultModel.ExternalLinkOccurence.Count);
        }

        private static HomeController GetHomeController(IUrlUtility utility)
        {
            var provider = new StopWordsProvider();
            
            var controller = new HomeController(
                utility,
                provider,
                new TextAnalyser(),
                new HtmlBodyAnalyser(),
                new HtmlMetaAnalyser(),
                new HtmlAnchorAnalyser()
            );

            provider.Path = Path.Combine(Environment.CurrentDirectory, ConfigurationManager.AppSettings["StopWordsFileLocation"]);

            return controller;
        }
    }
}
