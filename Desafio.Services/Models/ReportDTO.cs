using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Services.Models
{
    public class ReportDTO
    {
        public Guid Id { get; set; }
        
        public ICollection<ReportItemDTO> Items { get; set; }
        
        public string VerboseMsg { get; set; }
        public int Total { get; set; }
        public int Positives { get; set; }
        public string Resource { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string Filename { get; set; }
        public string Permalink { get; set; }
    }
}
