namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Attributes
{
    using AutoFixture;
    using Common.Attributes;
    using Xunit;

    public class TranslationAttribute_Test
    {
        private readonly Fixture _fixture;

        public TranslationAttribute_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Contructor_PassAttributeValue_CorrectValue()
        {
            //Arrange
            var propertyId = _fixture.Create<string>();
            var translationType = _fixture.Create<string>();

            //Act
            var attribute = new TranslationAttribute(propertyId, translationType);

            //Assert
            Assert.Equal(propertyId, attribute.PropertyId);
            Assert.Equal(translationType, attribute.TranslationType);
        }
    }
}