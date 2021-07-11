using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Models;
using BooksApi.Services;


namespace BooksApi.Controllers
{
    [Route("api/bookshelves")]
    [ApiController]
    public class BookshelvesController : ControllerBase
    {

      private readonly BookshelfService _service;

      public BookshelvesController(BookshelfService service)
      {
        _service = service;
      }

      [HttpGet]
      [Route("my")]
      public ActionResult<List<Book>> Get()
      {
        var userId = _GetUserId();

        if (String.IsNullOrEmpty(userId))
          return Unauthorized();

        var books = _service.Get(userId);

        if (books == null)
          return NotFound();

        return books;
      }

      [HttpPut]
      [Route("add")]
      public ActionResult<Bookshelf> AddBook([Required]string bookId)
      {
        var userId = _GetUserId();

        if (string.IsNullOrEmpty(userId))
          return Unauthorized();

        if (bookId == null)
          return BadRequest();

        return _service.AddBook(userId, bookId);;
      }

      private string _GetUserId()
      {
        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId;
      }

    }

}