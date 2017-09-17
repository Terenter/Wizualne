using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW
{
    class BooksViewModel
    {
        private KeyValuePair<int, string> _SelectedBook = new KeyValuePair<int, string>();

        public KeyValuePair<int, string> SelectedBook
        {
            get { return _SelectedBook; }
            set { _SelectedBook = value; }
        }


        private ObservableCollection<KeyValuePair<int, string>> m_BooksList;

        public ObservableCollection<KeyValuePair<int, string>> BooksList
        {
            get { return m_BooksList; }
            set { m_BooksList = value; }
        }


        public BooksViewModel()
        {
            m_BooksList = new ObservableCollection<KeyValuePair<int, string>>();
            foreach (Models.Book book in new Helpers.BookDbHelper().getBooks())
            {
                m_BooksList.Add(new KeyValuePair<int, string>(book.Id, book.Author + " : " + book.Title));
            }
        }
    }
}
