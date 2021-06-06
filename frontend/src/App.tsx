import React, { useEffect, useState } from 'react'
import './App.scss'
import BooksList from './components/BooksList/BooksList'
import NewBook from './components/NewBook/NewBook'
import { Book } from './models/book.model'
import { httpClient } from './utils/httpClient'

function App() {
  const [books, updateBooks] = useState<Book[]>(null)

  useEffect(() => {
    fetchBooks();
  }, [])

  async function fetchBooks() {
    const books = await httpClient.get('Books')

    if (!books) { return }

    updateBooks(books);
  }

  async function handleBookDeletion(id: string) {
    if (!id) { return }

    const resp = await httpClient.delete(`Books/${id}`)

    if (resp?.status === 204) {
      deleteBookLocally(id)
    }
  }

  function deleteBookLocally(id: string) {
    const bookIndex = books.findIndex(b => b.id === id)

    if (bookIndex !== undefined) {
      const booksArray = [...books]

      booksArray.splice(bookIndex, 1)
      updateBooks(booksArray)
    }
  }

  return (
    <main className="app">
      <section className="section">
        <div className="container">
          <BooksList list={ books } handleBookDeletion={ handleBookDeletion.bind(this) }/>
          <NewBook/>
        </div>
      </section>
    </main>
  )
}

export default App
