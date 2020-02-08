using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;


        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(int start, int count)
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            if (!(start == 0 && count == 0)) // client has requested paging 
            {
                if (count < 1 || start < 1)
                {
                    return BadRequest("start and count parameters must both be greater than 0");
                }

                // if count is greater than the number of items between books[start-1] and books[books.Count]
                // then just return the remaining items
                if (count > books.Count - start)
                {
                    books = books.GetRange(start - 1, books.Count - start + 1);
                }
                else
                {
                    books = books.GetRange(start - 1, count);
                }
            }

            var bookResponseList = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponseList);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetBooksByTitle(string title, int start, int count)
        {
            _logger.LogInformation("Get book by title");

            var allBooks = await _bookService.GetBooksAsync();

            var filteredList = new List<Book>();
            foreach (Book book in allBooks)
            {
                //if the book title contains the searched title add to the filterd list
                if (book.Title.ToLower().Contains(title.ToLower()))
                {
                    filteredList.Add(book);
                }
            }

            if (!(start == 0 && count == 0)) // client has requested paging
            {
                if (count < 1 || start < 1)
                {
                    return BadRequest("start and count parameters must both be greater than 0.");
                }

                if (count > filteredList.Count - start)
                {
                    filteredList = filteredList.GetRange(start - 1, filteredList.Count - start + 1);
                }
                else
                {
                    filteredList = filteredList.GetRange(start - 1, count);
                }
            }


            var bookResponseList = _mapper.Map<List<BookResponse>>(filteredList);

            return Ok(bookResponseList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody]BookRequest bookRequest)
        {
            var allBooks = await _bookService.GetBooksAsync();

            bool foundMatchingBook = false;
            foreach (Book b in allBooks)
            {
                if (id == b.BookId)
                {
                    foundMatchingBook = true;
                    break;
                }
            }

            if (!foundMatchingBook)
            {
                // book with the requested ID doesn't exist
                return BadRequest("Update failed - Book with ID " + id + " not found.");
            }

            _logger.LogInformation("updating book " + id);

            var book = _mapper.Map<Book>(bookRequest);

            await _bookService.UpdateBook(id, book);

            return Ok();
        }

        [HttpPost("create")]        
        public async Task<IActionResult> createBook([FromBody]BookRequest bookRequest)
        {
            _logger.LogInformation("create book");

            var newBook = _mapper.Map<Book>(bookRequest);

            await _bookService.createBook(newBook);

            return Ok();
        }


    }
}
