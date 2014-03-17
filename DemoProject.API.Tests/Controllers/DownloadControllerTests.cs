using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DemoProject.API.ActionResults;
using DemoProject.API.Controllers;
using DemoProject.API.Repositories;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace DemoProject.API.Tests.Controllers
{
    [TestClass]
    public class DownloadControllerTests
    {
        private const int TestId = 1;

        private readonly Guid testUid = Guid.NewGuid();

        [TestMethod]
        public void Should_Return_Internal_Server_Error_If_File_Not_Found()
        {
            // Arrange
            Stream stream = null;
            Mock<IMetadataRepository> metadataRepositoryMock;
            Mock<IStorageRepository> storageRepositoryMock;
            DownloadController controller = this.PrepareController(stream, out metadataRepositoryMock, out storageRepositoryMock);

            // Act
            var actionResult = controller.Get(default(int));

            // Assert
            System.Web.Http.Results.NotFoundResult result = actionResult as System.Web.Http.Results.NotFoundResult;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
            metadataRepositoryMock.VerifyAll();
            storageRepositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), out stream), Times.Never, "If ID was not found, Storage should not be accessed");
        }

        [TestMethod]
        public void Should_Return_File_As_Attachment()
        {
            // Arrange
            Stream stream = new MemoryStream();
            stream.WriteByte(42);
            Mock<IMetadataRepository> metadataRepositoryMock;
            Mock<IStorageRepository> storageRepositoryMock;
            DownloadController controller = this.PrepareController(stream, out metadataRepositoryMock, out storageRepositoryMock);

            // Act
            var actionResult = controller.Get(TestId);

            // Assert
            //TODO: check content 
            Attachment result = actionResult as Attachment;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
            result.Stream.Should().NotBeNull();
            metadataRepositoryMock.VerifyAll();
            storageRepositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), out stream), Times.Once, "Storage should be accessed only once");
        }

        private DownloadController PrepareController(Stream stream, out Mock<IMetadataRepository> metadataRepositoryMock, out Mock<IStorageRepository> storageRepositoryMock)
        {
            var queryableList = new List<Model.Metadata>
            {
                new Model.Metadata { Id = TestId, FileStorageUid = this.testUid },
                new Model.Metadata { Id = 2 }
            }.AsQueryable();

            DownloadController controller = new DownloadController();
            metadataRepositoryMock = new Mock<IMetadataRepository>();
            metadataRepositoryMock.Setup(x => x.GetAll()).Returns(queryableList).Verifiable();
            storageRepositoryMock = new Mock<IStorageRepository>();
            storageRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), out stream)).Returns(true);
            controller.MetadataRepository = metadataRepositoryMock.Object;
            controller.StorageRepository = storageRepositoryMock.Object;
            controller.Request = new System.Net.Http.HttpRequestMessage();

            return controller;
        }
    }
}
