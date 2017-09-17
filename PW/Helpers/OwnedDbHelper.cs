using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Helpers
{
    class OwnedDbHelper
    {
        String strOwnedBooksBase = "owned_books.txt";
        public static int lastId = -1;

        public void addOwnedBook(Models.OwnedBook cBook)
        {
            cBook.Id = ++lastId;
            String strBook = Newtonsoft.Json.JsonConvert.SerializeObject(cBook);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(strOwnedBooksBase, true, Encoding.Unicode))
            {
                file.WriteLine(strBook);
            }
        }

        public Models.OwnedBook getOwnedBook(int nId)
        {
            List<Models.OwnedBook> books = getOwnedBook();
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId)
                {
                    Models.GetOwnedBook retBook = new Models.GetOwnedBook();
                    retBook.Aquired = enu.Current.Aquired;
                    retBook.BookId = enu.Current.BookId;
                    retBook.Id = enu.Current.Id;
                    retBook.UserId = enu.Current.UserId;
                    retBook.Favorite = enu.Current.Favorite;
                    retBook.cBook = new BookDbHelper().getBook(retBook.BookId);
                    return retBook;
                }
            }
            return null;
        }

        public List<Models.OwnedBook> getOwnedBook()
        {
            List<Models.OwnedBook> books = new List<Models.OwnedBook>();
            string[] lines = System.IO.File.ReadAllLines(strOwnedBooksBase);
            foreach (string line in lines)
            {
                try
                {
                    Models.OwnedBook book = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OwnedBook>(line);
                    if (book.Id > lastId) lastId = book.Id;
                    books.Add(book);
                }
                catch (Exception ex)
                {
                    MainWindow.showError(ex.Message);
                }
            }
            return books;
        }

        public List<Models.GetOwnedBook> getOwnedBooks()
        {
            List<Models.GetOwnedBook> books = new List<Models.GetOwnedBook>();
            string[] lines = System.IO.File.ReadAllLines(strOwnedBooksBase);
            foreach (string line in lines)
            {
                try
                {
                    Models.OwnedBook book = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OwnedBook>(line);
                    if (book.Id > lastId) lastId = book.Id;
                    Models.GetOwnedBook retBook = new Models.GetOwnedBook();
                    retBook.Aquired = book.Aquired;
                    retBook.BookId = book.BookId;
                    retBook.Id = book.Id;
                    retBook.UserId = book.UserId;
                    retBook.Favorite = book.Favorite;
                    retBook.cBook = new BookDbHelper().getBook(retBook.BookId);
                    books.Add(retBook);
                }
                catch (Exception ex)
                {
                    MainWindow.showError(ex.Message);
                }
            }
            return books;
        }

        public void deleteOwnedBook(int nId)
        {
            List<Models.OwnedBook> books = getOwnedBook();
            Models.OwnedBook toRem = null;
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId) toRem = enu.Current;
            }
            if(toRem != null)
            {
                books.Remove(toRem);
            }
            List<String> jsons = new List<string>();
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                jsons.Add(Newtonsoft.Json.JsonConvert.SerializeObject(enu.Current));
            }
            String[] lines = jsons.ToArray<string>();
            System.IO.File.WriteAllLines(strOwnedBooksBase, lines, Encoding.Unicode);
        }

        public void updateOwnedBook(int nId, Models.OwnedBook cBook)
        {
            List<Models.OwnedBook> books = getOwnedBook();
            int id = -1;
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId)
                {
                    id = books.IndexOf(enu.Current);
                }
            }
            if(id != -1)
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
            System.IO.File.WriteAllLines(strOwnedBooksBase, lines, Encoding.Unicode);
        }
    }
}
