using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;
using Xunit;

namespace MyBeerCellar.API.UnitTests.Controllers
{
    public class CellarItemControllerTests : BaseContextUnitTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly CellarItemController _controller;

        public CellarItemControllerTests()
        {
            _mockMapper = TestMockRepository.Create<IMapper>();
            _controller = new CellarItemController(Context,
                _mockMapper.Object);
        }

        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
        {
            // Arrange
            MyBeerCellarContext context = null;
            Action ctor = () => new CellarItemController(context,
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
            Action ctor = () => new CellarItemController(Context,
                mapper);

            // Act + Assert
            ctor.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAsync_Should_Return_All_CellarItems()
        {
            // Arrange
            var items = TestFixture.Create<IEnumerable<CellarItem>>();

            await Context.CellarItems.AddRangeAsync(items);
            await Context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAsync();

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .HaveCount(items.Count());
        }

        [Fact]
        public async Task GetById_Should_Return_Item_With_Matching_Id()
        {
            // Arrange
            var existingItem = TestFixture.Create<CellarItem>();
            await Context.CellarItems.AddAsync(existingItem);
            await Context.SaveChangesAsync();

            // Act
            var result = await _controller.GetById(existingItem.CellarItemId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<OkObjectResult>();

            var returnedItem = ((result as OkObjectResult).Value as CellarItem);

            returnedItem.Should()
                .NotBeNull();

            returnedItem.CellarItemId
                .Should()
                .Be(existingItem.CellarItemId);

        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            var id = TestFixture.Create<int>();

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }
    }
}
