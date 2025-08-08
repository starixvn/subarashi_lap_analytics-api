namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Attributes
{
    using System;
    using AutoFixture;
    using Core.Attributes;
    using Xunit;

    public class BearerAuthorizeAttributeTests
    {
        private readonly Fixture _fixture;

        public BearerAuthorizeAttributeTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithNoRoles_SetsAuthenticationSchemeOnly()
        {
            // Act
            var attribute = new BearerAuthorizeAttribute();

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }

        [Fact]
        public void Constructor_WithEmptyRoles_SetsAuthenticationSchemeOnly()
        {
            // Act
            var attribute = new BearerAuthorizeAttribute();

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }

        [Fact]
        public void Constructor_WithSingleRole_SetsRolesAndAuthenticationScheme()
        {
            // Arrange
            var role = _fixture.Create<string>();

            // Act
            var attribute = new BearerAuthorizeAttribute(role);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal(role, attribute.Roles);
        }

        [Fact]
        public void Constructor_WithMultipleRoles_SetsCommaSeparatedRoles()
        {
            // Arrange
            var role1 = _fixture.Create<string>();
            var role2 = _fixture.Create<string>();
            var role3 = _fixture.Create<string>();
            var expectedRoles = $"{role1},{role2},{role3}";

            // Act
            var attribute = new BearerAuthorizeAttribute(role1, role2, role3);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal(expectedRoles, attribute.Roles);
        }

        [Fact]
        public void Constructor_WithNullRoles_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BearerAuthorizeAttribute(null));
        }

        [Fact]
        public void Constructor_WithMixedRoles_IncludingNulls_SetsOnlyNonNullRoles()
        {
            // Arrange
            var role1 = _fixture.Create<string>();
            var role2 = _fixture.Create<string>();

            // Act
            var attribute = new BearerAuthorizeAttribute(role1, null, role2);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal($"{role1},,{role2}", attribute.Roles);
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("user")]
        [InlineData("super_admin")]
        public void Constructor_WithSpecificRole_SetsCorrectRole(string role)
        {
            // Act
            var attribute = new BearerAuthorizeAttribute(role);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal(role, attribute.Roles);
        }

        [Fact]
        public void Constructor_WithEmptyStringRole_SetsEmptyRole()
        {
            // Act
            var attribute = new BearerAuthorizeAttribute(string.Empty);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal(string.Empty, attribute.Roles);
        }

        [Fact]
        public void Constructor_WithWhitespaceRole_SetsWhitespaceRole()
        {
            // Arrange
            var whitespaceRole = "   ";

            // Act
            var attribute = new BearerAuthorizeAttribute(whitespaceRole);

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal(whitespaceRole, attribute.Roles);
        }
    }
}
