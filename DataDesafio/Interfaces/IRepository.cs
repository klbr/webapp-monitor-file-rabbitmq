using Desafio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Data.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);        
    }
}
