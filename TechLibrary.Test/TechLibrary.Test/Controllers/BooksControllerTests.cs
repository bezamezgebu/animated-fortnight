using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechLibrary.Models;
using TechLibrary.Domain;
using TechLibrary.Services;

namespace TechLibrary.Controllers.Tests
{
    [TestFixture()]
    [Category("ControllerTests")]
    public class BooksControllerTests
    {

        private  Mock<ILogger<BooksController>> _mockLogger;
        private  Mock<IBookService> _mockBookService;
        private  Mock<IMapper> _mockMapper;
        private NullReferenceException _expectedException;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _expectedException = new NullReferenceException("Test Failed...");
            _mockLogger = new Mock<ILogger<BooksController>>();
            _mockBookService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();
        }

        [SetUp]
        public void SetUp()
        {
            _mockBookService.Reset();
        }

        [Test()]
        public async Task GetAllTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBooksAsync()).Returns(Task.FromResult(It.IsAny<List<Domain.Book>>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.GetBooks(0, 0);

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");
        }

        [Test()]
        public async Task GetWithValidStartAndCount()
        {
            //  Arrange
            var books = new List<Book> { new Book { BookId = 1 } };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.GetBooks(1, 1);

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");

            var okResult = result as OkObjectResult;

            // ensure that the cast to OkObjectResult succeeded
            Assert.IsNotNull(okResult);
            Assert.That(okResult is OkObjectResult);

            // verify that the response was 200 OK
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            // verify that the result contains 4 items
            // Assert.That(okResult.)
        }
    }
}