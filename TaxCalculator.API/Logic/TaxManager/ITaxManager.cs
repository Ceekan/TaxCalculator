using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic
{
    public interface ITaxManager
	{
		Task<PostalCode> GetPostalCodeById(int postalCodeId);
		Task<List<PostalCode>> GetPostalCodes();
		Task AddTaxResult(Tax tax);
	}
}
