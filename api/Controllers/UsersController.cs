using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Services;
using BooksApi.Models;

namespace BooksApi.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
      private readonly UserService _service;

      public UsersController(UserService service)
      {
        _service = service;
      }

      [HttpGet]
      public ActionResult<List<User>> GetUsers() => _service.GetUsers();

      [HttpGet("{id:length(24)}", Name = "GetUser")]
      public ActionResult<User> GetUser(string id)
      {
        var user = _service.GetUser(id);
        return user;
      }

      [HttpPost]
      public ActionResult<User> Create(UserDTO user)
      {
        var result = _service.Create(user);
        return CreatedAtRoute("GetUser", new { id = result.Id }, result);
      }

      [AllowAnonymous]
      [Route("login")]
      [HttpPost]
      public ActionResult Login([FromBody] UserDTO user)
      {
        var token = _service.Authenticate(user.Email, user.Password);

        if (token == null)
          return Unauthorized();

        return Ok(new { token });
      }
    }
}