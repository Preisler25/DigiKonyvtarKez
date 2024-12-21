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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private List<Book> Books;
        public UserWindow()
        {
            InitializeComponent();

            if (!File.Exists("books.txt"))
            {
                File.Create("books.txt").Close();
            }
            Books = File.ReadAllLines("books.txt").Select(Book.FromString).ToList();
            BookListBox.ItemsSource = Books.Select(b => b.Title);
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            String title = TitleTextBox.Text;
            String author = AuthorTextBox.Text;
            String year = RealiseDateTextBox.Text;
            String genre = GenreComboBox.Text;

            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(author) || String.IsNullOrEmpty(year) || String.IsNullOrEmpty(genre))
            {
                MessageBox.Show("Minden mező kitöltése kötelező", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int res;
            if (!int.TryParse(year, out res))
            {
                MessageBox.Show("A kiadási év nem szám", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (title.Contains(",") || author.Contains(",") || genre.Contains(","))
            {
                MessageBox.Show("A cím, szerző és a műfaj nem tartalmazhat vesszőt!");
                return;
            }
            if (res < 1800)
            {
                MessageBox.Show("A kiadási év nem lehet 1800 kevesebb", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (res > DateTime.Now.Year)
            {
                MessageBox.Show("A kiadási év nem lehet jövőbeli", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Books.Any(b => b.Title == title))
            {
                MessageBox.Show("A könyv már létezik", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Book b = new Book(title, author, res, genre);

            Books.Add(b);
            using (StreamWriter writer = new StreamWriter("books.txt", true))
            {
                writer.WriteLine(b.ToString());
            }
            BookListBox.ItemsSource = Books.Select(book => book.Title);

            TitleTextBox.Text = "";
            AuthorTextBox.Text = "";
            RealiseDateTextBox.Text = "";
            GenreComboBox.Text = "";
        }
        

        private void DDC_Click(object sender, EventArgs e)
        {
            String genre = BookFilterComboBox.Text;
            if (String.IsNullOrEmpty(genre) || genre == "Összes")
            {
                BookListBox.ItemsSource = Books.Select(b => b.Title);
                return;
            }
            else
            {
                BookListBox.ItemsSource = Books.Where(b => b.Genre == genre).Select(b => b.Title);
                return;
            }
        }
    }
}
