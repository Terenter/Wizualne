using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Helpers
{
    class BookDbHelper
    {
        String strBooksBase = "books.txt";
        public static int lastId = -1;

        public int addBook(Models.Book cBook)
        {
            cBook.Id = ++lastId;
            String strBook = Newtonsoft.Json.JsonConvert.SerializeObject(cBook);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(strBooksBase, true, Encoding.Unicode))
            {
                file.WriteLine(strBook);
            }
            return cBook.Id;
        }

        public Models.Book getBook(int nId)
        {
            List<Models.Book> books = getBooks();
            for(var enu = books.GetEnumerator(); enu.MoveNext(); )
            {
                if (enu.Current.Id == nId) return enu.Current;
            }
            return null;
        }

        public List<Models.Book> getBooks()
        {
            List<Models.Book> books = new List<Models.Book>();
            string[] lines = System.IO.File.ReadAllLines(strBooksBase);
            foreach (string line in lines)
            {
                try
                {
                    Models.Book book = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Book>(line);
                    if (book.Id > lastId) lastId = book.Id;
                    books.Add(book);
                }
                catch(Exception ex)
                {
                    MainWindow.showError(ex.Message);
                }
            }
            return books;
        }

        public void deleteBook(int nId)
        {
            List<Models.Book> books = getBooks();
            Models.Book toRem = null;
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId) toRem = enu.Current;
            }
            if (toRem != null)
            {
                books.Remove(toRem);
            }
            List<String> jsons = new List<string>();
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                jsons.Add(Newtonsoft.Json.JsonConvert.SerializeObject(enu.Current));
            }
            String[] lines = jsons.ToArray<string>();
            System.IO.File.WriteAllLines(strBooksBase, lines, Encoding.Unicode);
        }

        public void updateBook(int nId, Models.Book cBook)
        {
            List<Models.Book> books = getBooks();
            int id = -1;
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId)
                {
                    id = books.IndexOf(enu.Current);
                }
            }
            if (id != -1)
            {
                books.RemoveAt(id);
                books.Insert(id, cBook);
            }
            List<String> jsons = new List<string>();
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                jsons.Add(Newtonsoft.Json.JsonConvert.SerializeObject(enu.Current));
            }
            String[] lines = jsons.ToArray<string>();
            System.IO.File.WriteAllLines(strBooksBase, lines, Encoding.Unicode);
        }
    }
}
