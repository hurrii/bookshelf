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
          var user = MapDtoToUser(userIn);

          _users.InsertOne(user);
          return user;
        }

        public string Authenticate(string email, string password)
        {
          var user = _users.Find(user => user.Email == email && user.Password == password).FirstOrDefault();

          if (user == null)
            return null;

          var tokenHandler = new JwtSecurityTokenHandler();

          Console.WriteLine(jwtKey);
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

          Console.WriteLine("service token:");
          Console.WriteLine(tokenHandler.WriteToken(token));

          return tokenHandler.WriteToken(token);
        }

        private User MapDtoToUser(UserDTO userIn)
        {
          return new User {
            Email = userIn.Email,
            Password = userIn.Password
          };
        }
    }
}