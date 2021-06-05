using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
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
          _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
          var book = _bookService.Get(id);

          if (book == null)
          {
            return NotFound();
          }

          return book;
        }

        [HttpPost]
        public ActionResult<Book> Create(BookDTO book)
        {
          var result = _bookService.Create(book);

          return CreatedAtRoute("GetBook", new { id = result.Id }, result);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
          var book = _bookService.Get(id);

          if (book == null)
          {
            return NotFound();
          }

          _bookService.Update(id, bookIn);

          return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
          var book = _bookService.Get(id);

          if (book == null)
          {
            return NotFound();
          }

          _bookService.Remove(book.Id.ToString());

          return NoContent();
        }
    }
}