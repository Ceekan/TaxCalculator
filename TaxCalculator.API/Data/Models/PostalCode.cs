using System.Collections.Generic;

namespace TaxCalculator.API.Data.Models
{
    public partial class PostalCode
    {
        public PostalCode()
        {
            Taxes = new HashSet<Tax>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string TaxCalculationType { get; set; }

        public virtual ICollection<Tax> Taxes { get; set; }
    }
}