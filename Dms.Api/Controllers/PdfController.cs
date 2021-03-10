using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private IDmsFileService _dmsFileService;
        public PdfController(IDmsFileService dmsFileService)
        {
            _dmsFileService = dmsFileService;
        }


        // GET: api/<PdfController>
        [HttpGet]
        public async Task<IEnumerable<DmsFileView>> GetAsync()
        {
            return await _dmsFileService.GetFileViewsAsync();
        }

        // GET api/<PdfController>/5
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            var fileExist = await _dmsFileService.CheckFileExistAsync(fileName);
            if (!fileExist)
            {
                return BadRequest($"File not found: {fileName}");
            }

            var dmsFile = await _dmsFileService.GetFileAsync(fileName);

            MemoryStream memStream = new MemoryStream(dmsFile.FileContent);
            memStream.Position = 0;
            return File(memStream, "application/octet-stream", dmsFile.Name);
        }

        // POST api/<PdfController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Invalid input file.");
            }

            if (file.Length > 5000000)
            {
                return BadRequest("File too large. File must be under 5Mb size.");
            }

            var extension = Path.GetExtension(file.FileName);
            if (extension.ToLower() != ".pdf")
            {
                return BadRequest($"Invalid file type: {extension}");
            }

            var fileExist = await _dmsFileService.CheckFileExistAsync(file.FileName);
            if (fileExist)
            {
                return BadRequest($"File already exists: {file.FileName}");
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var dmsFile = new DmsFile
            {
                Name = file.FileName,
                FileSize = file.Length,
                Location = $"http://localhost:49826/api/pdf/{file.FileName}", // TODO make base URL configurable 
                FileContent = ms.ToArray()
            };
            return Ok(await _dmsFileService.AddFileAsync(dmsFile));
        }

        // DELETE api/<PdfController>/5
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteAsync(string fileName)
        {
            var fileExist = await _dmsFileService.CheckFileExistAsync(fileName);
            if (!fileExist)
            {
                return BadRequest($"File not found: {fileName}");
            }
            await _dmsFileService.DeleteFileAsync(fileName);
            return Ok(true);
        }
    }
}
