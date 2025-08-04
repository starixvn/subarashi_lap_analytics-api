using System;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Helpers
{
    public class CreationalHelperTests
    {
        private readonly Fixture _fixture;

        public CreationalHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Create_WithValidType_ReturnsInstance()
        {
            // Act
            var result = CreationalHelper.Create<TestClass>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestClass>(result);
        }

        [Fact]
        public void Create_WithDifferentTypes_ReturnsCorrectInstances()
        {
            // Act
            var intResult = CreationalHelper.Create<int>();
            var testClassResult = CreationalHelper.Create<TestClass>();

            // Assert
            Assert.Equal(0, intResult);
            Assert.NotNull(testClassResult);
            Assert.IsType<TestClass>(testClassResult);
        }

        [Fact]
        public void Create_WithGenericType_ReturnsInstance()
        {
            // Act
            var result = CreationalHelper.Create<GenericTestClass<string>>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GenericTestClass<string>>(result);
        }

        [Fact]
        public void Create_WithDerivedType_ReturnsInstance()
        {
            // Act
            var result = CreationalHelper.Create<DerivedTestClass>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DerivedTestClass>(result);
            Assert.IsAssignableFrom<TestClass>(result);
        }

        [Fact]
        public void Create_WithStructType_ReturnsInstance()
        {
            // Act
            var result = CreationalHelper.Create<TestStruct>();

            // Assert
            Assert.IsType<TestStruct>(result);
        }

        [Fact]
        public void Create_MultipleCallsReturnDifferentInstances()
        {
            // Act
            var result1 = CreationalHelper.Create<TestClass>();
            var result2 = CreationalHelper.Create<TestClass>();

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotSame(result1, result2);
        }

        [Fact]
        public void Create_WithValueType_ReturnsDefaultValue()
        {
            // Act
            var intResult = CreationalHelper.Create<int>();
            var boolResult = CreationalHelper.Create<bool>();
            var doubleResult = CreationalHelper.Create<double>();

            // Assert
            Assert.Equal(0, intResult);
            Assert.False(boolResult);
            Assert.Equal(0.0, doubleResult);
        }

        // Test classes for the unit tests
        public class TestClass
        {
            public string Name { get; set; }
        }

        public abstract class AbstractTestClass
        {
            public abstract void DoSomething();
        }

        public interface ITestInterface
        {
            void DoSomething();
        }

        public class TestClassWithoutParameterlessConstructor
        {
            public TestClassWithoutParameterlessConstructor(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
        }

        public class GenericTestClass<T>
        {
            public T Value { get; set; }
        }

        public class DerivedTestClass : TestClass
        {
            public int Id { get; set; }
        }

        public class UnrelatedTestClass
        {
            public string Description { get; set; }
        }

        public struct TestStruct
        {
            public int Value { get; set; }
        }
    }
}
