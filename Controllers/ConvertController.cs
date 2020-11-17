using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wkhtmltopdf.NetCore;

namespace service.html2pdfconverter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConvertController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        readonly IGeneratePdf _generatePdf;

        public ConvertController(ILogger<ConvertController> logger, IGeneratePdf generatePdf)
        {
            _logger = logger;
            _generatePdf = generatePdf;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string body = string.Empty;
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }
            
            if(string.IsNullOrWhiteSpace(body))
                return BadRequest();

            var pdf = _generatePdf.GetPDF(body);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/octet-stream");
        }
    }
}
