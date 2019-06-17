using DataDesafio;
using Desafio.Data.Interfaces;
using Desafio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        protected readonly DesafioContext context;

        public Repository(DesafioContext context)
        {
            this.context = context;
        }

        public abstract IEnumerable<T> GetAll();
        public abstract T Get(Guid id);
        public abstract void Add(T entity);
        public abstract void Update(T entity);
        public abstract void Delete(T entity);
    }
}
