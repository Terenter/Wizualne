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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String[] bookHeaders = { "Autor", "Tytuł", "Gatunek" };
        String[] ownedHeaders = { "Autor", "Tytuł", "Gatunek", "Od"};
        String[] whishlistedHeaders = { "Autor", "Tytuł", "Gatunek", "Od"};
        Helpers.BookDbHelper bDB = new Helpers.BookDbHelper();
        Helpers.OwnedDbHelper oDB = new Helpers.OwnedDbHelper();
        Helpers.WhislistedDbHelper wDB = new Helpers.WhislistedDbHelper();
        static MainWindow window;
        int currentState = 1;

        Button books_b;
        Button owned_b;
        Button whish_b;
        Button new_b;

        public MainWindow()
        {
            InitializeComponent();
            window = this;
            bDB.getBooks();
            wDB.getWhishlistedBook();
            oDB.getOwnedBook();
            fillOwned();
            books_b = (Button)this.FindName("books_button");
            owned_b = (Button)this.FindName("owned_button");
            whish_b = (Button)this.FindName("whishlisted_button");
            new_b = (Button)this.FindName("new_button");
            books_b.Click += booksClick;
            owned_b.Click += ownedClick;
            whish_b.Click += whishlistedClick;
            new_b.Click += addHandler;
            books_b.Background = Brushes.Goldenrod;
            owned_b.Background = Brushes.Gold;
            whish_b.Background = Brushes.Goldenrod;
            new_b.Background = Brushes.Goldenrod;
        }

        private void booksClick(object sender, RoutedEventArgs e)
        {
            fillBooks();
            books_b.Background = Brushes.Gold;
            owned_b.Background = Brushes.Goldenrod;
            whish_b.Background = Brushes.Goldenrod;
        }

        private void ownedClick(object sender, RoutedEventArgs e)
        {
            fillOwned();
            books_b.Background = Brushes.Goldenrod;
            owned_b.Background = Brushes.Gold;
            whish_b.Background = Brushes.Goldenrod;
        }

        private void whishlistedClick(object sender, RoutedEventArgs e)
        {
            fillWhishlisted();
            books_b.Background = Brushes.Goldenrod;
            owned_b.Background = Brushes.Goldenrod;
            whish_b.Background = Brushes.Gold;
        }

        private void fillBooks()
        {
            currentState = 0;
            Grid header = (Grid)this.FindName("table_header");
            Grid content = (Grid)this.FindName("table_content");
            header.Children.Clear();
            content.Children.Clear();
            header.ColumnDefinitions.Clear();
            content.ColumnDefinitions.Clear();
            content.RowDefinitions.Clear();
            for (int i = 0; i < bookHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                header.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef = new ColumnDefinition();
            nDef.Width = new GridLength(SystemParameters.VerticalScrollBarWidth + 60);
            header.ColumnDefinitions.Add(nDef);
            for (int i = 0; i < bookHeaders.Length; ++i)
            {
                TextBox text = new TextBox();
                text.Text = bookHeaders[i];
                text.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(text, i);
                Grid.SetRow(text, 1);
                header.Children.Add(text);
            }
            for (int i = 0; i < bookHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                content.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef2 = new ColumnDefinition();
            nDef2.Width = new GridLength(60);
            content.ColumnDefinitions.Add(nDef2);
            List<Models.Book> books = bDB.getBooks();
            int j = 0;
            for (var enu = books.GetEnumerator(); enu.MoveNext(); ++j)
            {
                RowDefinition def = new RowDefinition();
                def.Height = new GridLength(25);
                content.RowDefinitions.Add(def);
                TextBox title = new TextBox();
                title.Text = enu.Current.Title;
                title.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(title, 1);
                Grid.SetRow(title, j);
                content.Children.Add(title);
                TextBox author = new TextBox();
                author.Text = enu.Current.Author;
                author.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(author, 0);
                Grid.SetRow(author, j);
                content.Children.Add(author);
                TextBox genre = new TextBox();
                genre.Text = enu.Current.Genre;
                genre.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(genre, 2);
                Grid.SetRow(genre, j);
                content.Children.Add(genre);
                Button edit = new Button();
                edit.Content = "Edytuj";
                Grid.SetColumn(edit, 3);
                Grid.SetRow(edit, j);
                content.Children.Add(edit);
                int id = enu.Current.Id;
                edit.Click += (sender, EventArgs) => { editionHandler(sender, EventArgs, id); };
                if (j % 2 == 1)
                {
                    title.Background = Brushes.Aquamarine;
                    author.Background = Brushes.Aquamarine;
                    genre.Background = Brushes.Aquamarine;
                }
                else
                {
                    title.Background = Brushes.Chartreuse;
                    author.Background = Brushes.Chartreuse;
                    genre.Background = Brushes.Chartreuse;
                }
                title.IsReadOnly = true;
                author.IsReadOnly = true;
                genre.IsReadOnly = true;
            }
        }

        private void fillOwned()
        {
            currentState = 1;
            Grid header = (Grid)this.FindName("table_header");
            Grid content = (Grid)this.FindName("table_content");
            header.Children.Clear();
            content.Children.Clear();
            header.ColumnDefinitions.Clear();
            content.ColumnDefinitions.Clear();
            content.RowDefinitions.Clear();
            for (int i = 0; i < ownedHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                header.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef = new ColumnDefinition();
            nDef.Width = new GridLength(SystemParameters.VerticalScrollBarWidth + 60);
            header.ColumnDefinitions.Add(nDef);
            for (int i = 0; i < ownedHeaders.Length; ++i)
            {
                TextBox text = new TextBox();
                text.Text = ownedHeaders[i];
                text.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(text, i);
                Grid.SetRow(text, 1);
                header.Children.Add(text);
            }
            for (int i = 0; i < ownedHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                content.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef2 = new ColumnDefinition();
            nDef2.Width = new GridLength(60);
            content.ColumnDefinitions.Add(nDef2);
            List<Models.GetOwnedBook> books = oDB.getOwnedBooks() ;
            int j = 0;
            for (var enu = books.GetEnumerator(); enu.MoveNext(); ++j)
            {
                RowDefinition def = new RowDefinition();
                def.Height = new GridLength(25);
                content.RowDefinitions.Add(def);
                TextBox title = new TextBox();
                title.Text = enu.Current.cBook.Title;
                title.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(title, 1);
                Grid.SetRow(title, j);
                content.Children.Add(title);
                TextBox author = new TextBox();
                author.Text = enu.Current.cBook.Author;
                author.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(author, 0);
                Grid.SetRow(author, j);
                content.Children.Add(author);
                TextBox genre = new TextBox();
                genre.Text = enu.Current.cBook.Genre;
                genre.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(genre, 2);
                Grid.SetRow(genre, j);
                content.Children.Add(genre);
                TextBox from = new TextBox();
                from.Text = enu.Current.Aquired.ToShortDateString();
                from.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(from, 3);
                Grid.SetRow(from, j);
                content.Children.Add(from);
                Button delete = new Button();
                delete.Content = "Usuń";
                Grid.SetColumn(delete, 4);
                Grid.SetRow(delete, j);
                content.Children.Add(delete);
                int id = enu.Current.Id;
                delete.Click += (sender, EventArgs) => { deletionHandler(sender, EventArgs, id);  };
                if (j % 2 == 1)
                {
                    title.Background = Brushes.Aquamarine;
                    author.Background = Brushes.Aquamarine;
                    genre.Background = Brushes.Aquamarine;
                    from.Background = Brushes.Aquamarine;
                }
                else
                {
                    title.Background = Brushes.Chartreuse;
                    author.Background = Brushes.Chartreuse;
                    genre.Background = Brushes.Chartreuse;
                    from.Background = Brushes.Chartreuse;
                }
                title.IsReadOnly = true;
                author.IsReadOnly = true;
                genre.IsReadOnly = true;
                from.IsReadOnly = true;
            }
        }

        private void fillWhishlisted()
        {
            currentState = 2;
            Grid header = (Grid)this.FindName("table_header");
            Grid content = (Grid)this.FindName("table_content");
            header.ColumnDefinitions.Clear();
            content.ColumnDefinitions.Clear();
            content.RowDefinitions.Clear();
            header.Children.Clear();
            content.Children.Clear();
            for (int i = 0; i < whishlistedHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                header.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef = new ColumnDefinition();
            nDef.Width = new GridLength(SystemParameters.VerticalScrollBarWidth + 60);
            header.ColumnDefinitions.Add(nDef);
            for (int i = 0; i < whishlistedHeaders.Length; ++i)
            {
                TextBox text = new TextBox();
                text.Text = whishlistedHeaders[i];
                text.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(text, i);
                Grid.SetRow(text, 1);
                header.Children.Add(text);
            }
            for (int i = 0; i < whishlistedHeaders.Length; ++i)
            {
                ColumnDefinition def = new ColumnDefinition();
                def.Width = new GridLength(1, GridUnitType.Star);
                content.ColumnDefinitions.Add(def);
            }
            ColumnDefinition nDef2 = new ColumnDefinition();
            nDef2.Width = new GridLength(60);
            content.ColumnDefinitions.Add(nDef2);
            List<Models.GetWhislistedBook> books = wDB.getWhishlistedBooks();
            int j = 0;
            for (var enu = books.GetEnumerator(); enu.MoveNext(); ++j)
            {
                RowDefinition def = new RowDefinition();
                def.Height = new GridLength(25);
                content.RowDefinitions.Add(def);
                TextBox title = new TextBox();
                title.Text = enu.Current.cBook.Title;
                title.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(title, 1);
                Grid.SetRow(title, j);
                content.Children.Add(title);
                TextBox author = new TextBox();
                author.Text = enu.Current.cBook.Author;
                author.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(author, 0);
                Grid.SetRow(author, j);
                content.Children.Add(author);
                TextBox genre = new TextBox();
                genre.Text = enu.Current.cBook.Genre;
                genre.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(genre, 2);
                Grid.SetRow(genre, j);
                content.Children.Add(genre);
                TextBox from = new TextBox();
                from.Text = enu.Current.Whishlisted.ToShortDateString();
                from.TextAlignment = TextAlignment.Center;
                Grid.SetColumn(from, 3);
                Grid.SetRow(from, j);
                content.Children.Add(from);
                Button delete = new Button();
                delete.Content = "Usuń";
                Grid.SetColumn(delete, 4);
                Grid.SetRow(delete, j);
                content.Children.Add(delete);
                int id = enu.Current.Id;
                delete.Click += (sender, EventArgs) => { deletionHandler(sender, EventArgs, id); };
                if (j % 2 == 1)
                {
                    title.Background = Brushes.Aquamarine;
                    author.Background = Brushes.Aquamarine;
                    genre.Background = Brushes.Aquamarine;
                    from.Background = Brushes.Aquamarine;
                }
                else
                {
                    title.Background = Brushes.Chartreuse;
                    author.Background = Brushes.Chartreuse;
                    genre.Background = Brushes.Chartreuse;
                    from.Background = Brushes.Chartreuse;
                }
                title.IsReadOnly = true;
                author.IsReadOnly = true;
                genre.IsReadOnly = true;
                from.IsReadOnly = true;
            }
        }

        private bool deleteObject(int id)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy na pewno?", "Potwierdzenie usunięcia", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                switch (currentState)
                {
                    case 0:
                        bDB.deleteBook(id);
                        break;
                    case 1:
                        oDB.deleteOwnedBook(id);
                        break;
                    case 2:
                        wDB.deleteWhishlistedBook(id);
                        break;
                }
                return true;
            }
            return false;
        }

        void deletionHandler(object sender, RoutedEventArgs e, int id)
        {
          if (deleteObject(id))
            {
                switch (currentState)
                {
                    case 0:
                        fillBooks();
                        break;
                    case 1:
                        fillOwned();
                        break;
                    case 2:
                        fillWhishlisted();
                        break;
                }
            }
        }

        void editionHandler(object sender, RoutedEventArgs e, int id)
        {
            switch (currentState)
            {
                case 0:
                    EditBook editBook = new EditBook(this, id);
                    this.Visibility = Visibility.Hidden;
                    editBook.Show();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        void addHandler(object sender, RoutedEventArgs e)
        {
            switch(currentState)
            {
                case 0:
                    AddBook addBook = new AddBook(this);
                    this.Visibility = Visibility.Hidden;
                    addBook.Show();
                    break;
                case 1:
                    AddOwned addOwned = new AddOwned(this);
                    this.Visibility = Visibility.Hidden;
                    addOwned.Show();
                    break;
                case 2:
                    AddWhishlisted addWhishlisted = new AddWhishlisted(this);
                    this.Visibility = Visibility.Hidden;
                    addWhishlisted.Show();
                    break;
            }
        }

        public static void showError(string message)
        {
            MessageBox.Show(message);
        }

        public void refresh()
        {
            switch (currentState)
            {
                case 0:
                    fillBooks();
                    break;
                case 1:
                    fillOwned();
                    break;
                case 2:
                    fillWhishlisted();
                    break;
            }
        }
    }

}
