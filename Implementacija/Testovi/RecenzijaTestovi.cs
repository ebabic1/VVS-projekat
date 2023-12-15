using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace Testovi
{
    [TestClass]
    public class RecenzijaTestovi
    {
        private ApplicationDbContext _dbContext;
        private Izvodjac izvodjac;
        private Recenzija recenzija;
        private RecenzijaController recenzijaController;

        [TestInitialize]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

            _dbContext = new ApplicationDbContext(options);

            izvodjac = new Izvodjac
            {
                Email = "a@gmail.com",
                UserName = "username",
                Id = "1"
            };
            recenzija = new Recenzija
            {
                Id = 1,
                rating = 5.0,
                komentar = "Odlican nastup.",
                izvodjacId = "1",
                izvodjac = izvodjac
            };
            _dbContext.AddRange(izvodjac, recenzija);
        }

        [TestMethod]
        public async Task IndexAction_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            var result = await recenzijaController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var feedbacks = (List<Recenzija>)viewResult.Model;
            Assert.AreEqual(1, feedbacks.Count);
        }

        [TestMethod]
        public async Task DetailsAction_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - Happy path
            var result = await recenzijaController.Details(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Recenzija;
            StringAssert.Equals("Odlican nastup.", model.komentar);
            // Case2 - id is null
            var result1 = (NotFoundResult)await recenzijaController.Details(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid feedback id
            var result2 = (NotFoundResult)await recenzijaController.Details(2);
            Assert.IsNotNull(result2);
            Assert.AreEqual(404, result2.StatusCode);
        }

        [TestMethod]
        public async Task LeaveReview_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - Happy path
            var result = await recenzijaController.LeaveReview("1") as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Recenzija;
            StringAssert.Equals(5.0, model.rating);
            // Case2 - id is null
            var result1 = (NotFoundResult)await recenzijaController.LeaveReview(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid artist id
            var result2 = (NotFoundResult)await recenzijaController.LeaveReview("2");
            Assert.IsNotNull(result2);
            Assert.AreEqual(404, result2.StatusCode);
        }

        [TestMethod]
        public async Task CreateGet_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            var result = recenzijaController.Create() as ViewResult;
            Assert.IsTrue(result.ViewData.ContainsKey("izvodjacId"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreatePost_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            var feedback = new Recenzija
            {
                Id = 2,
                rating = 3.0,
                komentar = "Nastup je bio onako.",
                izvodjacId = "1",
                izvodjac = izvodjac
            };
            var result = (RedirectToActionResult)await recenzijaController.Create(feedback);
            Assert.IsNotNull(result);
            StringAssert.Equals("Index", result.ActionName);
            // Testing exception message when creating feedback with already existing id
            var result1 = recenzijaController.Create(recenzija);
            Assert.IsNotNull(result1.Exception.Message);
            // Catching exception
            var feedback1 = new Recenzija
            {
                Id = 3,
                rating = 3.0,
                komentar = "Nastup je bio onako.",
                izvodjacId = "1",
                izvodjac = izvodjac
            };
            var result2 = await recenzijaController.Create(feedback1) as ViewResult;
        }

        [TestMethod]
        public async Task CreateLeaveReview_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            var feedback = new Recenzija
            {
                Id = 2,
                rating = 1.0,
                komentar = "Nastup je bio jako loš.",
                izvodjacId = "1",
                izvodjac = izvodjac
            };
            var result = (RedirectToActionResult)await recenzijaController.CreateLeaveReview(feedback);
            Assert.IsNotNull(result);
            StringAssert.Equals("Index", result.ActionName);
        }

        [TestMethod]
        public async Task Edit_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - Happy path
            var result = await recenzijaController.Edit(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Recenzija;
            StringAssert.Equals(5.0, model.rating);
            // Case2 - id is null
            var result1 = (NotFoundResult)await recenzijaController.Edit(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid artist id
            var result2 = (NotFoundResult)await recenzijaController.Edit(2);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public async Task EditPost_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - id is not matching
            recenzija.komentar = "Nastup je ipak bio jako los.";
            recenzija.rating = 1;
            var result = (NotFoundResult)await recenzijaController.Edit(2, recenzija);
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            // Case2 - successfull update
            var result1 = (RedirectToActionResult)await recenzijaController.Edit(1, recenzija);
            Assert.IsNotNull(result1);
            StringAssert.Equals("Index", result1.ActionName);
        }

        [TestMethod]
        public async Task DeleteGet_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - Happy path
            var result = await recenzijaController.Delete(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Recenzija;
            StringAssert.Equals(5.0, model.rating);
            // Case2 - id is null
            var result1 = (NotFoundResult)await recenzijaController.Delete(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid artist id
            var result2 = (NotFoundResult)await recenzijaController.Delete(2);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public async Task DeleteConfirmed_Feedback_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var recenzijaController = new RecenzijaController(_dbContext);
            // Case1 - Happy path
            var result = (RedirectToActionResult)await recenzijaController.DeleteConfirmed(1);
            StringAssert.Equals("Index", result.ActionName);
        }
    }
}
