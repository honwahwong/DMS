using Dms.Api.Controllers;
using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Api
{
    public class PdfController_AddTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, TestCaseSource("GetTesttems")]
        public async Task AddFileAsync_Test(IFormFile mockFile, int expectedResultStatusCode, string expectedResultValue, bool fileCheckResult)
        {
            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.CheckFileExistAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<bool>(fileCheckResult))
                .Verifiable();

            mockFileService.Setup(x => x.AddFileAsync(It.IsAny<DmsFile>()))
                .Returns(Task.FromResult<int>(1))
                .Verifiable();

            var controller = new PdfController(mockFileService.Object);

            //Act
            var result = await controller.PostAsync(mockFile) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(expectedResultStatusCode, result.StatusCode);
            Assert.AreEqual(expectedResultValue, result.Value.ToString());
        }

        public static IEnumerable<object[]> GetTesttems()
        {
            yield return
                new object[]
                {
                    CreateMockIFormFileObject("test.pdf", 10),
                    200,
                    "1",
                    false
                };

            yield return
                new object[]
                {
                    CreateMockIFormFileObject("test.XXX", 10),
                    400,
                    "Invalid file type: .XXX",
                    false
                };

            yield return
                new object[]
                {
                    CreateMockIFormFileObject("test.pdf", 5000001),
                    400,
                    "File too large. File must be under 5Mb size.",
                    false
                };

            yield return
                new object[]
                {
                    CreateMockIFormFileObject("test.pdf", 10),
                    400,
                    "File already exists: test.pdf",
                    true
                };

            yield return
                new object[]
                {
                    null,
                    400,
                    "Invalid input file.",
                    false
                };
        }

        public static IFormFile CreateMockIFormFileObject(string fileName, int fileSize)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Mock file content");
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns(fileName);
            fileMock.Setup(x => x.Length).Returns(fileSize);
            fileMock.Setup(x => x.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream));
            return fileMock.Object;
        }
    }
}
