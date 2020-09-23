using System;
using System.Collections.Generic;
using System.Linq;
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
using MyBeerCellar.API.ViewModels;
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

        [Fact]
        public async Task PostAsync_Should_Save_New_Container_To_Database()
        {
            // Arrange
            var containerToSave = TestFixture.Create<CreateBeerContainer>();
            var mappedContainer = TestFixture.Create<BeerContainer>();

            _mockMapper.Setup(it => it.Map<BeerContainer>(containerToSave))
                .Returns(mappedContainer);

            // Act
            var result = await _controller.PostAsync(containerToSave);

            // Assert
            TestMockRepository.VerifyAll();

            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<CreatedAtActionResult>();

            var dbItem = await _context.BeerContainers.FirstOrDefaultAsync(_ => _.ContainerType == mappedContainer.ContainerType);
            dbItem.Should()
                .NotBeNull();
        }

        [Fact]
        public async Task PostAsync_Should_Return_BadRequest_On_Exception()
        {
            // Arrange
            var containerToSave = TestFixture.Create<CreateBeerContainer>();

            _mockMapper.Setup(it => it.Map<BeerContainer>(containerToSave))
                .Throws<ArgumentException>();

            // Act
            var result = await _controller.PostAsync(containerToSave);

            // Assert
            TestMockRepository.VerifyAll();

            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Existing_Item()
        {
            // Arrange
            var existingItem = TestFixture.Create<BeerContainer>();
            await _context.BeerContainers.AddAsync(existingItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAsync(existingItem.BeerContainerId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NoContentResult>();

            var item = await _context.BeerContainers.FirstOrDefaultAsync(_ => _.BeerContainerId == existingItem.BeerContainerId);

            item.Should()
                .BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_NotFound_When_Id_Does_Not_Exist()
        {
            // Arrange
            var item = TestFixture.Create<BeerContainer>();

            // Act
            var result = await _controller.DeleteAsync(item.BeerContainerId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutAsync_Should_Update_Existing_Container()
        {
            // Arrange
            var existingItem = TestFixture.Create<BeerContainer>();
            var updateRequest = TestFixture.Build<UpdateBeerContainer>()
                .With(_ => _.BeerContainerId, existingItem.BeerContainerId)
                .Create();

            await _context.BeerContainers.AddAsync(existingItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PutAsync(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var updatedContainer = okResult.Value as BeerContainer;

            updatedContainer.ContainerType
                .Should()
                .Be(updatedContainer.ContainerType);

            var dbItem = await _context.BeerContainers.FindAsync(updateRequest.BeerContainerId);
            dbItem.ContainerType
                .Should()
                .Be(updatedContainer.ContainerType);
        }

        [Fact]
        public async Task PutAsync_Should_Return_Not_Found_When_Item_Does_Not_Exist()
        {
            // Arrange
            var itemToUpdate = TestFixture.Create<UpdateBeerContainer>();

            // Act
            var result = await _controller.PutAsync(itemToUpdate);

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
