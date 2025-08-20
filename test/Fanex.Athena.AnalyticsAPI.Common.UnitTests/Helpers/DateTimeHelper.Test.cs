namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using Common.Helpers;
    using Xunit;

    public class DateTimeHelper_Test
    {
        [Theory]
        [MemberData(nameof(FakeInputForGetDistanceBetweenTwoDate))]
        public void GetDistanceBetweenTwoDate_WhenCall_ReturnCorrectResult(DateTime from, DateTime to, int expected)
        {
            //Arrange

            //Act
            var actual = DateTimeHelper.GetDistanceBetweenTwoDate(from, to);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(FakeInputForParseFromFormatDateToDate))]
        public void ParseFromFormatDateToDate_WhenCall_ReturnCorrectResult(string dateString, DateTime expected)
        {
            //Arrange

            //Act
            var actual = DateTimeHelper.ParseFromFormatDateToDate(dateString);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(FakeInputForParseFromFormatToString))]
        public void ParseFromFormatToString_WhenCall_ReturnCorrectResult(string dateString, bool includeEndOfMonth, string expected)
        {
            //Arrange

            //Act
            var actual = DateTimeHelper.ParseFromFormatToString(dateString, includeEndOfMonth);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> FakeInputForParseFromFormatToString()
        {
            yield return new object[]
            {
                "01/2019",
                false,
                "01/01/2019"
            };

            yield return new object[]
            {
                "12/2017",
                true,
                "12/31/2017"
            };

            yield return new object[]
            {
                "10/20/2019",
                false,
                "10/01/2019",
            };

            yield return new object[]
            {
                "10/31/2019",
                true,
                "10/31/2019",
            };
        }

        public static IEnumerable<object[]> FakeInputForParseFromFormatDateToDate()
        {
            yield return new object[]
            {
                "01/01/2019",
                new DateTime(2019, 1, 1)
            };

            yield return new object[]
            {
                "12/30/2017",
                new DateTime(2017, 12, 30)
            };

            yield return new object[]
           {
                "10/31/2019",
                new DateTime(2019, 10, 31)
           };
        }

        public static IEnumerable<object[]> FakeInputForGetDistanceBetweenTwoDate()
        {
            yield return new object[]
            {
                new DateTime(2019, 1, 1),
                new DateTime(2019, 12, 1),
                11
            };

            yield return new object[]
            {
                new DateTime(2018, 11, 1),
                new DateTime(2019, 1, 1),
                2
            };

            yield return new object[]
            {
                new DateTime(2017, 1, 1),
                new DateTime(2019, 2, 1),
                25
            };

            yield return new object[]
            {
                new DateTime(2019, 1, 1),
                new DateTime(2019, 1, 1),
                0
            };
        }
    }
}