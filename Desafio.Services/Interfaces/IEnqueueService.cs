using Desafio.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Services.Interfaces
{
    public interface IEnqueueService
    {
        void Enqueue(QueueMessage messageInput);        
    }
}
