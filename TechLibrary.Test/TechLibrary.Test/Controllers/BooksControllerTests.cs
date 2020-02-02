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
using TechLibrary.MappingProfiles;

namespace TechLibrary.Controllers.Tests
{
    [TestFixture()]
    [Category("ControllerTests")]
    public class BooksControllerTests
    {

        private  Mock<ILogger<BooksController>> _mockLogger;
        private  Mock<IBookService> _mockBookService;
        private  Mock<IMapper> _mockMapper;
        private IMapper _mapper;
        private NullReferenceException _expectedException;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _expectedException = new NullReferenceException("Test Failed...");
            _mockLogger = new Mock<ILogger<BooksController>>();
            _mockBookService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();

            var mapperProfile = new DomainToResponseProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            _mapper = new Mapper(configuration);
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
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mapper);

            //  Act
            var result = await sut.GetBooks(0, 0);

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");
        }

        [Test()]
        public async Task GetWithValidStartAndCount()
        {
            //  Arrange
            var books = new List<Book>
            {
              new Book { BookId = 1 },
              new Book { BookId = 2 }
            };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mapper);

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

            // verify that 1 item was returned
            Assert.That(okResult.Value is List<BookResponse>);
            var returnedList = okResult.Value as List<BookResponse>;
            Assert.That(returnedList.Count, Is.EqualTo(1));
        }

        [Test()]
        public async Task GetWithCountGreaterThanRemainingItems()
        {
            //  Arrange
            var books = new List<Book> { new Book { BookId = 1 } };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.GetBooks(1, 2);

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");

            var okResult = result as OkObjectResult;

            // ensure that the cast to OkObjectResult succeeded
            Assert.IsNotNull(okResult);
            Assert.That(okResult is OkObjectResult);

            // verify that the response was 200 OK
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test()]
        public async Task GetWithMatchingSearchTitle()
        {
            //  Arrange
            var books = new List<Book>
            {
              new Book { BookId = 1, Title = "The Great Gatsby" },
              new Book { BookId = 2, Title = "War and Peace" },
              new Book { BookId = 3, Title = "Unity of Problems" }
            };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mapper);

            //  Act
            var result = await sut.GetBooksByTitle("Unity");

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");

            var okResult = result as OkObjectResult;

            // ensure that the cast to OkObjectResult succeeded
            Assert.IsNotNull(okResult);
            Assert.That(okResult is OkObjectResult);

            // verify that the response was 200 OK
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            // verify that 1 item was returned
            Assert.That(okResult.Value is List<BookResponse>);
            var returnedList = okResult.Value as List<BookResponse>;
            Assert.That(returnedList.Count, Is.EqualTo(1));
        }

        [Test()]
        public async Task GetWithMatchingSearchTitleButDifferentCasing()
        {
            //  Arrange
            var books = new List<Book>
            {
              new Book { BookId = 1, Title = "The Great Gatsby" },
              new Book { BookId = 2, Title = "War and Peace" },
              new Book { BookId = 3, Title = "Unity of Problems" }
            };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mapper);

            //  Act
            var result = await sut.GetBooksByTitle("uNiTy");

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");

            var okResult = result as OkObjectResult;

            // ensure that the cast to OkObjectResult succeeded
            Assert.IsNotNull(okResult);
            Assert.That(okResult is OkObjectResult);

            // verify that the response was 200 OK
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            // verify that 1 item was returned
            Assert.That(okResult.Value is List<BookResponse>);
            var returnedList = okResult.Value as List<BookResponse>;
            Assert.That(returnedList.Count, Is.EqualTo(1));
        }

        [Test()]
        public async Task GetWithNoMatchingSearchTitle()
        {
            //  Arrange
            var books = new List<Book>
            {
              new Book { BookId = 1, Title = "The Great Gatsby" },
              new Book { BookId = 2, Title = "War and Peace" },
              new Book { BookId = 3, Title = "Unity of Problems" }
            };
            _mockBookService.Setup(s => s.GetBooksAsync()).ReturnsAsync(books);

            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mapper);

            //  Act
            var result = await sut.GetBooksByTitle("hseknf");

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync(), Times.Once, "Expected GetBooksAsync to have been called once");

            var okResult = result as OkObjectResult;

            // ensure that the cast to OkObjectResult succeeded
            Assert.IsNotNull(okResult);
            Assert.That(okResult is OkObjectResult);

            // verify that the response was 200 OK
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            // verify that 1 item was returned
            Assert.That(okResult.Value is List<BookResponse>);
            var returnedList = okResult.Value as List<BookResponse>;
            Assert.That(returnedList.Count, Is.EqualTo(0));
        }


    }
}