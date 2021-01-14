using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic.Manager
{
    public class TaxManagerMock : ITaxManager
	{
        private static readonly List<Tax> taxList = new List<Tax>();
        private readonly List<Tax> taxes = taxList; 

        public TaxManagerMock()
		{
        }

        public Task AddTaxResultAsync(Tax tax)
        {
            taxes.Add(tax);
            return Task.FromResult(tax);
        }
    }
}
