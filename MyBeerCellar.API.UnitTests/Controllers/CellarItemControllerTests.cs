using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Azure.Core;
using FluentAssertions;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBeerCellar.API.Controllers;
using MyBeerCellar.API.Data;
using MyBeerCellar.API.Models;
using MyBeerCellar.API.ViewModels;
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

        [Fact]
        public async Task Post_Should_Add_New_Item_To_Db()
        {
            // Arrange
            var addRequest = TestFixture.Create<CreateCellarItem>();
            var itemToAdd = TestFixture.Create<CellarItem>();

            _mockMapper.Setup(it => it.Map<CellarItem>(addRequest))
                .Returns(itemToAdd);

            // Act
            var result = await _controller.Post(addRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<CreatedAtActionResult>();

            var dbItem = await Context.CellarItems.FindAsync(itemToAdd.CellarItemId);

            dbItem.Should()
                .NotBeNull();
        }

        [Fact]
        public async Task Post_Should_Return_BadRequest_On_Exception()
        {
            // Arrange
            var addRequest = TestFixture.Create<CreateCellarItem>();

            _mockMapper.Setup(it => it.Map<CellarItem>(addRequest))
                .Throws<ArgumentException>();

            // Act
            var result = await _controller.Post(addRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_Should_Remove_Item()
        {
            // Arrange
            var itemToRemove = TestFixture.Create<CellarItem>();
            await Context.CellarItems.AddAsync(itemToRemove);
            await Context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(itemToRemove.CellarItemId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NoContentResult>();

            var dbItem = await Context.CellarItems.FindAsync(itemToRemove.CellarItemId);
            dbItem.Should()
                .BeNull();
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            var id = TestFixture.Create<int>();

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Put_Should_Update_Existing_Item()
        {
            // Arrange
            var updateRequest = TestFixture.Create<UpdateCellarItem>();
            var itemToUpdate = TestFixture.Build<CellarItem>()
                .With(_ => _.CellarItemId, updateRequest.CellarItemId)
                .With(_ => _.BeerStyleId, updateRequest.BeerStyleId)
                .With(_ => _.BeerContainerId, updateRequest.BeerContainerId)
                .Create();

            var style = TestFixture.Build<BeerStyle>()
                .With(_ => _.StyleId, updateRequest.BeerStyleId)
                .Create();

            var container = TestFixture.Build<BeerContainer>()
                .With(_ => _.BeerContainerId, updateRequest.BeerContainerId)
                .Create();

            await Context.BeerStyles.AddAsync(style);
            await Context.BeerContainers.AddAsync(container);
            await Context.CellarItems.AddAsync(itemToUpdate);
            await Context.SaveChangesAsync();


            // Act
            var result = await _controller.Put(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<OkObjectResult>();

            var updatedItem = ((result as OkObjectResult).Value as CellarItem);

            updatedItem.Should()
                .NotBeNull();

            updatedItem.CellarItemId
                .Should()
                .Be(updateRequest.CellarItemId);

            updatedItem.ItemName
                .Should();
        }

        [Fact]
        public async Task Put_Should_Return_Not_Found_When_Item_Does_Not_Exist()
        {
            // Arrange
            var item = TestFixture.Create<UpdateCellarItem>();

            // Act
            var result = await _controller.Put(item);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Put_Should_Return_BadRequest_On_Exception()
        {
            // Arrange
            UpdateCellarItem updateRequest = null;

            // Act
            var result = await _controller.Put(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<BadRequestResult>();
        }
    }
}
