using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;
using TaxCalculator.API.Repository;

namespace TaxCalculator.API.Logic.Manager
{
    public class TaxManager: ITaxManager
    {
        private readonly IEntityRepository<Tax> _entityRepository;

        public TaxManager(IEntityRepository<Tax> entityRepository)
            : base()
        {
            _entityRepository = entityRepository;
        }

        // <summary>
        /// Insert tax result into database
        /// </summary>
        /// <param name="tax"></param>
        /// <returns></returns>
        public async Task AddTaxResultAsync(Tax tax)
        {
            await _entityRepository.AddAsync(tax);
            await _entityRepository.SaveAsync();
        }
    }
}
