using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic
{
	public class TaxManagerMock : ITaxManager
	{
        private readonly List<PostalCode> postalCodes;

        private static readonly List<Tax> taxList = new List<Tax>();
        private readonly List<Tax> taxes = taxList; 

        public TaxManagerMock()
		{
            postalCodes = new List<PostalCode>()
            {
                new PostalCode()
                {
                    Id = 1,
                    Code = "7441",
                    TaxCalculationType = "Progressive"
                },
                new PostalCode()
                {
                    Id = 2,
                    Code = "A100",
                    TaxCalculationType = "Flat Value"
                },
                new PostalCode()
                {
                    Id = 3,
                    Code = "7000",
                    TaxCalculationType = "Flat rate"
                },
                new PostalCode()
                {
                    Id = 4,
                    Code = "1000",
                    TaxCalculationType = "Progressive"
                }
            };
        }

        public Task AddTaxResult(Tax tax)
        {
            taxes.Add(tax);
            return Task.FromResult(tax);
        }

        public Task<PostalCode> GetPostalCodeById(int postalCodeId)
        {
            var postalCode = postalCodes
                .Where(x => x.Id == postalCodeId)
                .FirstOrDefault();

            return Task.FromResult(postalCode);
        }

        public Task<List<PostalCode>> GetPostalCodes()
        {
            return Task.FromResult(postalCodes);
        }
    }
}
