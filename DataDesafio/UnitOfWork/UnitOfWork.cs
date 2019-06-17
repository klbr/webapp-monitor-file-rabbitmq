using DataDesafio;
using Desafio.Data.Interfaces;
using Desafio.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DesafioContext context;

        public UnitOfWork(DesafioContext context)
        {
            this.context = context;
        }

        private IReportRepository reportRepository;
        public IReportRepository ReportRepository => reportRepository ?? (reportRepository = new ReportRepository(context));

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
