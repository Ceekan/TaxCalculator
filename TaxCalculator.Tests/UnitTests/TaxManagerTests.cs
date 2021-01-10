﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculator.API.Data;
using TaxCalculator.API.Data.Models;
using TaxCalculator.API.Logic;

namespace TaxCalculator.Tests.UnitTests
{
    public class TaxManagerTests
    {
        private List<PostalCode> postalCodes;
        private List<Tax> taxes;

        [SetUp]
        public void Setup()
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

            taxes = new List<Tax>()
            {
                new Tax()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    AnnualIncome = 500000.00m,
                    CalculatedTaxValue = 152682.14m,
                    PostalCodeId = 1,
                    Timestamp = DateTime.Now
                }
            };
        }

        [Test]
        public async Task Logic_TaxManager_GetPostalCodes()
        {
            //Arrange
            var mockSet = TaxCalculatorDBContextMock.GetMockDbSet(postalCodes);

            var mockContext = new Mock<TaxCalculatorDBContext>();
            mockContext.Setup(c => c.PostalCodes).Returns(mockSet.Object);

            var service = new TaxManager(mockContext.Object);

            //Act
            var result = await service.GetPostalCodes().ConfigureAwait(false);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public async Task Logic_TaxManager_GetPostalCodeById()
        {
            //Arrange
            var mockSet = TaxCalculatorDBContextMock.GetMockDbSet(postalCodes);

            var mockContext = new Mock<TaxCalculatorDBContext>();
            mockContext.Setup(c => c.PostalCodes).Returns(mockSet.Object);

            var service = new TaxManager(mockContext.Object);

            //Act
            var result = await service.GetPostalCodeById(1).ConfigureAwait(false);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postalCodes[0].Id, result.Id);
        }

        [Test]
        public async Task Logic_TaxManager_AddTaxResult()
        {
            // Arrange
            var mockSet = TaxCalculatorDBContextMock.GetMockDbSet(taxes);

            var mockContext = new Mock<TaxCalculatorDBContext>();
            mockContext.Setup(c => c.Taxes).Returns(mockSet.Object);

            var service = new TaxManager(mockContext.Object);

            var taxResult = new Tax()
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111111"),
                AnnualIncome = 500000.00m,
                CalculatedTaxValue = 152682.14m,
                PostalCodeId = 1,
                Timestamp = DateTime.Now
            };

            // Act
            await service.AddTaxResult(taxResult).ConfigureAwait(false);

            // Assert
            mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockSet.Verify(x => x.AddAsync(It.IsAny<Tax>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
