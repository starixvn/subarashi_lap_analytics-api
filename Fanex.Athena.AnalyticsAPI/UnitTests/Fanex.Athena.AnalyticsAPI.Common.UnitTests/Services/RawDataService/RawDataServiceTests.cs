namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Services;
    using Moq;
    using Xunit;

    public class RawDataServiceTests
    {
        private readonly RawDataService _rawDataService;

        public RawDataServiceTests()
        {
            _rawDataService = new RawDataService();
        }

        [Fact]
        public void HandleRawData_WithListOfLists_WhenListDataIsNull_DoesNothing()
        {
            // Arrange
            IEnumerable<IEnumerable<string>> listData = null;
            var mockHandler = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler.Verify(x => x.HandleData(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleRawData_WithListOfLists_WhenListDataIsEmpty_DoesNothing()
        {
            // Arrange
            var listData = new List<IEnumerable<string>>();
            var mockHandler = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler.Verify(x => x.HandleData(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleRawData_WithListOfLists_CallsHandlerForEachItem()
        {
            // Arrange
            var listData = new List<List<string>>
            {
                new List<string> { "a", "b" },
                new List<string> { "c" }
            };
            var mockHandler1 = new Mock<IHandleData>();
            var mockHandler2 = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler1.Object, mockHandler2.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler1.Verify(x => x.HandleData(listData[0]), Times.Once);
            mockHandler1.Verify(x => x.HandleData(listData[1]), Times.Once);
            mockHandler2.Verify(x => x.HandleData(listData[0]), Times.Once);
            mockHandler2.Verify(x => x.HandleData(listData[1]), Times.Once);
        }

        [Fact]
        public void HandleRawData_WithSingleList_WhenListDataIsNull_DoesNothing()
        {
            // Arrange
            IEnumerable<string> listData = null;
            var mockHandler = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler.Verify(x => x.HandleData(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleRawData_WithSingleList_WhenListDataIsEmpty_DoesNothing()
        {
            // Arrange
            var listData = new List<string>();
            var mockHandler = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler.Verify(x => x.HandleData(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public void HandleRawData_WithSingleList_WhenHandlersAreEmpty_DoesNothing()
        {
            // Arrange
            var listData = new List<string> { "a", "b", "c" };
            var handlers = Enumerable.Empty<IHandleData>();
            var mockHandler = new Mock<IHandleData>();

            // Act
             _rawDataService.HandleRawData(listData, handlers);

            // Assert
            // No exception should be thrown, and no handlers called (as there are none)
            mockHandler.Verify(x => x.HandleData(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleRawData_WithSingleList_CallsHandlerForEachItem()
        {
            // Arrange
            var listData = new List<string> { "a", "b", "c" };
            var mockHandler1 = new Mock<IHandleData>();
            var mockHandler2 = new Mock<IHandleData>();
            var handlers = new List<IHandleData> { mockHandler1.Object, mockHandler2.Object };

            // Act
            _rawDataService.HandleRawData(listData, handlers);

            // Assert
            mockHandler1.Verify(x => x.HandleData("a"), Times.Once);
            mockHandler1.Verify(x => x.HandleData("b"), Times.Once);
            mockHandler1.Verify(x => x.HandleData("c"), Times.Once);
            mockHandler2.Verify(x => x.HandleData("a"), Times.Once);
            mockHandler2.Verify(x => x.HandleData("b"), Times.Once);
            mockHandler2.Verify(x => x.HandleData("c"), Times.Once);
        }
    }
} 