using System.Linq;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    public class PlatformPerformanceVMMapperTests
    {
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly PlatformPerformanceVMMapper _mapper;
        private readonly Fixture _fixture;

        public PlatformPerformanceVMMapperTests()
        {
            _mockCalculationService = new Mock<ICalculationService>();
            _mapper = new PlatformPerformanceVMMapper(_mockCalculationService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new PlatformPerformanceVMMapper(_mockCalculationService.Object);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<IPlatformPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Constructor_WithNullCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new PlatformPerformanceVMMapper(null);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<IPlatformPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Mapper_ImplementsIPlatformPerformanceVMMapper()
        {
            // Assert
            Assert.IsAssignableFrom<IPlatformPerformanceVMMapper>(_mapper);
        }

        [Fact]
        public void Mapper_ImplementsIPerformanceVMMapper()
        {
            // Assert
            Assert.IsAssignableFrom<IPerformanceVMMapper>(_mapper);
        }

        [Fact]
        public void Mapper_InheritsFromPerformanceVMMapper()
        {
            // Assert
            Assert.IsAssignableFrom<PerformanceVMMapper>(_mapper);
        }

        [Fact]
        public void Mapper_HasCorrectBaseClass()
        {
            // Assert
            Assert.Equal(typeof(PerformanceVMMapper), _mapper.GetType().BaseType);
        }

        [Fact]
        public void Mapper_CanBeUsedAsPerformanceVMMapper()
        {
            // Arrange
            PerformanceVMMapper baseMapper = _mapper;

            // Assert
            Assert.NotNull(baseMapper);
            Assert.Same(_mapper, baseMapper);
        }

        [Fact]
        public void Mapper_CanBeUsedAsIPlatformPerformanceVMMapper()
        {
            // Arrange
            IPlatformPerformanceVMMapper interfaceMapper = _mapper;

            // Assert
            Assert.NotNull(interfaceMapper);
            Assert.Same(_mapper, interfaceMapper);
        }

        [Fact]
        public void Mapper_CanBeUsedAsIPerformanceVMMapper()
        {
            // Arrange
            IPerformanceVMMapper interfaceMapper = _mapper;

            // Assert
            Assert.NotNull(interfaceMapper);
            Assert.Same(_mapper, interfaceMapper);
        }

        [Fact]
        public void Mapper_HasNoAdditionalMethods()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Act
            var mapperMethods = mapperType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);

            // Assert - Should only have constructor, no additional methods
            Assert.Empty(mapperMethods.Where(m => !m.IsSpecialName)); // IsSpecialName filters out constructors and property accessors
        }

        [Fact]
        public void Mapper_HasOnlyOneConstructor()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Act
            var constructors = mapperType.GetConstructors();

            // Assert
            Assert.Single(constructors);
        }

        [Fact]
        public void Mapper_ConstructorHasCorrectParameters()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);
            var constructor = mapperType.GetConstructors().First();

            // Act
            var parameters = constructor.GetParameters();

            // Assert
            Assert.Single(parameters);
            Assert.Equal(typeof(ICalculationService), parameters[0].ParameterType);
            Assert.Equal("calculationService", parameters[0].Name);
        }

        [Fact]
        public void Mapper_TypeIsPublic()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Assert
            Assert.True(mapperType.IsPublic);
        }

        [Fact]
        public void Mapper_TypeIsNotAbstract()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsAbstract);
        }

        [Fact]
        public void Mapper_TypeIsNotSealed()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsSealed);
        }

        [Fact]
        public void Mapper_ImplementsCorrectInterfaces()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);
            var interfaces = mapperType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(IPlatformPerformanceVMMapper), interfaces);
            Assert.Contains(typeof(IPerformanceVMMapper), interfaces);
        }

        [Fact]
        public void Mapper_HasCorrectNamespace()
        {
            // Arrange
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Assert
            Assert.Equal("Fanex.Athena.AnalyticsAPI.Performance.Mappers", mapperType.Namespace);
        }

        [Fact]
        public void Mapper_InheritsAllMethodsFromBase()
        {
            // Arrange
            var baseType = typeof(PerformanceVMMapper);
            var mapperType = typeof(PlatformPerformanceVMMapper);

            // Act
            var baseMethods = baseType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var mapperMethods = mapperType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Assert - Mapper should have at least all the methods from base class
            Assert.True(mapperMethods.Length >= baseMethods.Length);
        }
    }
}
