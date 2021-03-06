import React from 'react'
import { Book } from '../../models/book.model';
import classes from './bookslist.module.scss';

interface Props {
  list: Book[];
  handleBookDeletion: Function;
}

function BooksList(props: Props) {
  if (props.list?.length) {
    const list = props.list.map(book => (
      <div className={ [classes.book, 'col-3'].join(' ') } key={ book.id }>
        <p className={ [classes.line, classes.name].join(' ') }>{ book.name }</p>
        <p className={ [classes.line, classes.author].join(' ') }>{ book.author }</p>
        <p className={ [classes.line, classes.category].join(' ') }>{ book.category }</p>
        <p className={ [classes.line, classes.price].join(' ') }>{ book.price } $</p>

        <button className={ classes['btn-delete'] } onClick={ () => props.handleBookDeletion(book.id) }>Delete book</button>
      </div>
    ))

    return (
      <div className={ ['row', classes.list].join(' ') }>
        { list }
      </div>
    )
  }

  return <EmptyList/>
}

export default BooksList


function EmptyList() {
  return (
    <p>Нет данных о книгах</p>
  )
}