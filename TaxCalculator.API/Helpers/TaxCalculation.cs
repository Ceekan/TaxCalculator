using System;
using System.Collections.Generic;

namespace TaxCalculator.API.Helpers
{
    public class TaxCalculation : ITaxCalculation
    {
        private static List<ProgressiveBand> ProgressiveBands { get; set; }

        private const decimal FLAT_RATE = 0.175m;
        private const decimal FLAT_VALUE_RATE = 0.05m;
        private const decimal FLAT_VALUE_RATE_UPPER_BOUND = 200000m;

        public TaxCalculation()
        {
            ProgressiveBands = new List<ProgressiveBand>
            {
                new ProgressiveBand() { From = 0m, To = 8350m, Rate = 0.1m },
                new ProgressiveBand() { From = 8351m, To = 33950m, Rate = 0.15m },
                new ProgressiveBand() { From = 33951m, To = 82250m, Rate = 0.25m },
                new ProgressiveBand() { From = 82251m, To = 171550m, Rate = 0.28m },
                new ProgressiveBand() { From = 171551m, To = 372950m, Rate = 0.33m },
                new ProgressiveBand() { From = 372951m, To = decimal.MaxValue, Rate = 0.35m }
            };
        }

        public decimal FlatRateCalculation(decimal annualIncome)
        {
            decimal taxValue = annualIncome * FLAT_RATE;

            return Math.Round(taxValue, 2);
        }

        public decimal FlatValueCalculation(decimal annualIncome)
        {
            decimal taxValue = 10000m;

            if (annualIncome < FLAT_VALUE_RATE_UPPER_BOUND)
                taxValue = annualIncome * FLAT_VALUE_RATE;

            return Math.Round(taxValue, 2);
        }

        public decimal ProgressiveCalculation(decimal annualIncome)
        {
            decimal taxValue = 0m;

            foreach (var band in ProgressiveBands)
            {
                if (annualIncome > band.From)
                {
                    decimal taxableRate = Math.Min(band.To - band.From, annualIncome - band.From);
                    decimal currentTax = taxableRate * band.Rate;
                    taxValue += currentTax;
                }
            }

            return Math.Round(taxValue, 2);
        }
    }
}
