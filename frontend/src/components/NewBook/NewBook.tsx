import React, { FormEvent, useEffect, useState } from 'react'
import { Book } from '../../models/book.model';
import { httpClient } from '../../utils/httpClient';
import classes from './NewBook.module.scss';

interface Props {
  fetchBooks: Function
}

function NewBook(props: Props) {
  const [isValid, setValidity] = useState(false)

  const [name, setName] = useState('')
  const [author, setAuthor] = useState('')
  const [category, setCategory] = useState('')
  const [price, setPrice] = useState('')

  async function sendForm(e: FormEvent) {
    e.preventDefault();
    await httpClient.post('books', { name, author, category, price })
    await props.fetchBooks()
  }

  function validateForm(book: Book) {
    const isValid = !Object.values(book).some(val => !val)
    setValidity(isValid)
  }

  function setPriceValue(value: string) {
    const filteredValue = value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');

    setPrice(filteredValue)
  }

  useEffect(() => {
    validateForm({ name, author, category, price })
  }, [name, author, category, price])


  return (
    <form className={ classes.form } onSubmit={ sendForm }>
      <p className={ classes.heading }>Add a new book</p>

      <div className={ classes.field }>
        <label className={ classes.label } htmlFor="name">Title</label>
        <input className={ classes.input } type="text" name="name" value={ name } onChange={ e => setName(e.target.value) }/>
      </div>

      <div className={ classes.field }>
        <label className={ classes.label } htmlFor="author">Author</label>
        <input className={ classes.input } type="text" name="author" value={ author } onChange={ e => setAuthor(e.target.value) }/>
      </div>

      <div className={ classes.field }>
        <label className={ classes.label } htmlFor="category">Category</label>
        <input className={ classes.input } type="text" name="category" value={ category } onChange={ e => setCategory(e.target.value) }/>
      </div>

      <div className={ classes.field }>
        <label className={ classes.label } htmlFor="price">Price</label>
        <input className={ classes.input }
          type="text"
          name="price"
          value={ price }
          onChange={ e => setPriceValue(e.target.value) }/>
      </div>

      <button className={ classes['submit-btn'] } disabled={ !isValid }>Add book</button>
    </form>
  )
}

export default NewBook