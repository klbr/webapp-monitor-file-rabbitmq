using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirusTotalServices.Interface;
using WebAppDesafio.Models;

namespace WebAppDesafio.Controllers
{
    [Route("api/[controller]")]
    public class ProcessController : Controller
    {
        private static readonly string[] supportedExtension = new[] { "exe", "dll" };
        private readonly IVirusTotalService virusTotalService;

        public ProcessController(IVirusTotalService virusTotalService)
        {
            this.virusTotalService = virusTotalService;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var file = Request?.Form?.Files.FirstOrDefault();

            if (file == null)
            {
                return BadRequest("File not found");
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

            var scanOutput = virusTotalService.ScanFile(file.FileName, data);
            return Ok(new ProcessModel { Resource = scanOutput.Resource });
        }       
    }
}
