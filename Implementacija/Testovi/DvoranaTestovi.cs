using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Testovi
{
    [TestClass]
    public class DvoranaTestovi
    {
        private ApplicationDbContext _dbContext;
        private IDvoranaManager dvoranaManager;
        private Izvodjac izvodjac;
        private Iznajmljivac iznajmljivac;
        private Dvorana dvorana, dvorana1;
        private Rezervacija rezervacija;
        private RezervacijaDvorane rezervacijaDvorane;
        private DvoranaController dvoranaController;

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
            iznajmljivac = new Iznajmljivac
            {
                Email = "b@gmail.com",
                UserName = "iznajmljivac",
                Id = "2"
            };
            dvorana = new Dvorana
            {
                Id = 1,
                nazivDvorane = "Koncretna dvorana 1",
                adresaDvorane = "Antuna Branka Šimića 1",
                brojSjedista = 1,
                iznajmljivacId = "2",
                iznajmljivac = iznajmljivac
            };
            dvorana1 = new Dvorana
            {
                Id = 5,
                nazivDvorane = "Koncretna dvorana 5",
                adresaDvorane = "Antuna Branka Šimića 5",
                brojSjedista = 5,
                iznajmljivacId = "2",
                iznajmljivac = iznajmljivac
            };
            rezervacija = new Rezervacija
            {
                Id = 1,
                potvrda = true,
                cijena = 1500
            };
            rezervacijaDvorane = new RezervacijaDvorane
            {
                Id = 1,
                rezervacijaId = 1,
                rezervacija = rezervacija,
                dvoranaId = 1,
                dvorana = dvorana,
                izvodjac = izvodjac,
                izvodjacId = "1"
            };
            _dbContext.AddRange(izvodjac, iznajmljivac, dvorana, dvorana1, rezervacija, rezervacijaDvorane);
        }

        [TestMethod]
        public async Task IndexAction_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var result = await dvoranaController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var halls = (List<Dvorana>)viewResult.Model;
            Assert.AreEqual(2, halls.Count);
        }

        [TestMethod]
        public async Task GetAll_Hall_ReturnsListOfHalls()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            dvoranaManager = new DvoranaManager(_dbContext, mockHttpContextAccessor.Object);
            List<Dvorana> list = (List<Dvorana>) await dvoranaManager.GetAll();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public async Task GetUnreserved_Hall_ReturnsListOfHalls()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            dvoranaManager = new DvoranaManager(_dbContext, mockHttpContextAccessor.Object);
            List<Dvorana> list = (List<Dvorana>)await dvoranaManager.GetUnreserved();
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task GetReservedByCurrentPerformer_Hall_ReturnsListOfHalls()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                }))
            };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);
            dvoranaManager = new DvoranaManager(_dbContext, mockHttpContextAccessor.Object);
            List<Dvorana> list = (List<Dvorana>)await dvoranaManager.GetReservedByCurrentPerformer();
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task GetOwnedByCurrentRenter_Hall_ReturnsListOfHalls()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                }))
            };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);
            dvoranaManager = new DvoranaManager(_dbContext, mockHttpContextAccessor.Object);
            List<Dvorana> list = (List<Dvorana>)await dvoranaManager.GetOwnedByCurrentRenter();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public async Task GetReservations_Hall_ReturnsListOfHalls()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                }))
            };
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);
            dvoranaManager = new DvoranaManager(_dbContext, mockHttpContextAccessor.Object);
            List<RezervacijaDvorane> list = (List<RezervacijaDvorane>)await dvoranaManager.GetReservations();
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task DetailsAction_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            // Case1 - Happy path
            var result = await dvoranaController.Details(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Dvorana;
            StringAssert.Equals("Antuna Branka Šimića 1", model.adresaDvorane);
            // Case2 - id is null
            var result1 = (NotFoundResult)await dvoranaController.Details(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid hall id
            var result2 = (NotFoundResult)await dvoranaController.Details(2);
            Assert.IsNotNull(result2);
            Assert.AreEqual(404, result2.StatusCode);
        }

        [TestMethod]
        public async Task CreateGet_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var result = dvoranaController.Create() as ViewResult;
            Assert.IsTrue(result.ViewData.ContainsKey("iznajmljivacId"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreatePost_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            var hall = new Dvorana
            {
                Id = 2,
                nazivDvorane = "Koncretna dvorana 2",
                adresaDvorane = "Azize Šaćirbegović BB",
                brojSjedista = 15,
                iznajmljivacId = "1"
            };
            var result = (RedirectToActionResult)await dvoranaController.Create(hall);
            Assert.IsNotNull(result);
            StringAssert.Equals("Index", result.ActionName);
            // Testing exception message when creating hall with already existing id
            var result1 = dvoranaController.Create(dvorana);
            Assert.IsNotNull(result1.Exception.Message);
            // Catching exception
            var hall1 = new Dvorana
            {
                Id = 3,
                nazivDvorane = "Koncretna dvorana 3",
                adresaDvorane = "Azize Šaćirbegović 10",
                brojSjedista = 10,
                iznajmljivacId = "1"
            };
            var result2 = await dvoranaController.Create(hall1) as ViewResult;
        }

        [TestMethod]
        public async Task Edit_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            // Case1 - Happy path
            var result = await dvoranaController.Edit(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Dvorana;
            StringAssert.Equals("Koncretna dvorana 1", model.nazivDvorane);
            // Case2 - id is null
            var result1 = (NotFoundResult)await dvoranaController.Edit(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid hall id
            var result2 = (NotFoundResult)await dvoranaController.Edit(2);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public async Task EditPost_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            // Case1 - id is not matching
            dvorana.adresaDvorane = "Milana Preloga 1";
            dvorana.brojSjedista = 20;
            var result = (NotFoundResult)await dvoranaController.Edit(2, dvorana);
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            // Case2 - successfull update
            var result1 = (RedirectToActionResult)await dvoranaController.Edit(1, dvorana);
            Assert.IsNotNull(result1);
            StringAssert.Equals("Index", result1.ActionName);
        }

        [TestMethod]
        public async Task DeleteGet_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            // Case1 - Happy path
            var result = await dvoranaController.Delete(1) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Dvorana;
            StringAssert.Equals("Antuna Branka Šimića 1", model.adresaDvorane);
            // Case2 - id is null
            var result1 = (NotFoundResult)await dvoranaController.Delete(null);
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
            // Case3 - invalid hall id
            var result2 = (NotFoundResult)await dvoranaController.Delete(2);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public async Task DeleteConfirmed_Hall_ReturnsCorrectView()
        {
            await _dbContext.SaveChangesAsync();
            var dvoranaController = new DvoranaController(_dbContext);
            // Case1 - Happy path
            var result = (RedirectToActionResult)await dvoranaController.DeleteConfirmed(1);
            StringAssert.Equals("Index", result.ActionName);
        }
    }
}
