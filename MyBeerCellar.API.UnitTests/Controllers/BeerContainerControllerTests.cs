using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using Xunit;

namespace MyBeerCellar.API.UnitTests.Controllers
{
    public class BeerContainerControllerTests : BaseUnitTest
    {
        private MyBeerCellarContext _context;
        private Mock<IMapper> _mockMapper;

        public BeerContainerControllerTests()
        {
            _mockMapper = TestMockRepository.Create<IMapper>();
        }

        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_DbContext_Is_Null()
        {
            // Arrange
            MyBeerCellarContext context = null;
            Action ctor = () => new BeerContainerController(context,
                _mockMapper.Object);

            // Act + Assert
            ctor.Should()
                .Throw<ArgumentNullException>();
        }
    }
}
