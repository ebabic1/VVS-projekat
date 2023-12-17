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
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Testovi
{
    [TestClass]
    public class RezervacijaDvoraneControllerTests
    {
        private ApplicationDbContext _dbContext;
        private IRezervacijaManager rezervacijaManager;
        private Dvorana dvorana;
        private Izvodjac izvodjac;
        private Rezervacija rezervacija;
        private RezervacijaDvorane rezervacijaDvorane;
        private Iznajmljivac iznajmljivac;
        [TestInitialize]
        public  void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>
            {
                // Kao da je ovaj korisnik ulogovan
                new Claim(ClaimTypes.NameIdentifier, "999")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            mockHttpContext.SetupGet(h => h.User).Returns(principal);
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);
            _dbContext = new ApplicationDbContext(options);
            var porukaManager = new PorukaManager(_dbContext, mockHttpContextAccessor.Object);
            rezervacijaManager=  new RezervacijaManager(_dbContext, porukaManager);
            izvodjac = new Izvodjac
            {
                Email = "a@gmail.com",
                UserName = "username",
                Id = "1"
            };
            iznajmljivac = new Iznajmljivac
            {
                Email = "b@gmail.com",
                UserName = "username",
                Id = "2"
            };
            rezervacija = new Rezervacija
            {
                cijena = 0,
                potvrda = true,
                Id = 1
            };
            dvorana = new Dvorana
            {
                brojSjedista = 2,
                iznajmljivacId = "2",
                adresaDvorane = "test",
                nazivDvorane = "test"
            };
            rezervacijaDvorane = new RezervacijaDvorane
            {
                rezervacijaId = 1,
                izvodjacId = "1",
                dvoranaId = 1
            };
            _dbContext.AddRange(izvodjac, iznajmljivac, rezervacija, dvorana, rezervacijaDvorane);
        }
        [TestMethod]
        public async Task Index_ReturnsViewWithModel()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await controller.Index();
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelReservations = (List<RezervacijaDvorane>)viewResult.Model;
            Assert.AreEqual(1, modelReservations.Count);


        }
        [TestMethod]
        public async Task Details_WithValidId_ReturnsViewWithModel()
        {
            await _dbContext.SaveChangesAsync();
            // Arrange
            var expectedId = 1; // Id used in the setup method
            var _controller = new RezervacijaDvoraneController(_dbContext,rezervacijaManager);
            // Act
            var result = await _controller.Details(expectedId);
            // Assert
            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as RezervacijaDvorane;
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedId, model.Id);
        }
        [TestMethod]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            await _dbContext.SaveChangesAsync();
            // Arrange
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await _controller.Details(null);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Details_NoReservationWithId_ReturnsNotFound()
        {
            await _dbContext.SaveChangesAsync();
            var expectedId = 100;
            // Arrange
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await _controller.Details(expectedId);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Reserve_WithValidId_ReturnsViewWithModel()
        {
            await _dbContext.SaveChangesAsync();
            // Arrange
            var expectedId = 1; // Id used in the setup method
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await _controller.Reserve(expectedId);
            // Assert
            Assert.IsNotNull(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as RezervacijaDvorane;
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedId, model.dvoranaId);
        }
        [TestMethod]
        public async Task Reserve_WithInvalidId_ReturnsNotFound()
        {
            await _dbContext.SaveChangesAsync();
            // Arrange
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await _controller.Reserve(null);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Reserve_NoReservationWithId_ReturnsNotFound()
        {
            await _dbContext.SaveChangesAsync();
            var expectedId = 100;
            // Arrange
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Act
            var result = await _controller.Reserve(expectedId);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task CreateReserve_WithValidModel_RedirectsToIndexAction()
        {
            var novaDvorana = new Dvorana
            {
                Id = 2,
                adresaDvorane = "test",
                brojSjedista = 50,
                iznajmljivacId = "2"
            };
            var noviIzvodjac = new Izvodjac
            {
                Email = "a@gmail.com",
                UserName = "user123",
                Id = "user123"
            };
            
            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            _dbContext.Dvorane.Add(novaDvorana);   
            _dbContext.Izvodjaci.Add(noviIzvodjac);
            await _dbContext.SaveChangesAsync();
            var rezervacijaDvorane = new RezervacijaDvorane
            {
                dvoranaId = novaDvorana.Id,
                dvorana = novaDvorana,
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user123") 
                    }, "mock"))
                }
            };
            // Act
            var result = await _controller.CreateReserve(rezervacijaDvorane) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);

            // Metoda koja se testira uzima trenutno ulogovanog izvodjaca i pravi rezervaciju u njegovo ime
            // Kood u nastavku provjerava da li se to ispravno desilo
            var rezervacijaUser = _dbContext.RezervacijaDvorana.FirstOrDefault(rd => rd.izvodjacId == "user123");
            Assert.AreEqual(rezervacijaUser.izvodjacId, "user123");
        }
        [TestMethod]
        public void Create_ReturnsViewWithSelectLists()
        {
            // Arrange
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData["dvoranaId"]);
            Assert.IsNotNull(result.ViewData["izvodjacId"]);
            Assert.IsNotNull(result.ViewData["rezervacijaId"]);

            // Još nešto provjetiti ako zatreba
        }
        [TestMethod]
        public async Task Create_InValidModelState_RedirectsToCreate()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Update
            controller.ModelState.AddModelError("izvodjacId", "Neki error");
            // Act
            var result = await controller.Create(rezervacijaDvorane) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange

            var _controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);


            var izvodjac = new Izvodjac
            {
                Email = "ab@gmail.com",
                UserName = "username",
                Id = "11"
            };
            var iznajmljivac = new Iznajmljivac
            {
                Email = "b@gmail.com",
                UserName = "username",
                Id = "22"
            };
            var rezervacija = new Rezervacija
            {
                cijena = 0,
                potvrda = true,
                Id = 11
            };
            var dvorana = new Dvorana
            {
                Id = 11,
                brojSjedista = 22,
                iznajmljivacId = "22",
                adresaDvorane = "test",
                nazivDvorane = "test"
            };
            var rezervacijaDvorane = new RezervacijaDvorane
            {
                rezervacijaId = 11,
                izvodjacId = "11",
                dvoranaId = 11
            };
            _dbContext.AddRange(izvodjac, iznajmljivac, rezervacija, dvorana);
            await _dbContext.SaveChangesAsync();
            var result = await _controller.Create(rezervacijaDvorane) as RedirectToActionResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);


            var indexResult = await _controller.Index() as ViewResult;
            Assert.IsNotNull(indexResult);
            var model = indexResult.Model as List<RezervacijaDvorane>;
            Assert.IsNotNull(model);


            var addedReservationId = rezervacijaDvorane.Id;
            Assert.IsTrue(model.Any(r => r.dvoranaId == dvorana.Id));

        }
        [TestMethod]
        public async Task Edit_ValidModelState_RedirectsToIndex()
        { 
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            _dbContext.Dvorane.Add(dvorana);
            _dbContext.Izvodjaci.Add(izvodjac);
            _dbContext.RezervacijaDvorana.Add(rezervacijaDvorane);
            await _dbContext.SaveChangesAsync();

            // Update
            rezervacijaDvorane.izvodjacId = "8"; 

            // Act
            var result = await controller.Edit(rezervacijaDvorane.Id, rezervacijaDvorane) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updatedReservation = _dbContext.RezervacijaDvorana.FirstOrDefault(r => r.Id == rezervacijaDvorane.Id);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual(rezervacijaDvorane.izvodjacId, updatedReservation.izvodjacId);
        }
        [TestMethod]
        public async Task Edit_InValidModelState_RedirectsToIndex()
        {
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            _dbContext.Dvorane.Add(dvorana);
            _dbContext.Izvodjaci.Add(izvodjac);
            _dbContext.RezervacijaDvorana.Add(rezervacijaDvorane);
            await _dbContext.SaveChangesAsync();

            // Update
            rezervacijaDvorane.izvodjacId = "8";
            controller.ModelState.AddModelError("izvodjacId", "Neki error");
            // Act
            var result = await controller.Edit(rezervacijaDvorane.Id, rezervacijaDvorane);

            // Assert
            Assert.IsNotNull(result);

            var updatedReservation = _dbContext.RezervacijaDvorana.FirstOrDefault(r => r.Id == rezervacijaDvorane.Id);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual(rezervacijaDvorane.izvodjacId, updatedReservation.izvodjacId);
        }

       
        [TestMethod]
        public async Task Edit_InValidId_ReturnsNotfound()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Update
            rezervacijaDvorane.izvodjacId = "8";
            // Act
            var result = await controller.Edit(rezervacijaDvorane.Id+2, rezervacijaDvorane);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task Delete_WithValidId_ReturnsViewWithModel()
        {
            await _dbContext.SaveChangesAsync();

            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            var nullResult = await controller.Delete(null) as NotFoundResult;
            Assert.IsNotNull(nullResult);
            Assert.IsNotNull(nullResult);
            var invalidResult = await controller.Delete(999) as NotFoundResult;
            Assert.IsNotNull(invalidResult);
            var result = await controller.Delete(rezervacijaDvorane.Id) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as RezervacijaDvorane;
            Assert.IsNotNull(model);
            Assert.AreEqual(rezervacijaDvorane.Id, model.Id);
            Assert.IsNotNull(model.dvorana);
            Assert.IsNotNull(model.izvodjac);
            Assert.IsNotNull(model.rezervacija);
        }

        [TestMethod]
        public async Task Edit_WithNullId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);

            // Act
            var result = await controller.Edit(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task DeleteConfirmed_WithValidId_DeletesReservationAndRedirectsToIndex()
        {
            // Arrange
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            await _dbContext.SaveChangesAsync();
           
            // Act
            var result = await controller.DeleteConfirmed(rezervacija.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var deletedReservation = _dbContext.RezervacijaDvorana.FirstOrDefault(r => r.Id == rezervacijaDvorane.Id);
            Assert.IsNull(deletedReservation);
        }
        [TestMethod]
        public async Task DeleteConfirmed_WithExistingConcert_DeletesConcert()
        {
            // Arrange
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            
            var koncert = new Koncert
            {
                datum = new DateTime(2022, 10, 12),
                Id = 12,
                izvodjac = izvodjac,
                izvodjacId = izvodjac.Id,
                naziv = "Neki koncert",
                zanr = Zanr.HIPHOP
            };
            _dbContext.Add(koncert);
            await _dbContext.SaveChangesAsync();
            var result = await controller.DeleteConfirmed(rezervacija.Id) as NotFoundResult;
        }
        [TestMethod]
        public async Task Edit_WithDifferentIds_ReturnsCorrectResult()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            // Null Id provided
            var nullResult = await controller.Edit(null) as NotFoundResult;
            Assert.IsNotNull(nullResult);
            // Invalid Id provided
            var invalidResult = await controller.Edit(999) as NotFoundResult;
            Assert.IsNotNull(invalidResult);
            // Valid Id provided
            var validResult = await controller.Edit(rezervacijaDvorane.Id) as ViewResult;
            Assert.IsNotNull(validResult);
            var model = validResult.Model as RezervacijaDvorane;
            Assert.IsNotNull(model);
            Assert.AreEqual(rezervacijaDvorane.Id, model.Id);
        }
        [TestMethod]
        public async Task RezervacijaExists_WithValidId_ReturnsTrue()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);

            var methodInfo = typeof(RezervacijaDvoraneController).GetMethod("RezervacijaDvoraneExists", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo, "Method not found");

            var result = methodInfo.Invoke(controller, new object[] { 1 }) as bool?;

            // Assert the result or check behavior
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Edit_ValidModelState_RedirectsToEdit()
        {
            // Arrange
            var novaDvorana = new Dvorana
            {
                Id = 4,
                adresaDvorane = "test",
                brojSjedista = 500,
                iznajmljivacId = "3"
            };
            var noviIzvodjac = new Izvodjac
            {
                Email = "aizvo2@etf.unsa.ba",
                UserName = "izvo",
                Id = "7"
            };
            var novaRezervacija = new RezervacijaDvorane
            {
                Id = 123,
                dvoranaId = novaDvorana.Id,
                dvorana = novaDvorana,
                izvodjacId = noviIzvodjac.Id,
                izvodjac = noviIzvodjac
            };

            var controller = new RezervacijaDvoraneController(_dbContext, rezervacijaManager);
            _dbContext.Dvorane.Add(novaDvorana);
            _dbContext.Izvodjaci.Add(noviIzvodjac);
            _dbContext.RezervacijaDvorana.Add(novaRezervacija);
            await _dbContext.SaveChangesAsync();

            // Update
            novaRezervacija.izvodjacId = "8";

            // Act
            var result = await controller.Edit(novaRezervacija.Id, novaRezervacija) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updatedReservation = _dbContext.RezervacijaDvorana.FirstOrDefault(r => r.Id == novaRezervacija.Id);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual(novaRezervacija.izvodjacId, updatedReservation.izvodjacId);
        }
    }

}

