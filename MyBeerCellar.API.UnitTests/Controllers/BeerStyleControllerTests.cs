using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using Xunit;

namespace MyBeerCellar.API.UnitTests.Controllers
{
    public class BeerStyleControllerTests : BaseUnitTest
    {
        private readonly MyBeerCellarContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BeerStyleController _controller;

        public BeerStyleControllerTests()
        {
            _mockMapper = TestMockRepository.Create<IMapper>();
            _context = new MyBeerCellarContext();
            _controller = new BeerStyleController(_context,
                _mockMapper.Object);
        }

        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
        {
            // Arrange
            MyBeerCellarContext context = null;
            Action ctor = () => new BeerStyleController(context,
                _mockMapper.Object);

            // Act + Assert
            ctor.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
        {
            // Arrange
            IMapper mapper = null;
            Action ctor = () => new BeerStyleController(_context,
                mapper);

            // Act + Assert
            ctor.Should()
                .Throw<ArgumentNullException>();
        }
    }
}
