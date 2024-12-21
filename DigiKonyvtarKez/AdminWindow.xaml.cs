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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private List<User> Users;
        private List<Book> Books;
        public AdminWindow()
        {
            InitializeComponent();
            if (!File.Exists("users.txt"))
            {
                throw new FileNotFoundException("A felhasználói adatok nem találhatók");
            }
            Users = File.ReadAllLines("users.txt").Select(User.FromString).ToList();
            UserListBox.ItemsSource = Users.Select(u => u.Name);

            if (!File.Exists("books.txt"))
            {
                File.Create("books.txt").Close();
            }
            Books = File.ReadAllLines("books.txt").Select(Book.FromString).ToList();
            BookListBox.ItemsSource = Books.Select(b => b.Title);

        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (UserListBox.SelectedItem != null)
            {
                var selectedUser = UserListBox.SelectedItem.ToString();
                User selected = Users.Where(u => u.Name == selectedUser).First();
                if (selected != null) {
                    if (selected.IsAdmin)
                    {
                        MessageBox.Show("Az adminisztrátort nem lehet törölni", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    bool res = MessageBox.Show("Biztosan törölni szeretné a felhasználót?", "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

                    if (res)
                    {
                        Users.Remove(Users.Where(u => u.Name == selected.Name).First());

                        File.WriteAllLines("users.txt", Users.Select(u => u.ToString()));
                        UserListBox.ItemsSource = Users.Select(u => u.Name);
                    }
                }
            }

        }

        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookListBox.SelectedItem != null)
            {
                var selectedBook = BookListBox.SelectedItem.ToString();
                Book selected = Books.Where(b => b.Title == selectedBook).First();

                BookEditor editor = new BookEditor(selected);
                editor.Show();
                this.Close();
            }
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
