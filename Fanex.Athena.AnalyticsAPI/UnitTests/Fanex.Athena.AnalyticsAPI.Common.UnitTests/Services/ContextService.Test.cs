using System.Linq;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class ContextService_Test
    {
        private readonly ContextService _target;

        public ContextService_Test()
        {
            _target = new ContextService();
        }

        [Fact]
        public void AddError_WithKeyAndMessage_AddsErrorToDictionary()
        {
            // Arrange
            var key = "test_key";
            var message = "test error message";

            // Act
            _target.AddError(key, message);

            // Assert
            Assert.NotNull(_target.Errors);
            Assert.Single(_target.Errors);
            Assert.True(_target.Errors.ContainsKey(key));
            Assert.Equal(message, _target.Errors[key]);
        }

        [Fact]
        public void AddError_WithMessageOnly_AddsErrorWithGeneratedKey()
        {
            // Arrange
            var message = "test error message";

            // Act
            _target.AddError(message);

            // Assert
            Assert.NotNull(_target.Errors);
            Assert.Single(_target.Errors);
            Assert.Equal(message, _target.Errors.Values.First());
            Assert.NotEmpty(_target.Errors.Keys.First()); // Generated GUID key should not be empty
        }

        [Fact]
        public void AddError_MultipleErrors_AddsAllErrors()
        {
            // Arrange
            var key1 = "error1";
            var message1 = "first error";
            var message2 = "second error";

            // Act
            _target.AddError(key1, message1);
            _target.AddError(message2);

            // Assert
            Assert.NotNull(_target.Errors);
            Assert.Equal(2, _target.Errors.Count);
            Assert.True(_target.Errors.ContainsKey(key1));
            Assert.Equal(message1, _target.Errors[key1]);
            Assert.Contains(message2, _target.Errors.Values);
        }

        [Fact]
        public void AddInfo_WithKeyAndMessage_AddsInfoToDictionary()
        {
            // Arrange
            var key = "info_key";
            var message = "test info message";

            // Act
            _target.AddInfo(key, message);

            // Assert
            Assert.NotNull(_target.Info);
            Assert.Single(_target.Info);
            Assert.True(_target.Info.ContainsKey(key));
            Assert.Equal(message, _target.Info[key]);
        }

        [Fact]
        public void AddInfo_WithMessageOnly_AddsInfoWithGeneratedKey()
        {
            // Arrange
            var message = "test info message";

            // Act
            _target.AddInfo(message);

            // Assert
            Assert.NotNull(_target.Info);
            Assert.Single(_target.Info);
            Assert.Equal(message, _target.Info.Values.First());
            Assert.NotEmpty(_target.Info.Keys.First()); // Generated GUID key should not be empty
        }

        [Fact]
        public void AddInfo_MultipleInfos_AddsAllInfos()
        {
            // Arrange
            var key1 = "info1";
            var message1 = "first info";
            var message2 = "second info";

            // Act
            _target.AddInfo(key1, message1);
            _target.AddInfo(message2);

            // Assert
            Assert.NotNull(_target.Info);
            Assert.Equal(2, _target.Info.Count);
            Assert.True(_target.Info.ContainsKey(key1));
            Assert.Equal(message1, _target.Info[key1]);
            Assert.Contains(message2, _target.Info.Values);
        }

        [Fact]
        public void Message_WithErrors_ReturnsJoinedErrorMessages()
        {
            // Arrange
            _target.AddError("error1", "First error");
            _target.AddError("error2", "Second error");

            // Act
            var result = _target.Message;

            // Assert
            Assert.Contains("First error", result);
            Assert.Contains("Second error", result);
            Assert.Contains(", ", result); // Should be joined with comma and space
        }

        [Fact]
        public void Message_WithInfoOnly_ReturnsJoinedInfoMessages()
        {
            // Arrange
            _target.AddInfo("info1", "First info");
            _target.AddInfo("info2", "Second info");

            // Act
            var result = _target.Message;

            // Assert
            Assert.Contains("First info", result);
            Assert.Contains("Second info", result);
            Assert.Contains(", ", result); // Should be joined with comma and space
        }

        [Fact]
        public void Message_WithBothErrorsAndInfo_ReturnsErrorsOnly()
        {
            // Arrange
            _target.AddError("error1", "Error message");
            _target.AddInfo("info1", "Info message");

            // Act
            var result = _target.Message;

            // Assert
            Assert.Contains("Error message", result);
            Assert.DoesNotContain("Info message", result); // Errors take precedence
        }

        [Fact]
        public void Message_WithNoErrorsOrInfo_ReturnsEmptyString()
        {
            // Act
            var result = _target.Message;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Errors_InitiallyNull_BecomesNotNullAfterAddingError()
        {
            // Assert initial state
            Assert.Null(_target.Errors);

            // Act
            _target.AddError("test", "message");

            // Assert
            Assert.NotNull(_target.Errors);
        }

        [Fact]
        public void Info_InitiallyNull_BecomesNotNullAfterAddingInfo()
        {
            // Assert initial state
            Assert.Null(_target.Info);

            // Act
            _target.AddInfo("test", "message");

            // Assert
            Assert.NotNull(_target.Info);
        }
    }
}
