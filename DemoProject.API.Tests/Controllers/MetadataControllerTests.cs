using System;
using System.Collections.Generic;
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
    public class MetadataControllerTests
    {
        private const int ExpectedId = 42;
        private static readonly Guid ExpectedUid = Guid.NewGuid();
        private readonly Model.Metadata notExpected = new Model.Metadata { Id = default(int) };
        private readonly Model.Metadata expected = new Model.Metadata { Id = ExpectedId, FileStorageUid = ExpectedUid };

        [TestMethod]
        public void Should_Return_All_Metadata_From_Metadata_Repository_OnGet()
        {
            // Arrange
            var queryableList = this.ConstructQueryableList();
            MetadataController controller = new MetadataController();
            var metadataRepositoryMock = new Mock<IMetadataRepository>();
            metadataRepositoryMock.Setup(x => x.GetAll()).Returns(queryableList).Verifiable();
            controller.MetadataRepository = metadataRepositoryMock.Object;

            // Act
            var actionResult = controller.Get();

            // Assert
            GenericValueResult<IEnumerable<Models.MetadataInfo>> results = actionResult as GenericValueResult<IEnumerable<Models.MetadataInfo>>;
            results.Should().NotBeNull("Wrong data type was returned from the controller");
            results.Value.Count().Should().Be(queryableList.Count());

            HashSet<int> receivedIds = new HashSet<int>(results.Value.Select(x => x.Id).AsEnumerable());
            HashSet<int> expectedIds = new HashSet<int>(queryableList.Select(x => x.Id).AsEnumerable());
            expectedIds.Except(receivedIds).Count().Should().Be(0, "Result list should contain all the same IDs as the list in Repository");

            metadataRepositoryMock.VerifyAll();
        }
        
        [TestMethod]
        public void Should_Return_Single_Metadata_By_Its_Id()
        {
            // Arrange
            MetadataController controller = new MetadataController();
            var metadataRepositoryMock = new Mock<IMetadataRepository>();
            metadataRepositoryMock.Setup(x => x.GetAll()).Returns(this.ConstructQueryableList()).Verifiable();
            controller.MetadataRepository = metadataRepositoryMock.Object;

            // Act
            var actionResult = controller.Get(ExpectedId);

            // Assert
            GenericValueResult<Models.MetadataInfo> result = actionResult as GenericValueResult<Models.MetadataInfo>;
            result.Should().NotBeNull("Wrong data type was returned from the controller");

            result.Value.Id.Should().Be(ExpectedId);
            metadataRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Should_Delete_Metadata_From_Repository_And_File_From_Storage_OnDelete()
        {
            // Arrange
            var queryableList = this.ConstructQueryableList();
            MetadataController controller = new MetadataController();
            var metadataRepositoryMock = new Mock<IMetadataRepository>();
            metadataRepositoryMock.Setup(x => x.GetAll()).Returns(queryableList).Verifiable();
            metadataRepositoryMock.Setup(x => x.Delete(ExpectedId)).Verifiable();
            controller.MetadataRepository = metadataRepositoryMock.Object;
            var storageRepositoryMock = new Mock<IStorageRepository>();
            storageRepositoryMock.Setup(x => x.Delete(ExpectedUid)).Verifiable();
            controller.StorageRepository = storageRepositoryMock.Object;

            // Act
            controller.Delete(ExpectedId);

            // Assert
            metadataRepositoryMock.VerifyAll();
            storageRepositoryMock.VerifyAll();
        }

        private IQueryable<Model.Metadata> ConstructQueryableList()
        {
            var queryableList = new List<Model.Metadata> { this.notExpected, this.expected }.AsQueryable();
            queryableList.Select(x => x.Id).Count().Should().Be(queryableList.Select(x => x.Id).Distinct().Count(), "IDs should be unique");
            return queryableList;
        }
    }
}
