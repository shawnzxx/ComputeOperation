using Compute.Application.Controllers;
using FakeItEasy;
using System;
using Xunit;

namespace Compute.Test
{
    public class ComputeControllerUnitTest
    {
        [Fact]
        public void Add_ValidValue_ReturnsCorrectResult()
        {
            // Arange
            double x = 5;
            double y = 8;
            var computeController = A.Fake<ComputeController>();

            // Act
            var result = computeController.Add(x, y);
            // Assert
            Assert.True(result.Value.Equals(13));
        }

        [Fact]
        public void Sub_ValidValue_ReturnsCorrectResult()
        {
            // CHALLENGE - Complete this test method
            double x = 10;
            double y = 3;

            var computeController = A.Fake<ComputeController>();

            var result = computeController.Sub(x, y);

            Assert.True(result.Value.Equals(7));
        }

        [Fact]
        public void Mul_ValidValue_ReturnsCorrectResult()
        {
            // CHALLENGE - Complete this test method
            double x = 2;
            double y = 1;

            var computeController = A.Fake<ComputeController>();

            var result = computeController.Mul(x, y);

            Assert.True(result.Value.Equals(2));
        }

        [Fact]
        public void Div_ValidValue_ReturnsCorrectResult()
        {
            // CHALLENGE - Complete this test method
            double x = 100;
            double y = 4;

            var computeController = A.Fake<ComputeController>();

            var result = computeController.Div(x, y);

            Assert.True(result.Value.Equals(25));
        }


    }
}
