using System.Linq;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    public class ProductPerformanceVMMapperTests
    {
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly ProductPerformanceVMMapper _mapper;
        private readonly Fixture _fixture;

        public ProductPerformanceVMMapperTests()
        {
            _mockCalculationService = new Mock<ICalculationService>();
            _mapper = new ProductPerformanceVMMapper(_mockCalculationService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new ProductPerformanceVMMapper(_mockCalculationService.Object);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<IProductPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Constructor_WithNullCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new ProductPerformanceVMMapper(null);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<IProductPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Mapper_ImplementsIProductPerformanceVMMapper()
        {
            // Assert
            Assert.IsAssignableFrom<IProductPerformanceVMMapper>(_mapper);
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
        public void Mapper_CanBeUsedAsIProductPerformanceVMMapper()
        {
            // Arrange
            IProductPerformanceVMMapper interfaceMapper = _mapper;

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
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Act
            var mapperMethods = mapperType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);

            // Assert - Should only have constructor, no additional methods
            Assert.Empty(mapperMethods.Where(m => !m.IsSpecialName)); // IsSpecialName filters out constructors and property accessors
        }

        [Fact]
        public void Mapper_HasOnlyOneConstructor()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Act
            var constructors = mapperType.GetConstructors();

            // Assert
            Assert.Single(constructors);
        }

        [Fact]
        public void Mapper_ConstructorHasCorrectParameters()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);
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
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Assert
            Assert.True(mapperType.IsPublic);
        }

        [Fact]
        public void Mapper_TypeIsNotAbstract()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsAbstract);
        }

        [Fact]
        public void Mapper_TypeIsNotSealed()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsSealed);
        }

        [Fact]
        public void Mapper_ImplementsCorrectInterfaces()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);
            var interfaces = mapperType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(IProductPerformanceVMMapper), interfaces);
            Assert.Contains(typeof(IPerformanceVMMapper), interfaces);
        }

        [Fact]
        public void Mapper_HasCorrectNamespace()
        {
            // Arrange
            var mapperType = typeof(ProductPerformanceVMMapper);

            // Assert
            Assert.Equal("Fanex.Athena.AnalyticsAPI.Performance.Mappers", mapperType.Namespace);
        }
    }
}
