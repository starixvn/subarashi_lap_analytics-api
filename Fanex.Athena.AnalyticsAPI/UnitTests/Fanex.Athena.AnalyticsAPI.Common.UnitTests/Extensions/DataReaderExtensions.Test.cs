using System;
using System.Data;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Extensions
{
    public class DataReaderExtensions_Test
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDataReader> _readerMock;

        public DataReaderExtensions_Test()
        {
            _fixture = new Fixture();
            _readerMock = new Mock<IDataReader>();
        }

        [Fact]
        public void HasColumn_ReaderContainColumn_ReturnTrue()
        {
            // Arrange
            var columnName = _fixture.Create<string>();

            _readerMock.Setup(_ => _.FieldCount).Returns(1);
            _readerMock.Setup(_ => _.GetName(0)).Returns(columnName);

            // Action
            var result = _readerMock.Object.HasColumn(columnName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasColumn_ReaderNotContainColumn_ReturnFalse()
        {
            // Arrange
            var columnName = _fixture.Create<string>();
            _readerMock.Setup(_ => _.FieldCount).Returns(1);
            _readerMock.Setup(_ => _.GetName(0)).Returns("OtherColumnTest");

            // Action
            var result = _readerMock.Object.HasColumn(columnName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ReadObject_ReaderNotContainColumn_ReturnDefault()
        {
            // Arrange
            var expected = new DTOClassTest();

            // Action
            var result = _readerMock.Object.ReadObject<DTOClassTest>();

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void ReadObject_WhenCall_ReturnCorrectValue()
        {
            // Arrange
            var expected = _fixture.Build<DTOClassTest>().Without(x => x.Prop4).Create();
            var props = expected.GetType().GetProperties();
            _readerMock.Setup(_ => _.FieldCount).Returns(props.Length);
            for (int i = 0; i < props.Length; i++)
            {
                var value = props[i].GetValue(expected);
                var attributeName = Attribute.GetCustomAttribute(props[i], typeof(SsasColumnAttribute)) as SsasColumnAttribute;

                var isValid = attributeName != null && !string.IsNullOrWhiteSpace(attributeName.MapColumn);
                if (isValid)
                {
                    _readerMock.Setup(_ => _[attributeName.MapColumn]).Returns(value);
                    _readerMock.Setup(_ => _.GetName(i)).Returns(attributeName.MapColumn);
                }
                else
                {
                    _readerMock.Setup(_ => _[props[i].Name]).Returns(value);
                    _readerMock.Setup(_ => _.GetName(i)).Returns(props[i].Name);
                }
            }

            // Action
            var result = _readerMock.Object.ReadObject<DTOClassTest>();

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        private class DTOClassTest
        {
            [SsasColumn("prop1")]
            public int Prop1 { get; set; }

            [SsasColumn("prop2")]
            public string Prop2 { get; set; }

            [SsasColumn("prop3")]
            public string Prop3 { get; set; }

            public string Prop4 { get; set; } = "prop4";
        }
    }
}