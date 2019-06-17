using Desafio.Data.Interfaces;
using Desafio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDesafio.Models
{
    public class ReportItem : IEntity
    {
        public ReportItem()
        {
            Id = Guid.NewGuid();
        }

        public virtual Report Report { get; set; }

        public Guid Id { get; set; }
        public string ToolName { get; set; }
        public bool Detected { get; set; }
        public string Version { get; set; }
        public string Result { get; set; }
        public string Update { get; set; }
    }
}
