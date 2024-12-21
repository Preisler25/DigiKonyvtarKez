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
using System.IO;

namespace DigiKonyvtarKez
{
    /// <summary>
    /// Interaction logic for UserEditor.xaml
    /// </summary>
    public partial class BookEditor : Window
    {
        List<Book> Books;
        Book Selected;
        public BookEditor(Book book)
        {
            InitializeComponent();
            if (!File.Exists("books.txt"))
            {
                throw new FileNotFoundException("A könyv adatok nem találhatók");
            }
            Books = File.ReadAllLines("books.txt").Select(Book.FromString).ToList();
            Selected = book;

            BookTitleTextBox.Text = Selected.Title;
            AuthorTextBox.Text = Selected.Author;
            YearTextBox.Text = Selected.Year.ToString();
            GenreComboBox.Text = Selected.Genre;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            String title = BookTitleTextBox.Text;
            String author = AuthorTextBox.Text;
            String year = YearTextBox.Text;
            String genre = GenreComboBox.Text;

            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(author) || String.IsNullOrEmpty(year) || String.IsNullOrEmpty(genre))
            {
                MessageBox.Show("Nem adott meg minden adatot!");
                return;
            }
            if (title.Contains(",") || author.Contains(",") || genre.Contains(","))
            {
                MessageBox.Show("A cím, szerző és a műfaj nem tartalmazhat vesszőt!");
                return;
            }
            int res1;
            if (!int.TryParse(year, out res1))
            {
                MessageBox.Show("A kiadási év nem szám!");
                return;
            }
            if (res1 < 1800)
            {
                MessageBox.Show("A kiadási év nem lehet 1800 kevesebb", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (res1 > DateTime.Now.Year)
            {
                MessageBox.Show("A kiadási év nem lehet jövőbeli", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Selected.Title != title && Books.Any(b => b.Title == title))
            {
                MessageBox.Show("A könyv már létezik", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Books.Where(b => b.Title == Selected.Title).First().Author = author;
            Books.Where(b => b.Title == Selected.Title).First().Year = res1;
            Books.Where(b => b.Title == Selected.Title).First().Genre = genre;
            Books.Where(b => b.Title == Selected.Title).First().Title = title;

            File.WriteAllLines("books.txt", Books.Select(b => b.ToString()));

            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();

            this.Close();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Books.Remove(Selected);

            Books.Remove(Books.Where(b => b.Title == Selected.Title).First());

            File.WriteAllLines("books.txt", Books.Select(b => b.ToString()));

            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }
}
