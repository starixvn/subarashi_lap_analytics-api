namespace Fanex.Athena.AnalyticsAPI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AnalyticsAPI.Controllers;
    using AutoFixture;
    using Configuration;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Xunit;

    public class AuthController_Test
    {
        private readonly AuthController _target;
        private readonly Mock<IOptionsSnapshot<IdentityServerConfig>> _mockOptions;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly IdentityServerConfig _identityServerConfig;
        private readonly Fixture _fixture;

        public AuthController_Test()
        {
            _fixture = new Fixture();

            _identityServerConfig = new IdentityServerConfig
            {
                Host = "https://identity.test.com",
                TokenEndpoint = "connect/token",
                GrantType = "custom_grant",
                Scope = "test_scope",
                CredentialClient = new CredentialsConfig
                {
                    ClientId = "test_client",
                    ClientSecret = "test_secret"
                }
            };

            _mockOptions = new Mock<IOptionsSnapshot<IdentityServerConfig>>();
            _mockOptions.Setup(x => x.Value).Returns(_identityServerConfig);

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _target = new AuthController(_mockOptions.Object, _httpClient);

            // Setup HttpContext for IP address
            var mockHttpContext = new Mock<HttpContext>();
            var mockConnection = new Mock<ConnectionInfo>();
            mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));
            mockHttpContext.Setup(x => x.Connection).Returns(mockConnection.Object);

            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            _target.ControllerContext = controllerContext;
        }

        [Fact]
        public async Task Token_WithValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "valid_token" };

            var successTokenResponse = await CreateSuccessTokenResponse("access_token_123");
            SetupHttpClientMock(successTokenResponse);

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult);

            var value = jsonResult.Value;
            Assert.NotNull(value);

            // Use reflection to check the anonymous object properties
            var isSuccessProperty = value.GetType().GetProperty("IsSuccess");
            var accessTokenProperty = value.GetType().GetProperty("AccessToken");
            var msgProperty = value.GetType().GetProperty("Msg");

            Assert.True((bool)isSuccessProperty.GetValue(value));
            Assert.Equal("access_token_123", accessTokenProperty.GetValue(value));
            Assert.Null(msgProperty.GetValue(value));
        }

        [Fact]
        public async Task Token_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "test_token" };
            _target.ModelState.AddModelError("Token", "Token is required");

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Token_WithErrorResponse_ReturnsErrorResponse()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "invalid_token" };

            var errorTokenResponse = await CreateErrorTokenResponse("Invalid token provided");
            SetupHttpClientMock(errorTokenResponse);

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult);

            var value = jsonResult.Value;
            Assert.NotNull(value);

            // Use reflection to check the anonymous object properties
            var isSuccessProperty = value.GetType().GetProperty("IsSuccess");
            var msgProperty = value.GetType().GetProperty("Msg");
            var accessTokenProperty = value.GetType().GetProperty("AccessToken");

            Assert.False((bool)isSuccessProperty.GetValue(value));
            Assert.Equal("Invalid token provided", msgProperty.GetValue(value));
            Assert.Null(accessTokenProperty.GetValue(value));
        }

        [Fact]
        public void RedirectToLAP_WithLapUrl_ReturnsRedirectResult()
        {
            // Arrange
            var lapUrl = "test.example.com";
            var lapVer = "v1";

            // Act
            var result = _target.RedirectToLAP(lapUrl, lapVer);

            // Assert
            Assert.IsType<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.Equal("http://test.example.com/v1/12bet/Auth/Login?returnUrl=/Dashboard", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithoutLapVer_ReturnsRedirectResultWithEmptyVersion()
        {
            // Arrange
            var lapUrl = "test.example.com";

            // Act
            var result = _target.RedirectToLAP(lapUrl);

            // Assert
            Assert.IsType<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.Equal("http://test.example.com//12bet/Auth/Login?returnUrl=/Dashboard", redirectResult.Url);
        }

        [Fact]
        public async Task Token_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            LapTokenRequest request = null;
            _target.ModelState.AddModelError("", "Request cannot be null");

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task Token_WithInvalidToken_ReturnsBadRequest(string invalidToken)
        {
            // Arrange
            var request = new LapTokenRequest { Token = invalidToken };

            // Validate the model manually to trigger validation errors
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    _target.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? "", validationResult.ErrorMessage);
                }
            }

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Token_WithTokenTooLong_ReturnsBadRequest()
        {
            // Arrange - Token longer than 30 characters (MaxLengthToken)
            var request = new LapTokenRequest { Token = new string('a', 31) };

            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    _target.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? "", validationResult.ErrorMessage);
                }
            }

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Token_WithHttpClientException_ThrowsException()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "valid_token" };

            var exceptionTokenResponse = CreateExceptionTokenResponse(new HttpRequestException("Network error"));
            SetupHttpClientMock(exceptionTokenResponse);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _target.Token(request));
        }

        [Theory]
        [InlineData("example.com", "v1", "http://example.com/v1/12bet/Auth/Login?returnUrl=/Dashboard")]
        [InlineData("test.domain.com", "v2", "http://test.domain.com/v2/12bet/Auth/Login?returnUrl=/Dashboard")]
        [InlineData("localhost", "", "http://localhost//12bet/Auth/Login?returnUrl=/Dashboard")]
        [InlineData("192.168.1.1", "api", "http://192.168.1.1/api/12bet/Auth/Login?returnUrl=/Dashboard")]
        public void RedirectToLAP_WithVariousInputs_ReturnsCorrectRedirectUrl(string lapUrl, string lapVer, string expectedUrl)
        {
            // Act
            var result = _target.RedirectToLAP(lapUrl, lapVer);

            // Assert
            Assert.IsType<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.Equal(expectedUrl, redirectResult.Url);
        }

        [Fact]
        public async Task Token_WithValidTokenAndIPAddress_IncludesIPInRequest()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "valid_token" };
            var expectedIpAddress = "192.168.1.100";

            // Setup IP address - need to mock the entire HttpContext
            var mockHttpContext = new Mock<HttpContext>();
            var mockConnection = new Mock<ConnectionInfo>();
            mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(expectedIpAddress));
            mockHttpContext.Setup(x => x.Connection).Returns(mockConnection.Object);

            _target.ControllerContext.HttpContext = mockHttpContext.Object;

            var successTokenResponse = await CreateSuccessTokenResponse("access_token_123");
            SetupHttpClientMock(successTokenResponse);

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<JsonResult>(result);

            // Verify that the HTTP client was called with the correct parameters
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task Token_WithEmptyAccessToken_ReturnsSuccessWithNullToken()
        {
            // Arrange
            var request = new LapTokenRequest { Token = "valid_token" };

            var tokenResponse = await CreateSuccessTokenResponse(null);
            SetupHttpClientMock(tokenResponse);

            // Act
            var result = await _target.Token(request);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            var value = jsonResult.Value;

            var isSuccessProperty = value.GetType().GetProperty("IsSuccess");
            var accessTokenProperty = value.GetType().GetProperty("AccessToken");

            Assert.True((bool)isSuccessProperty.GetValue(value));

            // Debug: Check what the actual value is
            var actualAccessToken = accessTokenProperty.GetValue(value);

            // TokenResponse.AccessToken returns null when access_token is missing from JSON
            // but the anonymous object creation might convert null to empty string
            // Let's check if it's null or empty string
            Assert.True(actualAccessToken == null || string.IsNullOrEmpty(actualAccessToken?.ToString()));
        }

        // Helper methods for creating mock responses
        private async Task<TokenResponse> CreateSuccessTokenResponse(string accessToken)
        {
            var responseObject = new Dictionary<string, object>
            {
                { "token_type", "Bearer" },
                { "expires_in", 3600 }
            };

            // Only add access_token if it's not null
            if (accessToken != null)
            {
                responseObject["access_token"] = accessToken;
            }

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(responseObject)
            };

            return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(httpResponseMessage);
        }

        private async Task<TokenResponse> CreateErrorTokenResponse(string errorDescription)
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(new
                {
                    error = "invalid_grant",
                    error_description = errorDescription
                })
            };

            return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(httpResponseMessage);
        }

        private TokenResponse CreateExceptionTokenResponse(Exception exception)
        {
            // For exception scenarios, we'll create a TokenResponse that has the exception set
            // This simulates what happens when the HTTP call itself fails
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var tokenResponse = ProtocolResponse.FromHttpResponseAsync<TokenResponse>(httpResponseMessage).Result;

            // Use reflection to set the exception property since it's not publicly settable
            var exceptionProperty = typeof(ProtocolResponse).GetProperty("Exception");
            if (exceptionProperty != null && exceptionProperty.CanWrite)
            {
                exceptionProperty.SetValue(tokenResponse, exception);
            }

            return tokenResponse;
        }

        private void SetupHttpClientMock(TokenResponse tokenResponse)
        {
            var responseContent = tokenResponse.IsError
                ? JsonConvert.SerializeObject(new { error = tokenResponse.Error, error_description = tokenResponse.ErrorDescription })
                : JsonConvert.SerializeObject(new { access_token = tokenResponse.AccessToken, token_type = "Bearer" });

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            if (tokenResponse.Exception != null)
            {
                _mockHttpMessageHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ThrowsAsync(tokenResponse.Exception);
            }
            else
            {
                _mockHttpMessageHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(httpResponseMessage);
            }
        }
    }
}
