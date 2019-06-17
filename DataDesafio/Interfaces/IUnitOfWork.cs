using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IReportRepository ReportRepository { get; }

        void Commit();
    }
}
