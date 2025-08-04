using System.Net;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Constants
{
    public class RequestResultConstTests
    {
        [Fact]
        public void BadResult_ReturnsCorrectJsonResult()
        {
            // Act
            var result = RequestResultConst.BadResult;

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, jsonResult.StatusCode);

            // Verify the anonymous object structure
            var value = jsonResult.Value;
            Assert.NotNull(value);

            var msgProperty = value.GetType().GetProperty("msg");
            var isSuccessProperty = value.GetType().GetProperty("isSuccess");

            Assert.NotNull(msgProperty);
            Assert.NotNull(isSuccessProperty);

            Assert.Equal("400 Bad Request", msgProperty.GetValue(value));
            Assert.False((bool)isSuccessProperty.GetValue(value));
        }

        [Fact]
        public void ForbiddenResult_ReturnsCorrectJsonResult()
        {
            // Act
            var result = RequestResultConst.ForbiddenResult;

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal((int)HttpStatusCode.Forbidden, jsonResult.StatusCode);

            // Verify the anonymous object structure
            var value = jsonResult.Value;
            Assert.NotNull(value);

            var msgProperty = value.GetType().GetProperty("msg");
            var isSuccessProperty = value.GetType().GetProperty("isSuccess");

            Assert.NotNull(msgProperty);
            Assert.NotNull(isSuccessProperty);

            Assert.Equal("403 Forbidden", msgProperty.GetValue(value));
            Assert.False((bool)isSuccessProperty.GetValue(value));
        }

        [Fact]
        public void BadResult_IsSameInstanceOnMultipleCalls()
        {
            // Act
            var result1 = RequestResultConst.BadResult;
            var result2 = RequestResultConst.BadResult;

            // Assert
            Assert.Same(result1, result2);
        }

        [Fact]
        public void ForbiddenResult_IsSameInstanceOnMultipleCalls()
        {
            // Act
            var result1 = RequestResultConst.ForbiddenResult;
            var result2 = RequestResultConst.ForbiddenResult;

            // Assert
            Assert.Same(result1, result2);
        }

        [Fact]
        public void BadResult_And_ForbiddenResult_AreDifferentInstances()
        {
            // Act
            var badResult = RequestResultConst.BadResult;
            var forbiddenResult = RequestResultConst.ForbiddenResult;

            // Assert
            Assert.NotSame(badResult, forbiddenResult);
        }

        [Fact]
        public void BadResult_HasCorrectStatusCode()
        {
            // Act
            var result = RequestResultConst.BadResult;

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(400, jsonResult.StatusCode);
        }

        [Fact]
        public void ForbiddenResult_HasCorrectStatusCode()
        {
            // Act
            var result = RequestResultConst.ForbiddenResult;

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(403, jsonResult.StatusCode);
        }
    }
}
