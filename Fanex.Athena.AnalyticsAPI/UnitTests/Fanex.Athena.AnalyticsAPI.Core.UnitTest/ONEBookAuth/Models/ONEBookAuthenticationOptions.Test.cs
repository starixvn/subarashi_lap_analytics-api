using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
using Fanex.Athena.AnalyticsAPI.UnitTests.Utils.Helpers;
using LAP.ONEBook.Security;
using Moq;
using System;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.ONEBookAuth.Models
{
    public class ONEBookAuthenticationOptions_Test
    {
        private readonly ONEBookAuthenticationOptions _target;

        public ONEBookAuthenticationOptions_Test()
        {
            _target = new ONEBookAuthenticationOptions();
            FakeAuthClient();
            FakeBypass(false);
        }

        [Fact]
        public void Validate_AllowBypass_SuccessWithoutAnyException()
        {
            //Arrange
            FakeBypass(true);

            //Act and Assert
            var exception = Record.Exception(() => _target.Validate());
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_SecurityConnectionStringIsNullOrEmpty_ThrowExceptionWithMessage(string connectionString)
        {
            //Arrange
            _target.SecurityConnectionString = connectionString;

            //Act and Assert
            var exception = Assert.Throws<Exception>(() => _target.Validate());
            Assert.Equal("SecurityConnectionString is required", exception.Message);
        }

        [Fact]
        public void Validate_AuthClientIsNull_ThrowExceptionWithMessage()
        {
            //Arrange
            _target.SecurityConnectionString = FakeHelper.Create<string>();
            _target.AuthClient = null;

            //Act and Assert
            var exception = Assert.Throws<Exception>(() => _target.Validate());
            Assert.Equal("AuthClient is required", exception.Message);
        }

        [Fact]
        public void Validate_TokenRetrievalIsNull_ThrowExceptionWithMessage()
        {
            //Arrange
            _target.SecurityConnectionString = FakeHelper.Create<string>();
            _target.TokenRetrieval = null;

            //Act and Assert
            var exception = Assert.Throws<Exception>(() => _target.Validate());
            Assert.Equal("TokenRetrieval is required", exception.Message);
        }

        private void FakeAuthClient()
        {
            _target.AuthClient = new Lazy<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>(() => Mock.Of<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>());
        }

        private void FakeBypass(bool allowBypass)
        {
            _target.AllowBypass = allowBypass;
        }
    }
}