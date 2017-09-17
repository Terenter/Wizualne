using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Helpers
{
    class WhislistedDbHelper
    {
        String strWhishlistedBooksBase = "whishlisted_books.txt";
        public static int lastId = -1;

        public void addWhishlistedBook(Models.WhishlistedBook cBook)
        {
            cBook.Id = ++lastId;
            String strBook = Newtonsoft.Json.JsonConvert.SerializeObject(cBook);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(strWhishlistedBooksBase, true, Encoding.Unicode))
            {
                file.WriteLine(strBook);
            }
        }

        public Models.WhishlistedBook getWhishlistedBook(int nId)
        {
            List<Models.WhishlistedBook> books = getWhishlistedBook();
            for (var enu = books.GetEnumerator(); enu.MoveNext();)
            {
                if (enu.Current.Id == nId)
                {
                    Models.GetWhislistedBook retBook = new Models.GetWhislistedBook();
                    retBook.Whishlisted = enu.Current.Whishlisted;
                    retBook.BookId = enu.Current.BookId;
                    retBook.Id = enu.Current.Id;
                    retBook.UserId = enu.Current.UserId;
                    retBook.cBook = new BookDbHelper().getBook(retBook.BookId);
                    return retBook;
                }
            }
            return null;
        }

        public List<Models.WhishlistedBook> getWhishlistedBook()
        {
            List<Models.WhishlistedBook> books = new List<Models.WhishlistedBook>();
            string[] lines = System.IO.File.ReadAllLines(strWhishlistedBooksBase);
            foreach (string line in lines)
            {
                try
                {
                    Models.WhishlistedBook book = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.WhishlistedBook>(line);
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

        public List<Models.GetWhislistedBook> getWhishlistedBooks()
        {
            List<Models.GetWhislistedBook> books = new List<Models.GetWhislistedBook>();
            string[] lines = System.IO.File.ReadAllLines(strWhishlistedBooksBase);
            foreach (string line in lines)
            {
                try
                {
                    Models.WhishlistedBook book = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.WhishlistedBook>(line);
                    if (book.Id > lastId) lastId = book.Id;
                    Models.GetWhislistedBook retBook = new Models.GetWhislistedBook();
                    retBook.Whishlisted = book.Whishlisted;
                    retBook.BookId = book.BookId;
                    retBook.Id = book.Id;
                    retBook.UserId = book.UserId;
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

        public void deleteWhishlistedBook(int nId)
        {
            List<Models.WhishlistedBook> books = getWhishlistedBook();
            Models.WhishlistedBook toRem = null;
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
            System.IO.File.WriteAllLines(strWhishlistedBooksBase, lines, Encoding.Unicode);
        }

        public void updateWhishlistedBook(int nId, Models.WhishlistedBook cBook)
        {
            List<Models.WhishlistedBook> books = getWhishlistedBook();
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
            System.IO.File.WriteAllLines(strWhishlistedBooksBase, lines, Encoding.Unicode);
        }
    }
}
