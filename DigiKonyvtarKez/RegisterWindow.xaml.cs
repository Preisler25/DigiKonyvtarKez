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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private List<User> Users;
        public RegisterWindow()
        {
            InitializeComponent();
            if (!File.Exists("users.txt"))
            {
                File.Create("users.txt").Close();
            }
            Users = File.ReadAllLines("users.txt").Select(User.FromString).ToList();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Users.Count == 0)
            {
                MessageBox.Show("Nincs még regisztrált felhasználó!");
                return;
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            String name = UsernameTextBox.Text;
            String password = PasswordTextBox.Password;
            String password2 = Password2TextBox.Password;
            String email = EmailTextBox.Text;

            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password2) || String.IsNullOrEmpty(email))
            {
                MessageBox.Show("Nem adott meg minden adatot!");
                return;
            }
            if (name.Contains(",") || password.Contains(",") || email.Contains(","))
            {
                MessageBox.Show("A felhasználónév, jelszó és az email nem tartalmazhat vesszőt!");
                return;
            }
            if (name.Length < 3 || password.Length < 8)
            {
                MessageBox.Show("A felhasználónév (3) és a jelszó (8) karakter hosszú kell legyen!");
                return;
            }
            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper) || !password.Any(char.IsLower))
            {
                MessageBox.Show("A jelszónak tartalmaznia kell legalább egy számot, egy nagybetűt és egy kisbetűt!");
                return;
            }
            if (password != password2)
            {
                MessageBox.Show("A két jelszó nem egyezik meg!");
                return;
            }
            if (Users.Any(u => u.Name == name))
            {
                MessageBox.Show("A felhasználónév már foglalt!");
                return;
            }
            if (Users.Any(u => u.Email == email))
            {
                MessageBox.Show("Az email cím már foglalt!");
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Az email cím formátuma nem megfelelő!");
                return;
            }
            User u;

            if (Users.Count == 0)
            {
                u = new User(name, password, email, true);
            }
            else
            {
                u = new User(name, password, email, false);
            }

            using (StreamWriter writer = new StreamWriter("users.txt", true))
            {
                writer.WriteLine(u.ToString());
            }

            MessageBox.Show("Sikeres regisztráció!", "Regisztráció", MessageBoxButton.OK,
                MessageBoxImage.Information);

            if ((bool)u.IsAdmin)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
            else
            {
                UserWindow userWindow = new UserWindow();
                userWindow.Show();
                this.Close();
            }
        }
    }
}
