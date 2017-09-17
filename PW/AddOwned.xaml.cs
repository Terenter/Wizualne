using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PW
{
    /// <summary>
    /// Logika interakcji dla klasy AddOwned.xaml
    /// </summary>
    public partial class AddOwned : Window
    {
        MainWindow caller;
        public AddOwned(MainWindow caller)
        {
            InitializeComponent();
            this.caller = caller;
            ((Button)this.FindName("add_button")).Click += addHandler;
            ((Button)this.FindName("cancel_button")).Click += cancelHandler;
            this.Closing += closeHandler;
            this.DataContext = new BooksViewModel();
            //MessageBox.Show(((BooksViewModel)DataContext).BooksList.Count.ToString());
            fillCombo();
        }

        private void fillCombo()
        {
            List<Models.Book> books = new Helpers.BookDbHelper().getBooks();
            ComboBox combo = (ComboBox) FindName("book_combo");
            Dictionary<int, string> comboS = new Dictionary<int, string>();
            for(var enu = books.GetEnumerator(); enu.MoveNext(); )
            {
                comboS.Add(enu.Current.Id, enu.Current.Author + " : " + enu.Current.Title);
            }
            combo.ItemsSource = comboS;
            combo.DisplayMemberPath = "Value";
            combo.SelectedValuePath = "Key";
        }

        void addHandler(object sender, RoutedEventArgs e)
        {
            int bookId;
            if (((CheckBox)FindName("handwritten_check")).IsChecked == true)
            {
                String author = ((TextBox)this.FindName("author_text")).Text;
                String title = ((TextBox)this.FindName("title_text")).Text;
                String genre = ((TextBox)this.FindName("genre_text")).Text;
                Models.Book book = new Models.Book();
                book.Author = author;
                book.Title = title;
                book.Genre = genre;
                book.UserId = -1;
                book.ISBN = "...";
                bookId = new Helpers.BookDbHelper().addBook(book);
            }
            else
            {
                ComboBox combo = (ComboBox)FindName("book_combo");
                bookId = ((KeyValuePair<int, string>)combo.SelectedItem).Key;
            }
            Models.OwnedBook oBook = new Models.OwnedBook();
            oBook.Favorite = false;
            oBook.UserId = -1;
            oBook.BookId = bookId;
            oBook.Aquired = DateTime.Now;
            new Helpers.OwnedDbHelper().addOwnedBook(oBook);
            this.Close();
            caller.Visibility = Visibility.Visible;
            caller.refresh();
        }

        void cancelHandler(object sender, RoutedEventArgs e)
        {
            this.Close();
            caller.Visibility = Visibility.Visible;
        }

        void closeHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            caller.Visibility = Visibility.Visible;
        }
    }
}
