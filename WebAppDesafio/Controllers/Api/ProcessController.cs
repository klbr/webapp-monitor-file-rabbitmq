using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Desafio.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirusTotalServices.Interface;
using WebAppDesafio.Models;

namespace WebAppDesafio.Controllers
{
    [Route("api/[controller]")]
    public class ProcessController : Controller
    {
        private static readonly string[] supportedExtension = new[] { "exe", "dll" };

        private readonly IReportService scannedFileService;

        public ProcessController(IReportService scannedFileService)
        {            
            this.scannedFileService = scannedFileService;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var file = Request?.Form?.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                return BadRequest("File not found or empty");
            }
            
            if (!supportedExtension.Any(ext => file.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Unexpected extension");
            }

            byte[] data;
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                data = binaryReader.ReadBytes((int)file.OpenReadStream().Length);
            }
            
            scannedFileService.EnqueuCreateReport(file.FileName, data);

            return RedirectToAction("Index", "Report");
        }       
    }
}
