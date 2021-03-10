using Dms.Domain.Interfaces;
using Dms.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PdfController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PdfController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PdfController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
