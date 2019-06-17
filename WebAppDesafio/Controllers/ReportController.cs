using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.WebApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService service;

        public ReportController(IReportService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            var reports = service.GetReports();
            return View(reports);
        }

        public IActionResult Details(Guid id)
        {
            var report = service.GetReport(id);
            return View(report);
        }
    }
}