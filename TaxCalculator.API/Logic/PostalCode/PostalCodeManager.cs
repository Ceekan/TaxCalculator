using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;
using TaxCalculator.API.Repository;

namespace TaxCalculator.API.Logic.Manager
{
    public class PostalCodeManager: IPostalCodeManager
    {
        private readonly IEntityRepository<PostalCode> _entityRepository;

        public PostalCodeManager(IEntityRepository<PostalCode> entityRepository)
            : base()
        {
            _entityRepository = entityRepository;
        }

        /// <summary>
        /// Return postal code by ID
        /// </summary>
        /// <param name="postalCodeId"></param>
        /// <returns></returns>
        public async Task<PostalCode> GetPostalCodeByIdAsync(int postalCodeId)
        {
            return await _entityRepository
                .FindBy(x => x.Id == postalCodeId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns all postal codes
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostalCode>> GetPostalCodesAsync()
        {
            return await _entityRepository.GetAll().ToListAsync();
        }
    }
}
