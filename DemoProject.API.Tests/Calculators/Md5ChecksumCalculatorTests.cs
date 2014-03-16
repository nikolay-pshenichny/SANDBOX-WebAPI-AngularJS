using System.IO;

using DemoProject.API.Calculators;
using DemoProject.Model.Enums;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoProject.API.Tests.Calculators
{
    [TestClass]
    public class Md5ChecksumCalculatorTests
    {
        [TestMethod]
        public void Should_Correctly_Calculate_Md5()
        {
            // Arrange
            MemoryStream data = new MemoryStream();
            data.Write(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 0, 10);
            data.Position = 0;
            var calculator = new Md5ChecksumCalculator();

            // Act
            var result = calculator.Calculate(data);

            // Assert
            calculator.ChecksumType.Should().Be(ChecksumType.Md5);
            result.Should().Be("C5-6B-D5-48-0F-6E-54-13-CB-62-A0-AD-96-66-61-3A");
        }
    }
}
