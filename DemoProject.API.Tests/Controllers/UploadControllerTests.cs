using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using DemoProject.API.Calculators;
using DemoProject.API.Controllers;
using DemoProject.API.Processors;
using DemoProject.API.Repositories;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace DemoProject.API.Tests.Controllers
{
    [TestClass]
    public class UploadControllerTests
    {
        private const int ExpectedId = 42;

        private Guid storageUid = Guid.NewGuid();
        private Guid fileProcessingResult = Guid.NewGuid();
        private Guid checksumCalculationResult = Guid.NewGuid();

        [TestMethod]
        public void Should_Expect_Only_Multipart_Content()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);
            controller.Request.Content = new StringContent(string.Empty);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            task.Result.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [TestMethod]
        public void Should_Put_File_ToStorage()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            storageRepositoryMock.Verify(x => x.Put(It.IsAny<Stream>(), out this.storageUid), Times.Once, "Storage should be accessed only once");
        }

        [TestMethod]
        public void Should_Process_Received_File_Using_File_Processor()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            fileProcessorMock.Verify(x => x.Process(It.IsAny<Stream>()), Times.Once, "File processing should be called exactly once");
        }

        [TestMethod]
        public void Should_Calculate_File_CheckSum()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            checksumCalculatorMock.Verify(x => x.Calculate(It.IsAny<Stream>()), Times.Once, "Checksum calculation should be called exactly once");
        }

        [TestMethod]
        public void Should_Save_Metadata_To_Repository()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            metadataRepositoryMock.Verify(x => x.Save(It.IsAny<Model.Metadata>()), Times.Once);
        }

        [TestMethod]
        public void Should_Return_Results_With_Information_About_Processing()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);

            // Act
            Task<HttpResponseMessage> task = controller.Post();
            task.Wait();

            // Assert
            task.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            var t = task.Result.Content.As<ObjectContent<List<Models.MetadataInfo>>>();
            t.Value.Should().BeOfType<List<Models.MetadataInfo>>();
            var informationFromService = (t.Value as List<Models.MetadataInfo>).First();
            informationFromService.Id.Should().Be(ExpectedId);
            informationFromService.ProcessingResult.Should().Be(this.fileProcessingResult.ToString());
        }

        private UploadController ConfigureController(
            out Mock<IStorageRepository> storageRepositoryMock,
            out Mock<IFileProcessor> fileProcessorMock,
            out Mock<IChecksumCalculator> checksumCalculatorMock,
            out Mock<IMetadataRepository> metadataRepositoryMock)
        {
            var controller = new UploadController();
            storageRepositoryMock = new Mock<IStorageRepository>();
            storageRepositoryMock.Setup(x => x.Put(It.IsAny<Stream>(), out this.storageUid)).Verifiable();

            fileProcessorMock = new Mock<IFileProcessor>();
            fileProcessorMock.Setup(x => x.Process(It.IsAny<Stream>())).Returns(this.fileProcessingResult.ToString()).Verifiable();

            checksumCalculatorMock = new Mock<IChecksumCalculator>();
            checksumCalculatorMock.Setup(x => x.Calculate(It.IsAny<Stream>())).Returns(this.checksumCalculationResult.ToString()).Verifiable();

            metadataRepositoryMock = new Mock<IMetadataRepository>();
            metadataRepositoryMock.Setup(x => x.Save(It.IsAny<Model.Metadata>()))
                .Returns(
                    new Model.Metadata
                        {
                            Id = ExpectedId,
                            FileStorageUid = this.storageUid,
                            ProcessingResult = this.fileProcessingResult.ToString(),
                            Checksum = this.checksumCalculationResult.ToString()
                        })
                .Verifiable();

            controller.StorageRepository = storageRepositoryMock.Object;
            controller.FileProcessor = fileProcessorMock.Object;
            controller.MetadataRepository = metadataRepositoryMock.Object;
            controller.ChecksumCalculator = checksumCalculatorMock.Object;

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost");
            request.SetConfiguration(new System.Web.Http.HttpConfiguration());
            controller.Request = request;

            var multipartContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.Text.Encoding.ASCII.GetBytes("line1\nline2 is long\nline3\n"));
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "Test.txt" };
            multipartContent.Add(fileContent);
            request.Content = multipartContent;

            return controller;
        }
    }
}
