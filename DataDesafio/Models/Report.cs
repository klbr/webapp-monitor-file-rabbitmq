using Desafio.Data.Interfaces;
using Desafio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDesafio.Models
{
    public class Report : IEntity
    {
        public Report()
        {
            Id = Guid.NewGuid();
            LastUpdatedAt = CreatedAt = DateTimeOffset.Now;
        }

        public Guid Id { get; set; }

        public virtual ICollection<ReportItem> Items { get; set; }

        public string JsonMd5 { get; set; }        
        public string VerboseMsg { get; set; }
        public int Total { get; set; }
        public int Positives { get; set; }
        public string Resource { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string Filename { get; set; }
        public string ScanId { get; set; }
        public string Permalink { get; set; }
    }
}
