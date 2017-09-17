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
    /// Logika interakcji dla klasy EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        MainWindow caller;
        Models.Book editedBook;

        public EditBook(MainWindow caller, int bookId)
        {
            InitializeComponent();
            this.caller = caller;
            editedBook = new Helpers.BookDbHelper().getBook(bookId);
            ((Button)this.FindName("confirm_button")).Click += confirmHandler;
            ((Button)this.FindName("cancel_button")).Click += cancelHandler;
            ((TextBox)this.FindName("author_text")).Text = editedBook.Author;
            ((TextBox)this.FindName("title_text")).Text = editedBook.Title;
            ((TextBox)this.FindName("genre_text")).Text = editedBook.Genre;
            this.Closing += closeHandler;
        }

        void confirmHandler(object sender, RoutedEventArgs e)
        {
            String author = ((TextBox)this.FindName("author_text")).Text;
            String title = ((TextBox)this.FindName("title_text")).Text;
            String genre = ((TextBox)this.FindName("genre_text")).Text;
            editedBook.Author = author;
            editedBook.Title = title;
            editedBook.Genre = genre;
            new Helpers.BookDbHelper().updateBook(editedBook.Id, editedBook);
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
