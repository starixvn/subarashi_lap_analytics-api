using AutoFixture;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using Fanex.Athena.LGAPI.SSAS.Bridge.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.UnitTest.Extensions
{
    public class DtoTestClass
    {
        [SsasColumn("prop1")]
        public int Prop1 { get; set; }

        [SsasColumn("prop2")]
        public string Prop2 { get; set; }

        [SsasColumn("prop3")]
        public string Prop3 { get; set; }

        public string Prop4 { get; set; } = "prop4";
    }

    public class MappingExtensions_ToObject_Test
    {
        private readonly Fixture _fixture;

        public MappingExtensions_ToObject_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ToObject_SoureIsNull_ReturnNull()
        {
            //Arrange

            //Act
            var result = MappingExtensions.ToObject<DtoTestClass>(null);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToObject_WhenCall_ReturnCorrectObject()
        {
            //Arrange
            var source = new Dictionary<string, object>
            {
                { "prop1", _fixture.Create<int>()  },
                { "prop2", _fixture.Create<string>() },
                { "prop3",_fixture.Create<string>() }
            };
            //Act
            var result = MappingExtensions.ToObject<DtoTestClass>(source);

            //Assert
            Assert.Equal(source["prop1"], result.Prop1);
            Assert.Equal(source["prop2"], result.Prop2);
            Assert.Equal(source["prop3"], result.Prop3);
        }

        [Fact]
        public void ToObject_PropertyNoMatch_ValueOfPropertyIsDefault()
        {
            //Arrange
            var source = new Dictionary<string, object>
            {
                { "prop2", _fixture.Create<string>() },
            };
            //Act
            var result = MappingExtensions.ToObject<DtoTestClass>(source);

            //Assert
            Assert.Equal(default(int), result.Prop1);
            Assert.Equal(source["prop2"], result.Prop2);
            Assert.Equal(default(string), result.Prop3);
        }

        [Fact]
        public void ToObject_PropertyNoAttribute_ValueNoChange()
        {
            //Arrange
            var source = new Dictionary<string, object>();
            //Act
            var result = MappingExtensions.ToObject<DtoTestClass>(source);

            //Assert
            Assert.Equal("prop4", result.Prop4);
        }
    }
}