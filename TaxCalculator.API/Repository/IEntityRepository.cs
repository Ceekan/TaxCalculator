using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaxCalculator.API.Repository
{
    public interface IEntityRepository<T> where T : class, new()
    {
        Task AddAsync(T entity);
        IQueryable<T> GetAll();
        Task<T> FindByIdAsync(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task SaveAsync();
    }
}
