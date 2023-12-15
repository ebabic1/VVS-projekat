using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testovi
{
    [TestClass]
    public class RegistracijaKoncertaControllerTests
    {
        private ApplicationDbContext _context;
        private Izvodjac izvodjac;
        private Koncert koncert;
        private RegistracijaKoncerta registracijaKoncerta;

        [TestInitialize]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
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
            registracijaKoncerta = new RegistracijaKoncerta
            {
                Id = 1,
                naziv = "noviKoncert",
                zanr = Zanr.HIPHOP,
                datum = DateTime.Now,
                izvodjacId = "12345",
                izvodjac = izvodjac
            };
            _context.Add(izvodjac);
        }
        public static IEnumerable<object[]> ReadJson()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\RegistracijaKoncertaTestData.json");
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var koncerti = jobject["koncert"]?.ToObject<IEnumerable<Koncert>>();

            foreach (var koncert in koncerti ?? Enumerable.Empty<Koncert>())
            {
                yield return new[] { koncert };
            }
        }
        static IEnumerable<object[]> KoncertiJSON
        {
            get
            {
                return ReadJson();
            }
        }
        public static IEnumerable<object[]> ReadXML()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\RegistracijaKoncertaTestData.xml");
            var xml = XDocument.Load(filePath);

            var koncerti = xml.Descendants("Koncert")
                .Select(koncert => new object[]
                {
            int.Parse(koncert.Element("Id")?.Value),
            koncert.Element("Naziv")?.Value,
            koncert.Element("IzvodjacID")?.Value
                });

            return koncerti;
        }
        static IEnumerable<object[]> KoncertiXML
        {
            get
            {
                return ReadXML();
            }
        }
        [TestMethod]
        [DynamicData("KoncertiJSON")]
        public async Task Register_ValidKoncert_RedirectsToConfirmation_JSON(Koncert koncert)
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);

            // Act
            var result = await controller.Register(koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            var check = _context.Koncerti.Any(x => x.Id == koncert.Id);
            Assert.IsTrue(check);
            Assert.AreEqual("Confirmation", result.ActionName);
        }
        [TestMethod]
        [DynamicData("KoncertiXML")]
        public async Task Register_ValidKoncert_RedirectsToConfirmation_XML(int Id, string Naziv, string IzvodjacID)
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);
            var koncert = new Koncert { Id = Id, naziv = Naziv, izvodjacId = IzvodjacID };

            // Act
            var result = await controller.Register(koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            var check = _context.Koncerti.Any(x => x.Id == koncert.Id);
            Assert.IsTrue(check);
            Assert.AreEqual("Confirmation", result.ActionName);
        }
        [TestMethod]
        public async Task Register_ValidKoncert_RedirectsToConfirmation()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);

            // Act
            var result = await controller.Register(koncert) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Confirmation", result.ActionName);
        }

        [TestMethod]
        public async Task Register_ValidKoncert_SavesToDatabase()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);

            // Act
            await controller.Register(koncert);

            // Assert
            var savedKoncert = _context.Koncerti.FirstOrDefault(k => k.Id == 1);
            Assert.IsNotNull(savedKoncert);
            Assert.AreEqual(Zanr.HIPHOP, savedKoncert.zanr);
        }

        [TestMethod]
        public async Task Confirmation_ReturnsView()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);

            // Act
            var result = controller.Confirmation() as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task Register_ReturnsView()
        {
            // Arrange
            await _context.SaveChangesAsync();
            var controller = new RegistracijaKoncertaController(_context);

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        [TestCleanup]
        public void Cleanup()
        {
            var izvodjacToDelete = _context.Izvodjaci.FirstOrDefault(i => i.Id == "12345");
            if (izvodjacToDelete != null)
            {
                _context.Izvodjaci.Remove(izvodjacToDelete);
            }
            var koncertToDelete = _context.Koncerti.FirstOrDefault(i => i.Id == 1);
            if (koncertToDelete != null)
            {
                _context.Koncerti.Remove(koncertToDelete);
            }

            _context.SaveChanges();
        }
    }
}
