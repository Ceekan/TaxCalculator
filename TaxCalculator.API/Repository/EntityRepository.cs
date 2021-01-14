using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaxCalculator.API.Data;

namespace TaxCalculator.API.Repository
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, new()
    {
        private readonly TaxCalculatorDBContext context;
        private readonly DbSet<T> dbSet;

        public EntityRepository(TaxCalculatorDBContext taxCalculatorDBContext)
        {
            context = taxCalculatorDBContext;
            dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public virtual async Task<T> FindByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public virtual async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
