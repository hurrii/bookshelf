using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using BooksApi.Models;

namespace BooksApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly string jwtKey;

        public UserService(IBookstoreDatabaseSettings settings)
        {
          this.jwtKey = settings.JwtKey;

          var client = new MongoClient(settings.ConnectionString);
          var database = client.GetDatabase(settings.DatabaseName);

          _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> GetUsers() => _users.Find(user => true).ToList();

        public User GetUser(string id) => _users.Find(user => user.Id == id).FirstOrDefault();

        public User Create(UserDTO userIn)
        {
          var existingUser = _users.Find(user => user.Email == userIn.Email).FirstOrDefault();

          if (existingUser != null)
            return null;

          var passwordHash = BCrypt.Net.BCrypt.HashPassword(userIn.Password);

          var newUser = new User {
            Email = userIn.Email,
            Password = passwordHash
          };

          _users.InsertOne(newUser);
          return newUser;
        }

        public string Authenticate(string email, string password)
        {
          var user = _users.Find(user => user.Email == email).FirstOrDefault();

          if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

          var tokenHandler = new JwtSecurityTokenHandler();

          var tokenKey = Encoding.ASCII.GetBytes(jwtKey.ToString());

          var tokenDescriptior = new SecurityTokenDescriptor() {

            Subject = new ClaimsIdentity(new Claim[] {
              new Claim(ClaimTypes.Email, email)
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(tokenKey),
                  SecurityAlgorithms.HmacSha256Signature
                )
          };

          var token = tokenHandler.CreateToken(tokenDescriptior);

          return tokenHandler.WriteToken(token);
        }
    }
}