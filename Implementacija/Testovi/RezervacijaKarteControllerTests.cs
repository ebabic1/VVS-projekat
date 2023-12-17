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
    public class RezervacijaKarteControllerTests
    {
        private ApplicationDbContext _context;
        private IRezervacijaManager rezervacijaManager;
        private Izvodjac izvodjac;
        private Koncert koncert;
        private RezervacijaKarte rezervacijaKarte;
        private RezervacijaKarte rezervacijaKarteForCreate;
        private Rezervacija rezervacija;
        private ObicniKorisnik obicniKorisnik;
        private Iznajmljivac iznajmljivac;
        private Dvorana dvorana;
        private RezervacijaDvorane rezervacijaDvorane;
        private KartaManager kartaManager;
        [TestInitialize]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeUserId = "abcd";
            context.Request.Headers["User-ID"] = fakeUserId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            _context = new ApplicationDbContext(options);
            var porukaManager = new PorukaManager(_context, mockHttpContextAccessor.Object);
            rezervacijaManager = new RezervacijaManager(_context, porukaManager);
            kartaManager = new KartaManager(_context);
            izvodjac = new Izvodjac
            {
                Id = "12345",
                UserName = "NoviIzvodjac",
                Email = "noviizvodjac@example.com"
            };
            koncert = new Koncert
            {
                Id = 1,
                naziv = "noviKoncert",
                zanr = Zanr.HIPHOP,
                datum = DateTime.Now,
                izvodjacId = "12345"
            };
            rezervacija = new Rezervacija
            {
                cijena = 0,
                potvrda = true,
                Id = 1
            };
            obicniKorisnik = new ObicniKorisnik
            {
                Id = "23456",
                UserName = "NoviObicniKorisnik",
                Email = "noviobicnikorisnik@example.com"
            };
            rezervacijaKarte = new RezervacijaKarte
            {
                Id = 1,
                rezervacijaId = 1,
                rezervacija = rezervacija,
                obicniKorisnikId = "23456",
                obicniKorisnik = obicniKorisnik,
                koncertId = 1,
                tipMjesta = TipMjesta.PARTER
            };
            rezervacijaKarteForCreate = new RezervacijaKarte
            {
                Id = 2,
                rezervacijaId = 1,
                obicniKorisnikId = "23456",
                koncertId = 1,
                tipMjesta = TipMjesta.PARTER
            };
            iznajmljivac = new Iznajmljivac
            {
                Id = "34567",
                UserName = "NoviIznajmljivac",
                Email = "noviiznajmljivac@example.com"
            };
            dvorana = new Dvorana
            {
                Id = 1,
                nazivDvorane = "Naziv nove dvorane",
                adresaDvorane = "Adresa nove dvorane",
                brojSjedista = 100, 
                iznajmljivacId = "34567" 
            };
            rezervacijaDvorane = new RezervacijaDvorane
            {
                Id = 1,
                rezervacijaId = 1,
                izvodjacId = "12345",
                dvoranaId = 1
            };
            _context.AddRange(izvodjac, koncert, rezervacija, obicniKorisnik, rezervacijaKarte, iznajmljivac, dvorana, rezervacijaDvorane);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithModel()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelReservations = (List<RezervacijaKarte>)viewResult.Model;
            Assert.AreEqual(1, modelReservations.Count);
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            // Act
            var result = await controller.Details(null);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Details(5); // Ne postoji rezervacija karte sa id-em 5

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WhenIdExists()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Details(rezervacijaKarte.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(RezervacijaKarte));
            Assert.AreEqual(rezervacijaKarte.Id, ((RezervacijaKarte)viewResult.Model).Id);
        }

        [TestMethod]
        public async Task Reserve_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Reserve(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Reserve_ReturnsNotFound_WhenKoncertDoesNotExist()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Reserve(5); // Ne postoji koncert sa id-em 5

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Reserve_ReturnsViewResult_WhenIdExists()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Reserve(koncert.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(RezervacijaKarte));
            Assert.AreEqual(koncert.Id, ((RezervacijaKarte)viewResult.Model).koncert.Id);
        }
        [TestMethod]
        public async Task Create_ReturnsViewResult()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Create(rezervacijaKarteForCreate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            controller.ModelState.AddModelError("FieldName", "Error Message");

            // Act
            var result = await controller.Create(rezervacijaKarteForCreate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task CreateReserve_ReturnsRedirectToActionResult_WhenReservationIsSuccessful()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                        new Claim(ClaimTypes.NameIdentifier, "12345")
        }, "mock"))
                }
            };

            // Act
            var result = await controller.CreateReserve(rezervacijaKarteForCreate) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            var rezervacijaUser = _context.RezervacijaDvorana.FirstOrDefault(rd => rd.izvodjacId == "12345");
            Assert.AreEqual(rezervacijaUser.izvodjacId, "12345");
        }
        [TestMethod]
        public async Task CreateReserve_ReturnsRedirectToActionResult_WhenNoRemainingSeats()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            dvorana.brojSjedista = 0;

            // Act
            var result = await controller.CreateReserve(rezervacijaKarteForCreate);
            var redirectToActionResult = (RedirectToActionResult)result;

            // Assert
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            Assert.AreEqual("Home", redirectToActionResult.ControllerName);
        }
        [TestMethod]
        public async Task CreateReserve_ReturnsRedirectToActionResult_WhenModelStateIsInvalid()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            controller.ModelState.AddModelError("FieldName", "Error Message");

            // Act
            var result = await controller.CreateReserve(rezervacijaKarteForCreate);

            // Assert
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            Assert.AreEqual("Home", redirectToActionResult.ControllerName);
        }

        [TestMethod]
        public async Task Edit_ReturnsViewWithModel_WhenIdIsNotNull()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Edit(rezervacijaKarte.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var model = (RezervacijaKarte)viewResult.Model;
            Assert.AreEqual(rezervacijaKarte.Id, model.Id);
        }

        [TestMethod]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_ReturnsNotFound_WhenReservationNotFound()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Edit(1111);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task EditPost_ReturnsRedirectToIndex_WhenSuccessful()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Edit(rezervacijaKarte.Id, rezervacijaKarte);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task EditPost_ReturnsNotFound_WhenInvalidId()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Edit(1111, rezervacijaKarte);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        
        [TestMethod]
        public async Task EditPost_ReturnsViewWithModel_WhenInvalidModel()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            controller.ModelState.AddModelError("field", "Error message");

            // Act
            var result = await controller.Edit(rezervacijaKarte.Id, rezervacijaKarte);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            Assert.AreEqual(rezervacijaKarte, viewResult.Model);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenReservationNotFound()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Delete(1111); // Nema rezervacije s ID-em 1111

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsViewResult_WhenReservationExists()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.Delete(rezervacijaKarte.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(RezervacijaKarte));
            Assert.AreEqual(rezervacijaKarte.Id, ((RezervacijaKarte)viewResult.Model).Id);
        }

        [TestMethod]
        public async Task DeleteConfirmed_ReturnsRedirectToIndex_WhenReservationExists()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);

            // Act
            var result = await controller.DeleteConfirmed(rezervacijaKarte.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
        [TestMethod]
        public async Task RezervacijaExists_WithValidId_ReturnsTrue()
        {
            // Testiranje privatne metode

            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RezervacijaKarteController(_context, rezervacijaManager);
            var methodInfo = typeof(RezervacijaKarteController).GetMethod("RezervacijaKarteExists", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Assert
            Assert.IsNotNull(methodInfo, "Method not found");

            // Act
            var result = methodInfo.Invoke(controller, new object[] { 1 }) as bool?;

            // Assert
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public async Task KartaManager_GetAll_ReturnsAllReservations()
        {
            // Arrange
            await _context.SaveChangesAsync();

            // Act
            var result = await kartaManager.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<RezervacijaKarte>));
            CollectionAssert.Contains(result.ToList(), rezervacijaKarte);
        }
        [TestMethod]
        public async Task KartaManager_GetOwned_ReturnsOwnedReservations()
        {
            // Arrange
            await _context.SaveChangesAsync();

            // Act
            var result = kartaManager.GetOwned("23456");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<RezervacijaKarte>));
            Assert.AreEqual(1, result.Count()); 
        }
        [TestMethod]
        public async Task KartaManager_GetName_ReturnsKoncertName()
        {
            // Arrange
            await _context.SaveChangesAsync();

            // Act
            var result = kartaManager.GetName(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("noviKoncert", result);
        }
        [TestMethod]
        public async Task KartaManager_GetGuy_ReturnsIzvodjacId()
        {
            // Arrange
            await _context.SaveChangesAsync();

            // Act
            var result = kartaManager.GetGuy(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
        [TestCleanup]
        public void Cleanup()
        {
            var izvodjacToDelete = _context.Izvodjaci.FirstOrDefault(i => i.Id == "12345");
            if (izvodjacToDelete != null)
            {
                _context.Izvodjaci.Remove(izvodjacToDelete);
            }
            var koncertToDelete = _context.Koncerti.FirstOrDefault(k => k.Id == 1);
            if (koncertToDelete != null)
            {
                _context.Koncerti.Remove(koncertToDelete);
            }
            var rezervacijaToDelete = _context.Rezervacija.FirstOrDefault(k => k.Id == 1);
            if (rezervacijaToDelete != null)
            {
                _context.Rezervacija.Remove(rezervacijaToDelete);
            }
            var obicniKorisnikToDelete = _context.ObicniKorisnici.FirstOrDefault(k => k.Id == "23456");
            if (obicniKorisnikToDelete != null)
            {
                _context.ObicniKorisnici.Remove(obicniKorisnikToDelete);
            }
            var rezervacijaKarteToDelete = _context.RezervacijaKarata.FirstOrDefault(k => k.Id == 1);
            if (rezervacijaKarteToDelete != null)
            {
                _context.RezervacijaKarata.Remove(rezervacijaKarteToDelete);
            }
            var rezervacijaKarteForCreateToDelete = _context.RezervacijaKarata.FirstOrDefault(k => k.Id == 2);
            if (rezervacijaKarteForCreateToDelete != null)
            {
                _context.RezervacijaKarata.Remove(rezervacijaKarteForCreateToDelete);
            }
            var iznajmljivacToDelete = _context.Iznajmljivaci.FirstOrDefault(k => k.Id == "34567");
            if (iznajmljivacToDelete != null)
            {
                _context.Iznajmljivaci.Remove(iznajmljivacToDelete);
            }
            var dvoranaToDelete = _context.Dvorane.FirstOrDefault(k => k.Id == 1);
            if (dvoranaToDelete != null)
            {
                _context.Dvorane.Remove(dvoranaToDelete);
            }
            var rezervacijaDvoraneToDelete = _context.RezervacijaDvorana.FirstOrDefault(k => k.Id == 1);
            if (rezervacijaDvoraneToDelete != null)
            {
                _context.RezervacijaDvorana.Remove(rezervacijaDvoraneToDelete);
            }

            _context.SaveChanges();
        }
    }
}
