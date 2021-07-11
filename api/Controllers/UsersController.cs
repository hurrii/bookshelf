using System;
using System.Collections.Generic;
using System.Security.Claims;
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

      [HttpPut]
      public ActionResult<User> Update([FromBody] UserUpdateDTO userUpdates)
      {
        try
        {
          var claimsIdentity = this.User.Identity as ClaimsIdentity;
          var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

          return _service.Update(userUpdates, userId);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
      }

      [AllowAnonymous]
      [Route("register")]
      [HttpPost]
      public ActionResult<User> Register(UserDTO user)
      {
        try
        {
          var result = _service.Register(user);
          return CreatedAtRoute("GetUser", new { id = result.Id }, result);
        }
        catch (ArgumentException err)
        {
            return BadRequest(new { message = err.Message });
        }
      }

      [AllowAnonymous]
      [Route("login")]
      [HttpPost]
      public ActionResult Login([FromBody] LoginRequest user)
      {
        var token = _service.Authenticate(user.Email, user.Password);

        if (token == null)
          return Unauthorized();

        return Ok(new { token });
      }
    }
}