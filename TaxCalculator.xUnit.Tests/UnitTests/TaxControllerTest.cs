using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaxCalculator.API.Controllers;
using TaxCalculator.API.Data.Dto;
using TaxCalculator.API.Logic.Manager;
using Xunit;

namespace TaxCalculator.Tests
{
    public class TaxControllerTest
    {
        readonly TaxController _controller;
        readonly TaxManagerMock _taxManagerService;
        readonly PostalCodeManagerMock _postalCodeService;

        public TaxControllerTest()
        {
            _taxManagerService = new TaxManagerMock();
            _postalCodeService = new PostalCodeManagerMock();
            _controller = new TaxController(_taxManagerService, _postalCodeService);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetPostalCodesAsync();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllPostalCodes()
        {
            // Act
            var okResult = _controller.GetPostalCodesAsync().Result as OkObjectResult;

            // Assert
            var postalCodes = Assert.IsType<List<PostalCodeDto>>(okResult.Value);
            Assert.Equal(4, postalCodes.Count);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Act
            var badResponse = _controller.AddTaxResultAsync(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Add_UnknownPostalCodeIDPassed_ReturnsBadRequest()
        {
            // Arrange
            var testTaxDto = new TaxDto()
            {
                AnnualIncome = "100000",
                PostalCodeId = "5"
            };

            // Act
            var badResponse = _controller.AddTaxResultAsync(testTaxDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsOkResponse()
        {
            // Arrange
            var testTaxDto = new TaxDto()
            {
                AnnualIncome = "100000",
                PostalCodeId = "1"
            };

            // Act
            var createdResponse = _controller.AddTaxResultAsync(testTaxDto);

            // Assert
            Assert.IsType<OkObjectResult>(createdResponse.Result);
        }
    }
}