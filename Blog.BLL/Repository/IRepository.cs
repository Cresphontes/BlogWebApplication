using Blog.Models.Abstracts;
using System;
using System.Linq;

namespace Blog.BLL.Repository
{
    public interface IRepository<T, TId> where T : BaseEntity<TId>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Func<T, bool> predicate);
        T GetById(TId id);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}
