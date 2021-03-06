namespace BooksApi.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
      public string BooksCollectionName { get; set; }
      public string UsersCollectionName { get; set; }
      public string BookshelvesCollectionName { get; set; }
      public string ConnectionString { get; set; }
      public string DatabaseName { get; set; }
      public string JwtKey { get; set; }
    }

    public interface IBookstoreDatabaseSettings
    {
      string BooksCollectionName { get; set; }
      string UsersCollectionName { get; set; }
      string BookshelvesCollectionName { get; set; }
      string ConnectionString { get; set; }
      string DatabaseName { get; set; }
      string JwtKey { get; set; }
    }
}