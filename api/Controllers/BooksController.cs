using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksApi.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
          _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() =>
          _bookService.GetBooksList();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> GetBook(string id)
        {
          var book = _bookService.GetBook(id);

          if (book == null)
          {
            return NotFound();
          }

          return book;
        }

        [HttpPost]
        public ActionResult<Book> CreateBook(BookDTO book)
        {
          var result = _bookService.CreateBook(book);

          return CreatedAtRoute("GetBook", new { id = result.Id }, result);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult UpdateBook(string id, BookDTO bookItem)
        {
          var result = _bookService.UpdateBook(id, bookItem);

          if (result.ModifiedCount == 0)
            return NotFound();

          return NoContent(); // TODO: return a book
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteBook([Required]string id)
        {
          var result = _bookService.DeleteBook(id.ToString());

          if (result.DeletedCount == 0)
            return NotFound();

          return NoContent();
        }
    }
}