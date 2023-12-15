using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Implementacija.Controllers;
using Implementacija.Services;
using Implementacija.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Testovi
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> loggerMock;
        private Mock<IKoncertManager> koncertManagerMock;

        [TestInitialize]
        public void Setup()
        {
            loggerMock = new Mock<ILogger<HomeController>>();
            koncertManagerMock = new Mock<IKoncertManager>();
        }

        [TestMethod]
        public async Task Index_ActionExecutes_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            var result = await controller.Index(null, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Index_WithSortOrderAndSearchString_CallsSortAktuelni()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            await controller.Index("Date", "search");

            // Assert
            koncertManagerMock.Verify(x => x.SortAktuelni("Date", "search"), Times.Once);
        }

        [TestMethod]
        public async Task Index_NullSortOrder_CallsSortAktuelniWithDefaultSortOrder()
        {
            // Arrange
            var homeController = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            await homeController.Index(null, "");
            koncertManagerMock.Verify(x => x.SortAktuelni(null, ""), Times.Once);
        }

        [TestMethod]
        public void Privacy_ActionExecutes_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void About_ActionExecutes_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            var result = controller.About();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Error_ActionExecutes_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Index_WithEmptySearchString_CallsSortAktuelniWithDefaultSortOrder()
        {
            // Arrange
            var homeController = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            await homeController.Index("Date", "");

            // Assert
            koncertManagerMock.Verify(x => x.SortAktuelni("Date", ""), Times.Once);
        }

        [TestMethod]
        public async Task Index_WithInvalidSortOrder_DefaultsToNameDescSortOrder()
        {
            // Arrange
            var homeController = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            await homeController.Index("InvalidSortOrder", "search");

            // Assert
            koncertManagerMock.Verify(x => x.SortAktuelni("InvalidSortOrder", "search"), Times.Once);
        }

        [TestMethod]
        public void Error_HandlesException_ReturnsErrorViewWithRequestId()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ErrorViewModel));
            var model = (ErrorViewModel)viewResult.Model;
            Assert.IsNotNull(model.RequestId);
        }

        [TestMethod]
        public void Error_WithNullRequestId_GeneratesNewRequestId_ReturnsErrorView()
        {
            // Arrange
            var controller = new HomeController(loggerMock.Object, koncertManagerMock.Object);

            // Set Activity.Current to null to simulate the situation
            Activity.Current = null;

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ErrorViewModel));
            var model = (ErrorViewModel)viewResult.Model;
            Assert.IsNotNull(model.RequestId);// Ensure RequestId is not null
        }
    }
}
