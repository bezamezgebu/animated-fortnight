﻿using Microsoft.AspNetCore.Mvc;
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

            if (start != 0 && count != 0)
            {
                if (count < 1)
                {
                    return BadRequest("count parameter must be greater than 0");
                }
                books = books.GetRange(start - 1, count);
            }

            var bookResponseList = _mapper.Map<List<BookResponse>>(books);

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
    }
}
