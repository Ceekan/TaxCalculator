using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.API.Data;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic
{
    public class TaxManager : ITaxManager
	{
        private readonly TaxCalculatorDBContext _context;

        public TaxManager(TaxCalculatorDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insert tax result into database
        /// </summary>
        /// <param name="tax"></param>
        /// <returns></returns>
        public async Task AddTaxResult(Tax tax)
        {
            await _context.Taxes.AddAsync(tax);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Return postal code by ID
        /// </summary>
        /// <param name="postalCodeId"></param>
        /// <returns></returns>
        public async Task<PostalCode> GetPostalCodeById(int postalCodeId)
        {
            return await _context.PostalCodes.FirstOrDefaultAsync(x => x.Id == postalCodeId);
        }

        /// <summary>
        /// Returns all postal codes
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostalCode>> GetPostalCodes()
        {
            return await _context.PostalCodes.ToListAsync();
        }
    }
}
