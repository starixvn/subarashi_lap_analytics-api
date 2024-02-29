using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Extensions
{
    internal class Fake_GroupTo_Class
    {
        public int Key { get; set; }

        public int Value { get; set; }
    }

    public class CollectionExtension_GroupTo_Test
    {
        private readonly Fixture _fixture;

        public CollectionExtension_GroupTo_Test()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GroupTo_IsNull_ReturnEmpty()
        {
            //Arrange

            //Act
            var actual = CollectionExtension.GroupTo(null,
                _fixture.Create<Func<object, int>>(),
                _fixture.Create<Func<IGrouping<int, object>, object>>());

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void GroupTo_IsEmpty_ReturnEmpty()
        {
            //Arrange

            //Act
            var actual = CollectionExtension.GroupTo(new List<object>(),
                _fixture.Create<Func<object, int>>(),
                _fixture.Create<Func<IGrouping<int, object>, object>>());

            //Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void GroupTo_NotNullAndNotEmpty_ReturnCorrectResult()
        {
            //Arrange
            var fakeList = GetFakeList();
            var expected = new Dictionary<int, int>
            {
                { 1, 220 },
                { 2, 50 },
                { 3, 10 }
            };

            //Act
            var result = CollectionExtension.GroupTo(fakeList,
                _ => _.Key,
                _ => new Fake_GroupTo_Class
                {
                    Key = _.Key,
                    Value = _.Sum(__ => __.Value)
                });

            //Assert
            Assert.Equal(expected.Count, result.Count());
            foreach (var pair in expected)
            {
                var item = result.FirstOrDefault(_ => _.Key == pair.Key);
                Assert.True(item != null);
                Assert.Equal(pair.Value, item.Value);
            }
        }

        private static IEnumerable<Fake_GroupTo_Class> GetFakeList()
        {
            return new List<Fake_GroupTo_Class>
            {
                new Fake_GroupTo_Class
                {
                    Key = 1,
                    Value = 100,
                },
                new Fake_GroupTo_Class
                {
                    Key = 1,
                    Value = 120,
                },
                new Fake_GroupTo_Class
                {
                    Key = 2,
                    Value = 50,
                },
                new Fake_GroupTo_Class
                {
                    Key = 3,
                    Value = 50,
                },
                new Fake_GroupTo_Class
                {
                    Key = 3,
                    Value = -40
                }
            };
        }
    }
}