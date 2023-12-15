using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;

namespace Testovi
{
    [TestClass]
    public class ObicniKorisnikControllerTests
    {
        private ApplicationDbContext _dbContext;
        private ObicniKorisnik korisnik;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            korisnik = new ObicniKorisnik { Id = "3", UserName = "Korisnik3", Email = "user3@example.com" };
            _dbContext.ObicniKorisnici.AddRange(
                new ObicniKorisnik { Id = "1", UserName = "Korisnik1", Email = "user1@example.com" },
                new ObicniKorisnik { Id = "2", UserName = "Korisnik2", Email = "user2@example.com" },
                korisnik
            );
            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task Index_ReturnsViewResultWithListOfKorisnici()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<ObicniKorisnik>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public async Task Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Details("3");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Details("100");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var validModel = new ObicniKorisnik { Id = "4", UserName = "Korisnik4", Email = "user4@example.com" };

            // Act
            var result = await controller.Create(validModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            // Check if the model is added to the database
            var korisnikInDatabase = await _dbContext.ObicniKorisnici.FindAsync("4");
            Assert.IsNotNull(korisnikInDatabase);
            Assert.AreEqual("Korisnik4", korisnikInDatabase.UserName);
            Assert.AreEqual("user4@example.com", korisnikInDatabase.Email);
        }

        [TestMethod]
        public async Task Create_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var invalidModel = new ObicniKorisnik { Id = "4", UserName = "Korisnik4" }; 

            // Act
            controller.ModelState.AddModelError("Email", "Email is required");
            var result = await controller.Create(invalidModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(null, result.ViewName); 
            Assert.IsInstanceOfType(result.Model, typeof(ObicniKorisnik));
            Assert.AreEqual(invalidModel, result.Model);
        }

        [TestMethod]
        public async Task Edit_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Edit("3");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Edit("100");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = await _dbContext.ObicniKorisnici.FindAsync("3");

            // Act
            var result = await controller.Edit("3", obicniKorisnik) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Edit_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = new ObicniKorisnik { Id = "InvalidId", UserName = "UpdatedUserName", Email = "updateduser@example.com" };

            // Act
            var result = await controller.Edit("InvalidId", obicniKorisnik) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndInvalidModel_ReturnsNullViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = await _dbContext.ObicniKorisnici.FindAsync("3");
            obicniKorisnik.UserName = "";
            // Act
            var result = await controller.Edit("3", obicniKorisnik) as ViewResult;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Edit_WithConcurrencyException_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = new ObicniKorisnik { Id = "NonExistingId", UserName = "UpdatedUserName", Email = "updateduser@example.com" };

            // Simulate a concurrency exception
            _dbContext.Entry(obicniKorisnik).State = EntityState.Detached;

            // Act
            var result = await controller.Edit("NonExistingId", obicniKorisnik) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task EditBind_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = await _dbContext.ObicniKorisnici.FindAsync("3");

            // Act
            var result = await controller.Edit("InvalidId", obicniKorisnik) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndValidModelButModelStateIsInvalid_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = await _dbContext.ObicniKorisnici.FindAsync("3");
            controller.ModelState.AddModelError("UserName", "UserName is required");

            // Act
            var result = await controller.Edit("3", obicniKorisnik) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.ViewName);
        }

        [TestMethod]
        public async Task Edit_WithValidIdAndValidModel_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnik = await _dbContext.ObicniKorisnici.FindAsync("3");

            // Act
            var result = await controller.Edit("4", obicniKorisnik);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Delete("3");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Delete("100");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_WithNullId_ReturnsNotFound()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteConfirmed_WithValidId_RemovesObicniKorisnikAndRedirectsToIndex()
        {
            // Arrange
            var controller = new ObicniKorisnikController(_dbContext);
            var obicniKorisnikIdToDelete = "3";

            // Act
            var result = await controller.DeleteConfirmed(obicniKorisnikIdToDelete) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            // Verify that the ObicniKorisnik is removed from the context
            Assert.IsNull(await _dbContext.ObicniKorisnici.FindAsync(obicniKorisnikIdToDelete));
        }

    }
}
