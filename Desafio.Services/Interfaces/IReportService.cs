using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Desafio.Domain;
using Desafio.Services.Models;

namespace Desafio.Services.Interfaces
{
    public interface IReportService
    {
        void EnqueuCreateReport(string fileName, byte[] data);
        void DequeueCreateReport(Guid id, string filename, byte[] data);
        void UpdateAllReports();
        IEnumerable<ReportDTO> GetReports();
        ReportDTO GetReport(Guid id);
        void DequeueUpdateReport(Guid id, string resource);
    }
}
