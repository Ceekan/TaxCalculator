using System.Linq;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TaxCalculatorDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.PostalCodes.Any())
            {
                return;
            }

            var postalCodes = new PostalCode[]
            {
                new PostalCode
                {
                    Code = "7441",
                    TaxCalculationType = "Progressive"
                },
                new PostalCode
                {
                    Code= "A100",
                    TaxCalculationType = "Flat Value"
                },
                new PostalCode
                {
                    Code = "7000",
                    TaxCalculationType = "Flat rate"
                },
                new PostalCode{
                    Code = "1000",
                    TaxCalculationType = "Progressive"
                }
            };

            context.PostalCodes.AddRange(postalCodes);
            context.SaveChanges();
        }
    }
}
