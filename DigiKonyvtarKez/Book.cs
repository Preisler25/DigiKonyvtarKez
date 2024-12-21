using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigiKonyvtarKez
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        public Book(string title, string author, int year, string genre)
        {
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
        }

        public override string ToString()
        {
            return $"{Title},{Author},{Year},{Genre}";
        }

        public static Book FromString(string bookData)
        {
            var parts = bookData.Split(",");
            if (parts.Length != 4)
                throw new FormatException("A könyv adatainak formátuma nem megfelelő");
            int res1;
            if (!int.TryParse(parts[2], out res1))
                throw new FormatException("A könyv kiadási éve nem szám");
            return new Book(parts[0], parts[1], res1, parts[3]);
        }
    }
}
