using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Services.Models
{
    public class QueueMessage
    {
        public QueueType Type { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string Resource { get; set; }
        public Guid Id { get; set; }
    }
}
