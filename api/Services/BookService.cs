using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using BooksApi.Models;
namespace BooksApi.Services
{
    public class BookService
    {
      private readonly IMongoCollection<Book> _books;

      public BookService(IBookstoreDatabaseSettings settings)
      {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _books = database.GetCollection<Book>(settings.BooksCollectionName);
      }

      public List<Book> GetBooksList() => _books.Find(book => true).ToList();

      public Book GetBook(string id) => _books.Find<Book>(book => book.Id == id).FirstOrDefault();

      public Book CreateBook(BookDTO bookIn)
      {
        var book = MapDtoToBook(bookIn);

        _books.InsertOne(book);

        return book;
      }

      public ReplaceOneResult UpdateBook(string id, BookDTO bookItem)
      {
        var book = MapDtoToBook(bookItem, id);

        return _books.ReplaceOne(book => book.Id == id, book);
      }

      public DeleteResult DeleteBook(string id) => _books.DeleteOne(book => book.Id == id);

      private Book MapDtoToBook(BookDTO bookIn, string id = null)
      {
        return new Book {
          Id = id,
          Name = bookIn.Name,
          Price = bookIn.Price,
          Category = bookIn.Category,
          Author = bookIn.Author
        };
      }
    }
}