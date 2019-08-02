using Compute.Application.Controllers;
using Compute.Domain.Models;
using FakeItEasy;
using System;
using Xunit;

namespace Compute.Test
{
    public class ComputeDomainUnitTest
    {
        [Fact]
        public void Add_ValidValue_ReturnsCorrectResult()
        {
            // Arange
            double x = 5;
            double y = 8;
            var operation = new Operation(x, y, 1);

            // Act
            var result = operation.Result;
            // Assert
            Assert.True(result.Equals(13));
        }

        [Fact]
        public void Sub_ValidValue_ReturnsCorrectResult()
        {
            // Arange
            double x = 8;
            double y = 2;
            var operation = new Operation(x, y, 2);

            // Act
            var result = operation.Result;
            // Assert
            Assert.True(result.Equals(6));
        }

        [Fact]
        public void Mul_ValidValue_ReturnsCorrectResult()
        {
            // Arange
            double x = 8;
            double y = 2;
            var operation = new Operation(x, y, 3);

            // Act
            var result = operation.Result;
            // Assert
            Assert.True(result.Equals(16));
        }

        [Fact]
        public void Div_ValidValue_ReturnsCorrectResult()
        {
            // Arange
            double x = 16;
            double y = 4;
            var operation = new Operation(x, y, 4);

            // Act
            var result = operation.Result;
            // Assert
            Assert.True(result.Equals(4));
        }


    }
}
