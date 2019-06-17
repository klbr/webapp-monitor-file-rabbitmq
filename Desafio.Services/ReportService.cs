using AutoMapper;
using DataDesafio.Models;
using Desafio.Data.Interfaces;
using Desafio.Domain;
using Desafio.Domain.DTO;
using Desafio.Services.Infra;
using Desafio.Services.Interfaces;
using Desafio.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirusTotalServices.Interface;

namespace Desafio.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IVirusTotalService virusTotalService;
        private readonly IEnqueueService queueService;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IVirusTotalService virusTotalService, IEnqueueService queueService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.virusTotalService = virusTotalService;
            this.queueService = queueService;            
        }

        private void UpdateReport(Guid id, ScannedFileJson scanOutput, string filename)
        {
            var report = unitOfWork.ReportRepository.Get(id);

            report.Permalink = scanOutput.Permalink;
            report.Resource = scanOutput.Resource;
            report.ScanId = scanOutput.ScanId;
            report.Filename = filename;
            report.JsonMd5 = "";
            report.VerboseMsg = "Scanner Initialized";
            report.LastUpdatedAt = DateTimeOffset.Now;
            
            unitOfWork.ReportRepository.Update(report);
            unitOfWork.Commit();
        }

        private void UpdateReport(Guid id, ReportedFileJson reportData)
        {
            var report = unitOfWork.ReportRepository.Get(id);

            report.Permalink = reportData.Permalink;
            report.Resource = reportData.Resource;
            report.ScanId = reportData.ScanId;            
            report.JsonMd5 = "";
            report.VerboseMsg = reportData.VerboseMsg;
            report.Positives = reportData.Positives;
            report.Total = reportData.Total;
            
            foreach(var scan in reportData.Scans)
            {
                var item = report.Items?.FirstOrDefault(x => x.ToolName == scan.Key);
                if (item == null)
                {
                    item = new ReportItem();                    
                    if (report.Items == null)
                    {
                        report.Items = new List<ReportItem>();
                    }
                    report.Items.Add(item);
                }

                item.ToolName = scan.Key;
                item.Result = scan.Value.Result ?? "";
                item.Detected = scan.Value.Detected;
                item.Update = scan.Value.Update;
                item.Version = scan.Value.Version;
            }

            report.LastUpdatedAt = DateTimeOffset.Now;
            unitOfWork.ReportRepository.Update(report);
            unitOfWork.Commit();
        }

        private Report CreateReport(string filename)
        {
            var report = new Report
            {
                Filename = filename,
                VerboseMsg = "Waiting scanner",
                Permalink = "",
                Resource= "",
                ScanId = "",
                JsonMd5= "",
            };

            unitOfWork.ReportRepository.Add(report);
            unitOfWork.Commit();
            return report;
        }

        public void EnqueuCreateReport(string filename, byte[] data)
        {
            var report = CreateReport(filename);
            queueService.Enqueue(new QueueMessage { Type= QueueType.CreateReport, Id = report.Id, FileName = filename, Data = data });
        }

        public void DequeueCreateReport(Guid id, string filename, byte[] data)
        {
            UpdateReport(id, virusTotalService.ScanFile(filename, data), filename);
        }

        public ReportDTO GetReport(Guid id) =>
             mapper.Map<ReportDTO>(unitOfWork.ReportRepository.GetAll().FirstOrDefault(x => x.Id == id));

        public IEnumerable<ReportDTO> GetReports() =>
            mapper.Map<IEnumerable<ReportDTO>>(unitOfWork.ReportRepository.GetAll().ToList());

        public void UpdateAllReports()
        {
            var reports = unitOfWork.ReportRepository.GetAll().Where(x => !x.VerboseMsg.StartsWith("Scan finished") && !string.IsNullOrEmpty(x.Resource)).ToList();
            foreach(var report in reports)
            {
                queueService.Enqueue(new QueueMessage { Type = QueueType.CheckReport, Id = report.Id, Resource = report.Resource });
            }
        }

        public void DequeueUpdateReport(Guid id, string resource)
        {
            var reportData = virusTotalService.Report(resource);
            UpdateReport(id, reportData);
        }
    }
}
