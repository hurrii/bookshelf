using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace BooksApi.Models
{
  public class Bookshelf
  {
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      [JsonProperty("id")]

      public string id { get; set; }

      public string ownerId { get; set; }

      public List<string> books { get; set; }
  }
}