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
    public class PdfController_DeleteTest
    {
        [Test]
        public async Task DeleteAsync_Test()
        {
            var fileName = "Test.pdf";
            
            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.CheckFileExistAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<bool>(true))
                .Verifiable();

            mockFileService.Setup(x => x.DeleteFileAsync(fileName))
                .Verifiable();

            var contorller = new PdfController(mockFileService.Object);

            //Act
            var result = await contorller.DeleteAsync(fileName);

            //Assert
            Assert.IsInstanceOf<IActionResult>(result);            
        }

        [Test]
        public async Task DeleteAsync_FileNotFoundTest()
        {
            var fileName = "Test.pdf";
           
            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.CheckFileExistAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<bool>(false))
                .Verifiable();

            var contorller = new PdfController(mockFileService.Object);

            //Act
            var result = await contorller.DeleteAsync(fileName) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("File not found: Test.pdf", result.Value.ToString());
        }
    }
}
