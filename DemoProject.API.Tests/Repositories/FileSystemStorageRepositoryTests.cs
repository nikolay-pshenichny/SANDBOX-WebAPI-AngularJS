using System;
using System.IO;
using System.Linq;

using DemoProject.API.Configurations;
using DemoProject.API.Repositories;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace DemoProject.API.Tests.Repositories
{
    [TestClass]
    public class FileSystemStorageRepositoryTests
    {
        [TestMethod]
        public void Should_Put_Files_To_FileSystem_With_New_Name()
        {
            // Arrange
            var testContent = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Guid uid;
            var stream = new MemoryStream();
            stream.Write(testContent, 0, testContent.Count());
            stream.Position = 0;
            var repository = new FileSystemStorageRepository();
            var configurationMock = new Mock<IStorageConfiguration>();
            configurationMock.Setup(x => x.Path).Returns(Path.GetTempPath);
            repository.StorageConfiguration = configurationMock.Object;

            // Act
            repository.Put(stream, out uid);
            string fullFileName = Path.Combine(configurationMock.Object.Path, uid.ToString());

            // Assert
            File.Exists(fullFileName).Should().BeTrue();
            try
            {
                File.ReadAllBytes(fullFileName).Should().BeEquivalentTo(testContent);
            }
            finally
            {
                // Clean up
                File.Delete(fullFileName);
            }

            configurationMock.Verify(x => x.Path, Times.AtLeastOnce);
        }

        [TestMethod]
        public void Should_Get_Files_From_File_System_By_Uid()
        {
            // TODO
        }
    }
}
