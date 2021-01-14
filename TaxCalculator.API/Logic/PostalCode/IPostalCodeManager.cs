using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic.Manager
{
    public interface IPostalCodeManager
	{
		Task<PostalCode> GetPostalCodeByIdAsync(int postalCodeId);
		Task<List<PostalCode>> GetPostalCodesAsync();
	}
}
