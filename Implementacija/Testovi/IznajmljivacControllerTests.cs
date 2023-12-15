using Implementacija.Controllers;
using Implementacija.Data;
using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Linq;

namespace Testovi
{
    [TestClass]
    public class IznajmljivacControllerTests
    {
        private ApplicationDbContext _dbContext;
        private Iznajmljivac iznajmljivac;

        public static IEnumerable<object[]> ReadJson()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestData.json");
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var users = jobject["user"]?.ToObject<IEnumerable<Iznajmljivac>>();

            foreach (var user in users ?? Enumerable.Empty<Iznajmljivac>())
            {
                yield return new[] { user };
            }
        }
        static IEnumerable<object[]> IznajmljivaciJSON
        {
            get
            {
                return ReadJson();
            }
        }
        public static IEnumerable<object[]> ReadXML()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestData.xml");
            var xml = XDocument.Load(filePath);

            var izvodjaci = xml.Descendants("Izvodjac")
                .Select(izvodjac => new object[]
                {
            izvodjac.Element("Id")?.Value,
            izvodjac.Element("UserName")?.Value,
            izvodjac.Element("Email")?.Value
                });

            return izvodjaci;
        }
        static IEnumerable<object[]> IznajmljivaciXML
        {
            get
            {
                return ReadXML();
            }
        }
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            iznajmljivac = new Iznajmljivac { Id = "3", UserName = "Iznajmljivac3", Email = "user3@example.com" };
            _dbContext.Iznajmljivaci.AddRange(
                new Iznajmljivac { Id = "1", UserName = "Iznajmljivac1", Email = "user1@example.com" },
                new Iznajmljivac { Id = "2", UserName = "Iznajmljivac2", Email = "user2@example.com" },
                iznajmljivac
            );
            _dbContext.SaveChanges();
        }
        
        [TestMethod]
        public async Task Index_ReturnsViewWithListOfIznajmljivaci()
        {
            // Arrange
            var controller = new IznajmljivacController(_dbContext);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Iznajmljivac>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public async Task Details_WithValidId_ReturnsView()
        {
            var controller = new IznajmljivacController(_dbContext);
            var result = await controller.Details("1") as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Iznajmljivac;
            Assert.AreEqual(model.UserName, "Iznajmljivac1");
        }

        [TestMethod]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            var controller = new IznajmljivacController(_dbContext);

            var result1 = await controller.Details("5") as NotFoundResult;
            var result2 = await controller.Details(null) as NotFoundResult;    

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(404, result1.StatusCode);
            Assert.AreEqual(404, result2.StatusCode);
        }
        [TestMethod]
        [DynamicData("IznajmljivaciJSON")]
        public async Task Create_ValidModelState_RedirectsToIndexJSON(Iznajmljivac iznajmljivac)
        {
            var controller = new IznajmljivacController(_dbContext);

            var result = await controller.Create(iznajmljivac) as RedirectToActionResult;

            Assert.IsNotNull(result);
            var check = _dbContext.Iznajmljivaci.Any(x => x.Id == iznajmljivac.Id);
            Assert.IsTrue(check);
            Assert.AreEqual("Index", result.ActionName);
        }
        [TestMethod]
        [DynamicData("IznajmljivaciXML")]
        public async Task Create_ValidModelState_RedirectsToIndexXML(string Id, string UserName, string Email)
        {
            var controller = new IznajmljivacController(_dbContext);
            var iznajmljivac = new Iznajmljivac { Id = Id, Email = Email, UserName = UserName };
            var result = await controller.Create(iznajmljivac) as RedirectToActionResult;

            Assert.IsNotNull(result);
            var check = _dbContext.Iznajmljivaci.Any(x => x.Id == iznajmljivac.Id);
            Assert.IsTrue(check);
            Assert.AreEqual("Index", result.ActionName);
        }
        [TestMethod]
        public async Task Create_ValidModelState_RedirectsToIndex()
        {
            var controller = new IznajmljivacController(_dbContext);
            var iznajmljivac = new Iznajmljivac { Id = "999", Email = "mail@gmail.com", UserName = "temp" };
            var result = await controller.Create(iznajmljivac) as RedirectToActionResult;

            Assert.IsNotNull(result);
            var check = _dbContext.Iznajmljivaci.Any(x => x.Id == iznajmljivac.Id);
            Assert.IsTrue(check);
            Assert.AreEqual("Index", result.ActionName);
        }
        [TestMethod]
        public async Task Edit_ValidId_ReturnsModel()
        {
            var controller = new IznajmljivacController (_dbContext);
            var result = await controller.Edit("1") as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Iznajmljivac;
            Assert.AreEqual("1", model.Id);
        }
        [TestMethod]
        public async Task Edit_InvalidId_ReturnsNotFound()
        {
            var controller = new IznajmljivacController(_dbContext);

            var result1 = await controller.Edit("5") as NotFoundResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual(404, result1.StatusCode);
        }
        [TestMethod]
        public async Task Edit_InvalidIdNull_ReturnsNotFound()
        {
            var controller = new IznajmljivacController(_dbContext);
            var result2 = await controller.Edit(null) as NotFoundResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual(404, result2.StatusCode);
        }
        [TestMethod]
        public async Task DeleteConfirmed_ValidId_DeletesIznajmljivac()
        {
            var controller = new IznajmljivacController(_dbContext);

            var result = await controller.DeleteConfirmed("1") as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var deletedIznajmljivac = _dbContext.Iznajmljivaci.Find("1");

            Assert.IsNull(deletedIznajmljivac);
        }
        [TestMethod]
        public async Task Delete_WithValidId_ReturnsView()
        {
            var controller = new IznajmljivacController(_dbContext);
            var result = await controller.Delete("1") as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as Iznajmljivac;  
            Assert.IsNotNull(model);
            Assert.AreEqual("Iznajmljivac1", model.UserName);
        }
        [TestMethod]
        public async Task Delete_WithInValidId_ReturnsNotFoundResult()
        {
            var controller = new IznajmljivacController(_dbContext);
            var result1 = await controller.Delete("999") as NotFoundResult;
            var result2 = await controller.Delete(null) as NotFoundResult;
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result1, typeof(NotFoundResult));
            Assert.IsInstanceOfType(result2, typeof(NotFoundResult));
        }
        [TestMethod]
        public void Create_ReturnsViewResult()
        {
            var controller = new IznajmljivacController(_dbContext);

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task IznajmljivacExists_WithValidId_ReturnsTrue()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new IznajmljivacController(_dbContext);

            var methodInfo = typeof(IznajmljivacController).GetMethod("IznajmljivacExists", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo, "Method not found");

            var result = methodInfo.Invoke(controller, new object[] { "1" }) as bool?;

            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public async Task Edit_ValidModelState_RedirectsToIndex()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new IznajmljivacController(_dbContext);

            // Update
            iznajmljivac.UserName = "IznajmljivacPromijenjen";

            // Act
            var result = await controller.Edit(iznajmljivac.Id, iznajmljivac) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updatedIznajmljivac = _dbContext.Iznajmljivaci.FirstOrDefault(r => r.Id == iznajmljivac.Id);
            Assert.IsNotNull(updatedIznajmljivac);
            Assert.AreEqual(iznajmljivac.UserName, updatedIznajmljivac.UserName);
        }
        [TestMethod]
        public async Task Edit_MismatchedIds_ReturnsNotFoundResult()
        {
            await _dbContext.SaveChangesAsync();
            var controller = new IznajmljivacController(_dbContext);
            // Update
            iznajmljivac.UserName = "IznajmljivacPromijenjen";
            // Act
            var result = await controller.Edit(iznajmljivac.Id+1, iznajmljivac) as NotFoundResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        
    }
}
