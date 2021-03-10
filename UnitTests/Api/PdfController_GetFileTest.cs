using Dms.Api.Controllers;
using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Api
{
    public class PdfController_GetFileTest
    {
        [Test]
        public async Task GetFileAsync_Test()
        {
            var fileName = "Test.pdf";
            var dmsFile = new DmsFile { Id = 1, Name = fileName, FileContent = new byte[] { 1, 2, 3 } };

            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.CheckFileExistAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<bool>(true))
                .Verifiable();

            mockFileService.Setup(x => x.GetFileAsync(fileName))
                .Returns(Task.FromResult<DmsFile>(dmsFile))
                .Verifiable();

            var contorller = new PdfController(mockFileService.Object);

            //Act
            var result = await contorller.GetFileAsync(fileName);

            //Assert
            Assert.IsInstanceOf<IActionResult>(result);            
        }

        [Test]
        public async Task GetFileAsync_FileNotFoundTest()
        {
            var fileName = "Test.pdf";
           
            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.CheckFileExistAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<bool>(false))
                .Verifiable();

            var contorller = new PdfController(mockFileService.Object);

            //Act
            var result = await contorller.GetFileAsync(fileName) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("File not found: Test.pdf", result.Value.ToString());
        }
    }
}
