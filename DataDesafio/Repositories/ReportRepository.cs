using DataDesafio;
using DataDesafio.Models;
using Desafio.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desafio.Data.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(DesafioContext context) : base(context)
        {
        }

        public override void Add(Report entity)
        {
            context.Reports.Add(entity);
        }

        public override void Delete(Report entity)
        {
            context.Reports.Remove(entity);
        }

        public override Report Get(Guid id)
        {
            return context.Reports.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
        }

        public override IEnumerable<Report> GetAll()
        {
            return context.Reports.Include(x => x.Items);
        }

        public override void Update(Report entity)
        {
            context.Reports.Update(entity);
        }
    }
}
