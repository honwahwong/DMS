using Dms.Api.Controllers;
using Dms.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTest
{
    public class Case1_Test
    {
        private DmsFileContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DmsFileContext>();
            builder.UseSqlServer("Server=.;Database=DmsFilesIntegrationTest1;Trusted_Connection=True;")
                .UseInternalServiceProvider(serviceProvider);

            _context = new DmsFileContext(builder.Options);
            _context.Database.Migrate();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }



        [Test]
        public async Task Full_Test()
        {
            await PostAsync_Test();
            await GetFileViewAsync_Test();
            await GetFileAsync_Test();
            await DeleteAsync_Test();
        }

        public async Task PostAsync_Test()
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Mock file content");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("Test.pdf");
            fileMock.Setup(x => x.Length).Returns(ms.Length);
            fileMock.Setup(x => x.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream));

            var beforeCount = await _context.DmsFiles.CountAsync();

            var dmsFileService = new DmsFileService(_context);
            var dmsContorller = new PdfController(dmsFileService);

            //Act
            var result = await dmsContorller.PostAsync(fileMock.Object) as ObjectResult;

            //Assert
            var afterCount = await _context.DmsFiles.CountAsync();
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.IsTrue(afterCount - beforeCount == 1);
        }

        public async Task GetFileViewAsync_Test()
        {            
            var beforeCount = await _context.DmsFiles.CountAsync();

            var dmsFileService = new DmsFileService(_context);
            var dmsContorller = new PdfController(dmsFileService);

            //Act
            var result = await dmsContorller.GetAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(beforeCount == result.Count());
        }

        public async Task GetFileAsync_Test()
        {            
            var dmsFileService = new DmsFileService(_context);
            var dmsContorller = new PdfController(dmsFileService);

            //Act
            var result = await dmsContorller.GetFileAsync("Test.pdf");

            //Assert
            Assert.IsNotNull(result);
        }

        public async Task DeleteAsync_Test()
        {
            var dmsFileService = new DmsFileService(_context);
            var dmsContorller = new PdfController(dmsFileService);

            //Act
            var result = await dmsContorller.DeleteAsync("Test.pdf");

            //Assert
            var afterCount = await _context.DmsFiles.CountAsync();
            Assert.IsTrue(afterCount == 0);
            Assert.IsNotNull(result);

        }
    }
}
