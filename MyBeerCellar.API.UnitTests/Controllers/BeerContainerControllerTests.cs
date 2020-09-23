using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;
using Xunit;

namespace MyBeerCellar.API.UnitTests.Controllers
{
    public class BeerContainerControllerTests : BaseUnitTest
    {
        private MyBeerCellarContext _context;
        private Mock<IMapper> _mockMapper;
        private BeerContainerController _controller;

        public BeerContainerControllerTests()
        {
            InitContext();
            _mockMapper = TestMockRepository.Create<IMapper>();
            _controller = new BeerContainerController(_context,
                _mockMapper.Object);
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
        public async Task Get_Should_Return_List_Of_Containers()
        {
            // Arrange
            var containers = TestFixture.Create<IEnumerable<BeerContainer>>();
            _context.BeerContainers.AddRange(containers);
            await _context.SaveChangesAsync();

            // Act
            var results = await _controller.GetAsync();

            // Assert
            results.Should()
                .NotBeNull();

            results.Should()
                .HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Get_By_Id_Should_Return_Entity_With_Matching_Id()
        {
            // Arrange
            var existingContainer = TestFixture.Create<BeerContainer>();
            await _context.BeerContainers.AddAsync(existingContainer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Get(existingContainer.BeerContainerId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var retrievedContainer = okResult.Value as BeerContainer;

            retrievedContainer.ContainerType
                .Should()
                .Be(existingContainer.ContainerType);

        }

        [Fact]
        public async Task Get_By_Id_Should_Return_NotFound_When_Id_Is_Not_Found()
        {
            // Arrange
            var id = int.MaxValue;

            // Act
            var result = await _controller.Get(id);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<MyBeerCellarContext>()
                .UseInMemoryDatabase("MyBeerCellar");

            _context = new MyBeerCellarContext(builder.Options);
        }
    }
}
