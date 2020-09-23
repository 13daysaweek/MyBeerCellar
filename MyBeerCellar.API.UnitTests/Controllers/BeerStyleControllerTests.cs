using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;
using Xunit;

namespace MyBeerCellar.API.UnitTests.Controllers
{
    public class BeerStyleControllerTests : BaseUnitTest
    {
        private MyBeerCellarContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BeerStyleController _controller;

        public BeerStyleControllerTests()
        {
            InitContext();
            _mockMapper = TestMockRepository.Create<IMapper>();
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

        [Fact]
        public async Task GetAsync_Should_Return_All_Styles()
        {
            // Arrange
            var styles = TestFixture.Create<IEnumerable<BeerStyle>>();
            await _context.BeerStyles.AddRangeAsync(styles);
            await _context.SaveChangesAsync();

            // Act
            var actualStyles = await _controller.GetAsync();

            // Assert
            actualStyles.Should()
                .NotBeNull();

            actualStyles.Should()
                .HaveCount(styles.Count());
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Item_With_Matching_Id()
        {
            // Arrange
            var style = TestFixture.Create<BeerStyle>();
            await _context.BeerStyles.AddAsync(style);
            await _context.SaveChangesAsync();

            // Act
            var actualStyle = await _controller.GetByIdAsync(style.StyleId);

            // Assert
            actualStyle.Should()
                .NotBeNull();

            actualStyle.Should()
                .BeOfType<OkObjectResult>();

            var okResult = actualStyle as OkObjectResult;
            var returnedStyle = okResult.Value as BeerStyle;

            returnedStyle.StyleName
                .Should()
                .Be(style.StyleName);

        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_NotFound_When_Id_Does_Not_Exist()
        {
            // Arrange
            var id = TestFixture.Create<int>();

            // Act
            var result = await _controller.GetByIdAsync(id);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PostAsync_Should_Add_Item_To_Database()
        {
            // Arrange
            var createRequest = TestFixture.Create<CreateBeerStyle>();
            var itemToCreate = TestFixture.Create<BeerStyle>();
            _mockMapper.Setup(it => it.Map<BeerStyle>(createRequest))
                .Returns(itemToCreate);

            // Act
            var result = await _controller.PostAsync(createRequest);

            // Assert
            TestMockRepository.VerifyAll();

            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<CreatedAtActionResult>();

            var dbItem = await _context.BeerStyles.FindAsync(itemToCreate.StyleId);

            dbItem.Should()
                .NotBeNull();

            dbItem.StyleName
                .Should()
                .Be(itemToCreate.StyleName);
        }

        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<MyBeerCellarContext>()
                .UseInMemoryDatabase("MyBeerCellar");

            _context = new MyBeerCellarContext(builder.Options);
        }
    }
}
