using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Dms.FileService;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.FileService
{
    public class DmsFileServiceTest
    {
        protected DbContextOptions<DmsFileContext> _options;
        protected DmsFile _mockFileItem;

        [OneTimeSetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<DmsFileContext>()
                .UseInMemoryDatabase(databaseName: "Mock Dms File 1")
                .Options;

            var context = new DmsFileContext(_options);
            _mockFileItem = new DmsFile { Id = 1, Name = "MockFile1.pdf" };

            context.DmsFiles.Add(_mockFileItem);
            context.DmsFiles.Add(new DmsFile { Id = 2, Name = "MockDeleteFile1.pdf" });
            context.SaveChanges();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            var context = new DmsFileContext(_options);
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test, TestCaseSource("GetTesttems")]
        public async Task CheckFileExistAsync_Test(string fileName, bool expectedResult)
        {
            var context = new DmsFileContext(_options);
            IDmsFileService service = new DmsFileService(context);
            var result = await service.CheckFileExistAsync(fileName);
            Assert.AreEqual(expectedResult, result);
        }

        public static IEnumerable<object[]> GetTesttems()
        {
            yield return
                new object[]
                {
                    "MockFile1.pdf",
                    true
                };

            yield return
                new object[]
                {
                    "new_file_name.pdf",
                    false
                };
        }

        [Test]
        public async Task AddFileAsync_Test()
        {
            var testItem = new DmsFile
            {
                Id = 0,
                Name = "Test File Name",
                FileContent = new byte[] { 1, 2, 3, 4 }
            };

            var context = new DmsFileContext(_options);
            var beforeCount = await context.DmsFiles.CountAsync();

            IDmsFileService service = new DmsFileService(context);
            var result = await service.AddFileAsync(testItem);

            var afterCount = await context.DmsFiles.CountAsync();
            Assert.IsTrue(result != 0);
            Assert.IsTrue(afterCount - beforeCount == 1);
        }

        [Test]
        public async Task DeleteFileAsync_Test()
        {
            var fileName = "MockDeleteFile1.pdf";
            var context = new DmsFileContext(_options);
            var beforeCount = await context.DmsFiles.CountAsync();

            IDmsFileService service = new DmsFileService(context);
            await service.DeleteFileAsync(fileName);

            var afterCount = await context.DmsFiles.CountAsync();            
            Assert.IsTrue(beforeCount - afterCount == 1);
        }

        [Test]
        public async Task GetFileAsync_Test()
        {
            var fileName = "MockFile1.pdf";
            var context = new DmsFileContext(_options);
            
            IDmsFileService service = new DmsFileService(context);
            var result = await service.GetFileAsync(fileName);

            Assert.IsNotNull(result);
            Assert.AreEqual(_mockFileItem.Id, result.Id);
        }

        [Test]
        public async Task GetFileViewAsync_Test()
        {            
            var context = new DmsFileContext(_options);
            var count = await context.DmsFiles.CountAsync();

            IDmsFileService service = new DmsFileService(context);
            var result = await service.GetFileViewsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(count, result.Count());
        }
    }
}
