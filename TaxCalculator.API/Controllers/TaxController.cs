using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Dto;
using TaxCalculator.API.Data.Models;
using TaxCalculator.API.Helpers;
using TaxCalculator.API.Helpers.Constants;
using TaxCalculator.API.Logic.Manager;

namespace TaxCalculator.API.Controllers
{
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ITaxManager _taxManager;
        private readonly IPostalCodeManager _postalCodeManager;

        public TaxController(ITaxManager taxManager, IPostalCodeManager postalCodeManager)
        {
            _taxManager = taxManager;
            _postalCodeManager = postalCodeManager;
        }

        /// <summary>
        /// Return all postal codes
        /// </summary>
        /// <response code="200">OK </response> 
        /// <response code="401">Unauthorized </response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found</response>
        /// <response code="429">Too Many Requests</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns></returns>
        [Route(ApiControllerRoutes.TaxController.GET_POSTAL_CODES, Name = "GetPostalCodesAsync")]
        [ProducesResponseType(typeof(List<PostalCodeDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetPostalCodesAsync()
        {
            var items = await _postalCodeManager.GetPostalCodesAsync().ConfigureAwait(false);

            if (items == null || items.Count == 0)
                return NotFound();

            List<PostalCodeDto> postalCodes = new List<PostalCodeDto>();
            postalCodes = items.ConvertAll(x => x.ToDto());

            return Ok(postalCodes);
        }

        /// <summary>
        /// Insert the calculated tax result
        /// </summary>
        /// <param name="taxDto"></param>
        /// <response code="200">OK </response> 
        /// <response code="401">Unauthorized </response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found</response>
        /// <response code="429">Too Many Requests</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns></returns>
        [Route(ApiControllerRoutes.TaxController.ADD_TAX_RESULT, Name = "AddTaxResultAsync")]
        [ProducesResponseType(typeof(ResponseDto), 200)]
        [HttpPost]
        public async Task<IActionResult> AddTaxResultAsync([FromBody] TaxDto taxDto)
        {
            try
            {
                if (taxDto == null)
                    throw new ArgumentException($"Tax object not supplied.");

                int postalCodeId = int.Parse(taxDto.PostalCodeId);
                var postalCode = await _postalCodeManager.GetPostalCodeByIdAsync(postalCodeId).ConfigureAwait(false);

                if (postalCode == null)
                    throw new ArgumentException($"Postal code not found.");

                var taxCalulationType = postalCode.TaxCalculationType.ToEnum();
                decimal annualIncome = taxDto.AnnualIncome.ToDecimal();

                TaxCalculation taxCalculation = new TaxCalculation();
                decimal calculatedTaxValue = TaxCalculationExtentions.GetTaxResult(taxCalculation, taxCalulationType, annualIncome);

                await _taxManager.AddTaxResultAsync(new Tax()
                    {
                        Id = Guid.NewGuid(),
                        AnnualIncome = annualIncome,
                        PostalCodeId = postalCodeId,
                        CalculatedTaxValue = calculatedTaxValue,
                        Timestamp = DateTime.Now
                    })
                    .ConfigureAwait(false);

                return Ok(new ResponseDto()
                {
                    ResultMessage = "Tax calculation was successfully added."
                });
            }
            catch (Exception exc)
            {
                return BadRequest(new ResponseDto()
                {
                    ResultMessage = exc.Message
                });
            }
        }
    }
}
