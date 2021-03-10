using Dms.Api.Controllers;
using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Api
{
    public class PdfController_GetTest
    {


        [Test]
        public async Task GetFilesAsync_Test()
        {
            var testItems = new DmsFileView[] 
            { 
                new DmsFileView{ Name = "Test"},
                new DmsFileView{ Name = "Test2"}
            };

            var mockFileService = new Mock<IDmsFileService>();
            mockFileService.Setup(x => x.GetFileViewsAsync())
                .Returns(Task.FromResult<IEnumerable<DmsFileView>>(testItems))
                .Verifiable();

            var contorller = new PdfController(mockFileService.Object);

            //Act
            var result = await contorller.GetAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 2);
        }

    }
}
