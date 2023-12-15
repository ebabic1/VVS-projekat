using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Testovi
{
    [TestClass]
    public class KoncertControllerTests
    {
        private ApplicationDbContext _dbContext;
        private Koncert koncert;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            koncert = new Koncert { Id = 2, naziv = "Koncert2", datum = new DateTime(2023, 12, 1, 10, 30, 0), izvodjacId = "2", zanr = Zanr.ROCK };
            _dbContext.Izvodjaci.AddRange(
                new Izvodjac { Id = "1", UserName = "Izvodjac1", Email = "user1@example.com" },
                new Izvodjac { Id = "2", UserName = "Izvodjac2", Email = "user2@example.com" }
            );
            _dbContext.Koncerti.AddRange(
                new Koncert { Id = 1, naziv = "Koncert1", datum = new DateTime(2022, 12, 1, 10, 30, 0), izvodjacId = "1", zanr = Zanr.ROCK },
                koncert
            );
            _dbContext.Rezervacija.AddRange(
                new Rezervacija { Id = 1 }
            );
            _dbContext.RezervacijaDvorana.AddRange(
                new RezervacijaDvorane { Id = 1, rezervacijaId = 1, izvodjacId = "1" }
            );
            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task Index_ReturnsViewResultWithListOfKoncerti()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Koncert>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = new Koncert { Id = 3, naziv = "Sample Koncert", izvodjacId = "1" };

            // Act
            var result = await controller.Create(koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName); // Assuming you don't set the ViewName explicitly

            // Additional assertion to check ViewData
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData["izvodjacId"]);

            // You may also want to check the type of the ViewData
            Assert.IsInstanceOfType(result.ViewData["izvodjacId"], typeof(SelectList));
        }

        [TestMethod]
        public async Task Create_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var invalidKoncert = new Koncert(); 

            controller.ModelState.AddModelError("SomeProperty", "Invalid model state error");

            // Act
            var result = await controller.Create(invalidKoncert) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName); 
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData["izvodjacId"]);
            Assert.IsInstanceOfType(result.ViewData["izvodjacId"], typeof(SelectList));
            Assert.AreSame(invalidKoncert, result.Model as Koncert);
        }

        [TestMethod]
        public async Task CreateKoncert_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var izvodjacId = "2";
            var izvodjac = await _dbContext.Izvodjaci.FindAsync(izvodjacId);

            // Act
            var result = await controller.CreateKoncert(izvodjac.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewName);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Koncert));

            var koncert = result.Model as Koncert;

            // Additional assertions based on your specific logic
            Assert.AreEqual(izvodjac, koncert.izvodjac);
            Assert.AreEqual(izvodjacId, koncert.izvodjacId);
        }

        [TestMethod]
        public async Task CreateKoncert_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var izvodjacId = "16";

            // Act
            var result = await controller.CreateKoncert(izvodjacId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateKoncert_WithNullId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            string izvodjacId = null;

            // Act
            var result = await controller.CreateKoncert(izvodjacId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateKoncert_PostAction_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = new Koncert { Id = 3, naziv = "Test Koncert", izvodjacId = "2" };

            // Act
            var result = await controller.CreateKoncert(koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [TestMethod]
        public async Task Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Details(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Details(100);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Edit(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Edit(100);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = await _dbContext.Koncerti.FindAsync(1);

            // Act
            var result = await controller.Edit(1, koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Edit_WithDifferentIdAndValidModel_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = await _dbContext.Koncerti.FindAsync(1);

            // Act
            var result = await controller.Edit(2, koncert);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = await _dbContext.Koncerti.FindAsync(1);

            controller.ModelState.AddModelError("SomeProperty", "Invalid model state error");

            // Act
            var result = await controller.Edit(1, koncert);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Edit_WithConcurrencyExceptionAndNoExistingKoncert_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncert = new Koncert { Id = 3, naziv = "Koncert3", izvodjacId = "1" };

            // Simulate a concurrency exception
            _dbContext.Entry(koncert).State = EntityState.Detached;

            // Act
            var result = await controller.Edit(3, koncert);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Delete(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Delete(100);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteConfirmed_WithValidId_RemovesKoncertAndRedirectsToIndex()
        {
            // Arrange
            var controller = new KoncertController(_dbContext);
            var koncertIdToDelete = 1;

            // Act
            var result = await controller.DeleteConfirmed(koncertIdToDelete) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            // Verify that the Koncert is removed from the context
            Assert.IsNull(await _dbContext.Koncerti.FindAsync(koncertIdToDelete));
        }

        [TestMethod]
        public async Task SortAktuelni_WithName_DescSortOrderAndSearchString_ReturnsSortedKoncertiList()
        {
            // Arrange
            var koncertManager = new KoncertManager(_dbContext);

            // Act
            var result = await koncertManager.SortAktuelni("name_desc", "Koncert") as List<Koncert>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Koncert2", result[0].naziv);
            Assert.AreEqual("Koncert1", result[1].naziv);
        }

        [TestMethod]
        public async Task SortAktuelni_WithDate_AscSortOrderAndSearchString_ReturnsSortedKoncertiList()
        {
            // Arrange
            var koncertManager = new KoncertManager(_dbContext);

            // Act
            var result = await koncertManager.SortAktuelni("Date", "Koncert") as List<Koncert>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Koncert1", result[0].naziv);
            Assert.AreEqual("Koncert2", result[1].naziv);
        }

        [TestMethod]
        public async Task SortAktuelni_WithDate_DescSortOrderAndSearchString_ReturnsSortedKoncertiList()
        {
            // Arrange
            var koncertManager = new KoncertManager(_dbContext);

            // Act
            var result = await koncertManager.SortAktuelni("date_desc", "Koncert") as List<Koncert>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Koncert2", result[0].naziv);
            Assert.AreEqual("Koncert1", result[1].naziv);
        }

        [TestMethod]
        public async Task SortAktuelni_WithDefaultSortOrderAndSearchString_ReturnsSortedKoncertiList()
        {
            // Arrange
            var koncertManager = new KoncertManager(_dbContext);

            // Act
            var result = await koncertManager.SortAktuelni("default", "Koncert") as List<Koncert>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Koncert1", result[0].naziv);
            Assert.AreEqual("Koncert2", result[1].naziv);
        }

        [TestMethod]
        public void GetRecommended_ReturnsRecommendedKoncerti()
        {
            // Arrange
            var koncertManager = new KoncertManager(_dbContext);

            // Act
            var result = koncertManager.GetRecommended() as List<Koncert>;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
