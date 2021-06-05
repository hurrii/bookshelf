import React, { useEffect, useState } from 'react'
import './App.scss'

interface Book {
  id: string;
  name: string;
  price: string;
  category: string;
  author: string;
}

function App() {
  const [books, updateBooks] = useState(null)

  useEffect(() => {
    fetchBooks();
  }, [])

  async function fetchBooks() {
    const apiPath = 'https://localhost:5001/api/';

    try {
      const resp = await fetch(`${apiPath}Books`, { method: 'GET', headers: { 'Content-Type': 'application/json' }});
      const result = await resp.json();

      updateBooks(result);
    } catch (err) {
      console.error(err);
    }
  }

  let booksList: Book[] = null;

  if (books) {
    booksList = books.map((book: Book) => {
      return (
        <ul key={book.id}>
          <li>{book.id}</li>
          <li>{book.name}</li>
          <li>{book.category}</li>
          <li>{book.author}</li>
        </ul>
      )
    });
  }


  return (
    <div className="app">
      { booksList }
    </div>
  )
}

export default App
