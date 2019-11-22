using AutoFixture;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using Xunit;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.UnitTest.Attributes
{
    public class SsasColumnAttribute_Test
    {
        private readonly Fixture _fixture;

        public SsasColumnAttribute_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Contructor_PassMapColumn_CorrectMapColumn()
        {
            //Arrange
            var mapColumn = _fixture.Create<string>();

            //Act
            var attribute = new SsasColumnAttribute(mapColumn);

            //Assert
            Assert.Equal(mapColumn, attribute.MapColumn);
        }

        [Fact]
        public void AssignToMapColumn_Assias_CorrectMapColumn()
        {
            //Arrange
            var mapColumn = _fixture.Create<string>();
            var attribute = new SsasColumnAttribute();

            //Act
            attribute.MapColumn = mapColumn;

            //Assert
            Assert.Equal(mapColumn, attribute.MapColumn);
        }
    }
}