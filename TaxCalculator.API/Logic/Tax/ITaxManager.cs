using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic.Manager
{
    public interface ITaxManager
	{
		Task AddTaxResultAsync(Tax tax);
	}
}
