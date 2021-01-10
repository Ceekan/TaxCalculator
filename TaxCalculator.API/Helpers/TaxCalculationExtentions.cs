using System;
using System.Collections.Generic;
using TaxCalculator.API.Helpers.Constants.Enum;

namespace TaxCalculator.API.Helpers
{
    public static class TaxCalculationExtentions
    {
        public static decimal ToDecimal(this string input)
        {
            var isValid = false;
            while (!isValid)
            {
                isValid = decimal.TryParse(input, out var number);
                if (isValid)
                    return Math.Round(number, 2);
            }

            return 0m;
        }

        public static TaxCalculationType ToEnum(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default;

            var types = new Dictionary<string, TaxCalculationType> {
                { "Progressive", TaxCalculationType.Progressive },
                { "Flat Value", TaxCalculationType.FlatValue },
                { "Flat rate", TaxCalculationType.FlatRate }
            };

            var validEnumType = types.TryGetValue(value, out TaxCalculationType enumType);

            if (!validEnumType)
                return default;

            return enumType;
        }

        public static string ToTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }

        public static decimal GetTaxResult(ITaxCalculation taxCalculation, TaxCalculationType taxCalculationType, decimal annualIncome)
        {
            return taxCalculationType switch
            {
                TaxCalculationType.Progressive => taxCalculation.ProgressiveCalculation(annualIncome),
                TaxCalculationType.FlatValue => taxCalculation.FlatValueCalculation(annualIncome),
                TaxCalculationType.FlatRate => taxCalculation.FlatRateCalculation(annualIncome),
                _ => throw new InvalidOperationException("Operation failed")
            };
        }
    }
}
