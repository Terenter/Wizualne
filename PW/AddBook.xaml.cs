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
    /// Logika interakcji dla klasy AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        MainWindow caller;
        public AddBook(MainWindow caller)
        {
            InitializeComponent();
            this.caller = caller;
            ((Button)this.FindName("add_button")).Click += addHandler;
            ((Button)this.FindName("cancel_button")).Click += cancelHandler;
            this.Closing += closeHandler;
        }

        void addHandler(object sender, RoutedEventArgs e)
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
            new Helpers.BookDbHelper().addBook(book);
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
