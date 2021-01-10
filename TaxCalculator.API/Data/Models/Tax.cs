using System;

namespace TaxCalculator.API.Data.Models
{
    public partial class Tax
    {
        public Guid Id { get; set; }
        public decimal AnnualIncome { get; set; }
        public int PostalCodeId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal CalculatedTaxValue { get; set; }

        public virtual PostalCode PostalCode { get; set; }
    }
}
