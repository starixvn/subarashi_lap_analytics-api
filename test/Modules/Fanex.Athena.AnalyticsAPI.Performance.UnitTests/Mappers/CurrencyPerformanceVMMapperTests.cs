namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Mappers
{
    using System.Linq;
    using System.Reflection;
    using AutoFixture;
    using Common.Services;
    using Moq;
    using Performance.Mappers;
    using Xunit;

    public class CurrencyPerformanceVMMapperTests
    {
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly CurrencyPerformanceVMMapper _mapper;
        private readonly Fixture _fixture;

        public CurrencyPerformanceVMMapperTests()
        {
            _mockCalculationService = new Mock<ICalculationService>();
            _mapper = new CurrencyPerformanceVMMapper(_mockCalculationService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_WithValidCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new CurrencyPerformanceVMMapper(_mockCalculationService.Object);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<ICurrencyPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Constructor_WithNullCalculationService_CreatesInstance()
        {
            // Act
            var mapper = new CurrencyPerformanceVMMapper(null);

            // Assert
            Assert.NotNull(mapper);
            Assert.IsAssignableFrom<PerformanceVMMapper>(mapper);
            Assert.IsAssignableFrom<ICurrencyPerformanceVMMapper>(mapper);
        }

        [Fact]
        public void Mapper_ImplementsICurrencyPerformanceVMMapper()
        {
            // Assert
            Assert.IsAssignableFrom<ICurrencyPerformanceVMMapper>(_mapper);
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
        public void Mapper_CanBeUsedAsICurrencyPerformanceVMMapper()
        {
            // Arrange
            ICurrencyPerformanceVMMapper interfaceMapper = _mapper;

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
            var mapperType = typeof(CurrencyPerformanceVMMapper);

            // Act
            var mapperMethods = mapperType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // Assert - Should only have constructor, no additional methods
            Assert.Empty(mapperMethods.Where(m => !m.IsSpecialName)); // IsSpecialName filters out constructors and property accessors
        }

        [Fact]
        public void Mapper_HasOnlyOneConstructor()
        {
            // Arrange
            var mapperType = typeof(CurrencyPerformanceVMMapper);

            // Act
            var constructors = mapperType.GetConstructors();

            // Assert
            Assert.Single(constructors);
        }

        [Fact]
        public void Mapper_ConstructorHasCorrectParameters()
        {
            // Arrange
            var mapperType = typeof(CurrencyPerformanceVMMapper);
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
            var mapperType = typeof(CurrencyPerformanceVMMapper);

            // Assert
            Assert.True(mapperType.IsPublic);
        }

        [Fact]
        public void Mapper_TypeIsNotAbstract()
        {
            // Arrange
            var mapperType = typeof(CurrencyPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsAbstract);
        }

        [Fact]
        public void Mapper_TypeIsNotSealed()
        {
            // Arrange
            var mapperType = typeof(CurrencyPerformanceVMMapper);

            // Assert
            Assert.False(mapperType.IsSealed);
        }
    }
}
