using Desafio.Domain;
using Desafio.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirusTotalServices.Interface
{
    public interface IVirusTotalService
    {
        ScannedFileJson ScanFile(string fileName, byte[] data);
        ReportedFileJson Report(string resource);
    }
}
