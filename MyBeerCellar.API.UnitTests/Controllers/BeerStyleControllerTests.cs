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
    public class BeerStyleControllerTests : BaseContextUnitTest
    {
        //private MyBeerCellarContext Context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BeerStyleController _controller;

        public BeerStyleControllerTests()
        {
            _mockMapper = TestMockRepository.Create<IMapper>();
            _controller = new BeerStyleController(Context,
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
            Action ctor = () => new BeerStyleController(Context,
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
            await Context.BeerStyles.AddRangeAsync(styles);
            await Context.SaveChangesAsync();

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
            await Context.BeerStyles.AddAsync(style);
            await Context.SaveChangesAsync();

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

            var dbItem = await Context.BeerStyles.FindAsync(itemToCreate.StyleId);

            dbItem.Should()
                .NotBeNull();

            dbItem.StyleName
                .Should()
                .Be(itemToCreate.StyleName);
        }

        [Fact]
        public async Task PostAsync_Should_Return_BadRequest_On_Exception()
        {
            // Arrange
            var createRequest = TestFixture.Create<CreateBeerStyle>();

            _mockMapper.Setup(it => it.Map<BeerStyle>(createRequest))
                .Throws<ArgumentException>();

            // Act
            var result = await _controller.PostAsync(createRequest);

            // Assert
            TestMockRepository.VerifyAll();

            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Item()
        {
            // Arrange
            var style = TestFixture.Create<BeerStyle>();
            await Context.BeerStyles.AddAsync(style);
            await Context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAsync(style.StyleId);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NoContentResult>();

            var dbItem = await Context.BeerStyles.FindAsync(style.StyleId);

            dbItem.Should()
                .BeNull();
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_If_Item_Does_Not_Exist()
        {
            // Arrange
            var id = TestFixture.Create<int>();

            // Act
            var result = await _controller.DeleteAsync(id);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task PutAsync_Should_Update_Existing_Item()
        {
            // Arrange
            var updateRequest = TestFixture.Create<UpdateBeerStyle>();
            var itemToUpdate = TestFixture.Build<BeerStyle>()
                .With(_ => _.StyleId, updateRequest.StyleId)
                .Create();

            await Context.BeerStyles.AddAsync(itemToUpdate);
            await Context.SaveChangesAsync();

            // Act
            var result = await _controller.PutAsync(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<OkObjectResult>();

            var returnedItem = ((result as OkObjectResult).Value as BeerStyle);

            returnedItem.Should()
                .NotBeNull();

            returnedItem.StyleId
                .Should()
                .Be(updateRequest.StyleId);

            returnedItem.StyleName
                .Should()
                .Be(updateRequest.StyleName);

            var dbItem = await Context.BeerStyles.FindAsync(updateRequest.StyleId);

            dbItem.Should()
                .NotBeNull();

            dbItem.StyleName
                .Should()
                .Be(updateRequest.StyleName);
        }

        [Fact]
        public async Task PutAsync_Should_Return_NotFound_When_Style_Does_Not_Exist()
        {
            // Arrange
            var updateRequest = TestFixture.Create<UpdateBeerStyle>();

            // Act
            var result = await _controller.PutAsync(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PutAsync_Should_Return_BadRequest_On_Exception()
        {
            // Arrange
            UpdateBeerStyle updateRequest = null;

            // Act
            var result = await _controller.PutAsync(updateRequest);

            // Assert
            result.Should()
                .NotBeNull();

            result.Should()
                .BeOfType<BadRequestResult>();
        }
    }
}
