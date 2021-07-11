using MongoDB.Driver;
using BooksApi.Models;
using System.Collections.Generic;


namespace BooksApi.Services
{
    public class BookshelfService
    {
        private readonly IMongoCollection<Bookshelf> _bookshelves;
        private readonly IMongoCollection<Book> _books;

        public BookshelfService(IBookstoreDatabaseSettings settings)
        {
          var client = new MongoClient(settings.ConnectionString);
          var db = client.GetDatabase(settings.DatabaseName);

          _bookshelves = db.GetCollection<Bookshelf>(settings.BookshelvesCollectionName);
          _books = db.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get(string userId)
        {
          var bookshelf = _bookshelves.Find(bookshelf => bookshelf.ownerId == userId).FirstOrDefault();

          if (bookshelf.books == null || bookshelf.books.Count == 0)
            return null;


          var books = new List<Book>();

          foreach (var bookId in bookshelf.books)
          {
              var book = _books.Find(_book => _book.Id == bookId).FirstOrDefault();

              if (book == null)
                continue;

              books.Add(book);
          }

          return books;
        }

        public Bookshelf AddBook(string userId, string bookId)
        {
          var doesBookExist = _books.Find(book => book.Id == bookId).FirstOrDefault() != null;

          if (!doesBookExist)
            return null;


          var update = Builders<Bookshelf>.Update.AddToSet(x => x.books, bookId);
          var options = new  FindOneAndUpdateOptions<Bookshelf, Bookshelf>() { IsUpsert = true };

          var bookshelf = _bookshelves.FindOneAndUpdate<Bookshelf>(bookshelf => bookshelf.ownerId == userId, update, options);

          System.Console.WriteLine(bookshelf);

          return bookshelf;
        }
    }
}