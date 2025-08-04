using System;
using System.Collections.Generic;
using System.Net;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Models.MetaModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Attributes
{
    public class ApiRequestValidationAttributeTests
    {
        private readonly Fixture _fixture;
        private readonly ApiRequestValidationAttribute _target;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly ActionExecutingContext _actionExecutingContext;

        public ApiRequestValidationAttributeTests()
        {
            _fixture = new Fixture();
            _target = new ApiRequestValidationAttribute();
            _mockHttpContext = new Mock<HttpContext>();
            
            var actionContext = new ActionContext(
                _mockHttpContext.Object,
                new RouteData(),
                new ActionDescriptor()
            );

            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object
            );
        }

        [Fact]
        public void ResponseContent_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var expectedContent = _fixture.Create<string>();

            // Act
            _target.ResponseContent = expectedContent;

            // Assert
            Assert.Equal(expectedContent, _target.ResponseContent);
        }

        [Fact]
        public void OnActionExecuting_WithValidModel_DoesNotSetResult()
        {
            // Arrange
            var validModel = new Mock<IValidationMetaModel>();
            validModel.Setup(x => x.IsValid).Returns(true);
            
            _actionExecutingContext.ActionArguments.Add("model", validModel.Object);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithInvalidModel_SetsBadResult()
        {
            // Arrange
            var invalidModel = new Mock<IValidationMetaModel>();
            invalidModel.Setup(x => x.IsValid).Returns(false);
            
            _actionExecutingContext.ActionArguments.Add("model", invalidModel.Object);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.NotNull(_actionExecutingContext.Result);
            Assert.Equal(RequestResultConst.BadResult, _actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithInvalidModelAndCustomResponseContent_SetsCustomResult()
        {
            // Arrange
            var customContent = _fixture.Create<string>();
            _target.ResponseContent = customContent;
            
            var invalidModel = new Mock<IValidationMetaModel>();
            invalidModel.Setup(x => x.IsValid).Returns(false);
            
            _actionExecutingContext.ActionArguments.Add("model", invalidModel.Object);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.NotNull(_actionExecutingContext.Result);
            var contentResult = Assert.IsType<ContentResult>(_actionExecutingContext.Result);
            Assert.Equal((int)HttpStatusCode.OK, contentResult.StatusCode);
            Assert.Equal(customContent, contentResult.Content);
        }

        [Fact]
        public void OnActionExecuting_WithNullModel_DoesNotSetResult()
        {
            // Arrange
            _actionExecutingContext.ActionArguments.Add("model", null);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithNonValidationModel_DoesNotSetResult()
        {
            // Arrange
            var nonValidationModel = _fixture.Create<string>();
            _actionExecutingContext.ActionArguments.Add("model", nonValidationModel);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithEmptyActionArguments_DoesNotSetResult()
        {
            // Arrange
            // ActionArguments is already empty

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithMultipleArguments_OnlyChecksValidationModel()
        {
            // Arrange
            var validModel = new Mock<IValidationMetaModel>();
            validModel.Setup(x => x.IsValid).Returns(true);
            
            _actionExecutingContext.ActionArguments.Add("otherParam", _fixture.Create<string>());
            _actionExecutingContext.ActionArguments.Add("model", validModel.Object);
            _actionExecutingContext.ActionArguments.Add("anotherParam", _fixture.Create<int>());

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.Null(_actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_WithEmptyResponseContent_SetsBadResult()
        {
            // Arrange
            _target.ResponseContent = string.Empty;
            
            var invalidModel = new Mock<IValidationMetaModel>();
            invalidModel.Setup(x => x.IsValid).Returns(false);
            
            _actionExecutingContext.ActionArguments.Add("model", invalidModel.Object);

            // Act
            _target.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.NotNull(_actionExecutingContext.Result);
            Assert.Equal(RequestResultConst.BadResult, _actionExecutingContext.Result);
        }
    }
}
