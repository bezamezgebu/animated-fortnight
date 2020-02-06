using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);

        Task UpdateBook(int bookid, Book updatedBook);

        Task createBook(Book newBook);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task UpdateBook(int bookid, Book updatedBook)
        {
            updatedBook.BookId = bookid;

            var book = GetBookByIdAsync(bookid);
            if (book == null)
            {
                throw new ArgumentException();
            }

            // update properties of book with values from updatedBook
            book.Result.Title = updatedBook.Title;
            book.Result.ISBN = updatedBook.ISBN;
            book.Result.PublishedDate = updatedBook.PublishedDate;
            book.Result.ThumbnailUrl = updatedBook.ThumbnailUrl;
            book.Result.LongDescr = updatedBook.LongDescr;
            book.Result.ShortDescr = updatedBook.ShortDescr;

            await _dataContext.SaveChangesAsync();
        }

        public async Task createBook(Book newBook)
        {
            _dataContext.Books.Add(new Book()
            {
                Title = newBook.Title,
                ISBN = newBook.ISBN,
                PublishedDate =newBook.PublishedDate,
                ThumbnailUrl = newBook.ThumbnailUrl,
                ShortDescr = newBook.ShortDescr,
                LongDescr = newBook.LongDescr
            }) ;
        //    updatedBook.BookId = bookid;
        //    var book = GetBookByIdAsync(bookid);
        //    if (book == null)
        //    {
        //        throw new ArgumentException();
        //    }
        //    // update properties of book with values from updatedBook
        //    book.Result.Title = updatedBook.Title;
        //    book.Result.ISBN = updatedBook.ISBN;
        //    book.Result.PublishedDate = updatedBook.PublishedDate;
        //    book.Result.ThumbnailUrl = updatedBook.ThumbnailUrl;
        //    book.Result.LongDescr = updatedBook.LongDescr;
        //    book.Result.ShortDescr = updatedBook.ShortDescr;

            await _dataContext.SaveChangesAsync();
        }
    }
}
