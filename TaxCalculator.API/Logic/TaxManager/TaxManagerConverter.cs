using TaxCalculator.API.Data.Dto;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.API.Logic
{
    public static class TaxManagerConverter
	{
		public static PostalCodeDto ToDto(this PostalCode postalCode)
		{
			return new PostalCodeDto
			{
				Id = postalCode.Id,
				Name = postalCode.Code
			};
		}
	}
}
