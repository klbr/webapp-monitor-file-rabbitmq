using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Domain.DTO
{
    public class ReportedFileJson
    {
        public Dictionary<string, Scan> Scans { get; set; }
        public string ScanId { get; set; }
        public string Resource { get; set; }
        public string Permalink { get; set; }
        public string VerboseMsg { get; set; }
        public int Total { get; set; }
        public int Positives { get; set; }
    }

    public class Scan
    {
        public bool Detected { get; set; }
        public string Version { get; set; }
        public string Result { get; set; }
        public string Update { get; set; }
    }
}
