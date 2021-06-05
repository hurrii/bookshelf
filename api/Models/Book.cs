using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace BooksApi.Models
{
    public class Book
    {
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("category")]
      public string Category { get; set; }

      [JsonProperty("author")]
      public string Author { get; set; }
    }

    public class BookDTO
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("price")]
      public decimal Price { get; set; }

      [JsonProperty("category")]
      public string Category { get; set; }

      [JsonProperty("author")]
      public string Author { get; set; }
    }
}