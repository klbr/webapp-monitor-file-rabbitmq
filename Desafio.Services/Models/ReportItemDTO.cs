using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Services.Models
{
    public class ReportItemDTO
    {
        public Guid Id { get; set; }
        public string ToolName { get; set; }
        public bool Detected { get; set; }
        public string Version { get; set; }
        public string Result { get; set; }
        public string Update { get; set; }
    }
}
