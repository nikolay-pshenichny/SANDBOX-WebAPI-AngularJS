using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using DemoProject.API.ActionResults;
using DemoProject.API.Calculators;
using DemoProject.API.Controllers;
using DemoProject.API.Models;
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            System.Web.Http.Results.StatusCodeResult result = task.Result as System.Web.Http.Results.StatusCodeResult;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
            result.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
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
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");
            
            var t = result.Value as IEnumerable<Models.MetadataInfo>;
            t.Should().NotBeNull("Wrong data type was returned as a result of controller's work");

            var informationFromService = t.First();
            informationFromService.Id.Should().Be(ExpectedId);
            informationFromService.ProcessingResult.Should().Be(this.fileProcessingResult.ToString());
        }

        [TestMethod]
        public void Should_Accept_Only_Text_Content()
        {
            // Arrange
            Mock<IStorageRepository> storageRepositoryMock;
            Mock<IFileProcessor> fileProcessorMock;
            Mock<IChecksumCalculator> checksumCalculatorMock;
            Mock<IMetadataRepository> metadataRepositoryMock;
            var controller = this.ConfigureController(out storageRepositoryMock, out fileProcessorMock, out checksumCalculatorMock, out metadataRepositoryMock);
            
            var multipartContent = new MultipartFormDataContent();
            var binaryContent = Enumerable.Repeat(Enumerable.Range(14, 255).ToArray(), 20).SelectMany(x => x.Select(y => y)).Select(x=>(byte)x).ToArray();
            var fileContent = new ByteArrayContent(binaryContent);
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "Test.txt" };
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("something/that_we_dont_expect");
            multipartContent.Add(fileContent);
            controller.Request.Content = multipartContent;

            // Act
            Task<IHttpActionResult> task = controller.Post();
            task.Wait();

            // Assert
            GenericValueResult<List<MetadataInfo>> result = task.Result as GenericValueResult<List<MetadataInfo>>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");

            var t = result.Value as IEnumerable<Models.MetadataInfo>;
            t.Should().NotBeNull("Wrong data type was returned as a result of controller's work");

            var informationFromService = t.First();
            informationFromService.Id.Should().NotHaveValue();
            informationFromService.ProcessingResult.Should().Be(UploadController.ContentTypeCannotBeAcceptedMessage);
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
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            multipartContent.Add(fileContent);
            request.Content = multipartContent;

            return controller;
        }
    }
}
