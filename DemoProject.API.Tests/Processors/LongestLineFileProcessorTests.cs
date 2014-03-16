using System.IO;

using DemoProject.API.Processors;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoProject.API.Tests.Processors
{
    [TestClass]
    public class LongestLineFileProcessorTests
    {
        [TestMethod]
        public void Should_Return_Longest_Line_Of_Text()
        {
            // Arrange
            const string FirstLine = "This is a line of text";
            const string SecondLine = "Line 2";
            const string ThirdLine = "This is the longest line of text";

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(FirstLine);
            writer.WriteLine(SecondLine);
            writer.WriteLine(ThirdLine);
            writer.Flush();
            stream.Position = 0;

            // Act
            LongestLineFileProcessor processor = new LongestLineFileProcessor();
            string result = processor.Process(stream);

            // Assert
            result.Should().Be(ThirdLine);
        }
    }
}
