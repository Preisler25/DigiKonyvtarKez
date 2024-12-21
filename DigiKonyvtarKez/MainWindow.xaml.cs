using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DigiKonyvtarKez
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User> Users;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("users.txt"))
            {
                File.Create("users.txt").Close();
            }
            Users = File.ReadAllLines("users.txt").Select(User.FromString).ToList();
            if (Users.Count == 0)
            {
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.Show();
                this.Close();
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            String name = UsernameTextBox.Text;
            String password = PasswordTextBox.Password;

            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Nem adott meg felhasználónevet vagy jelszót!");
                return;
            }


            User user = new User(name, password);

            if (Users.Any(u => u.Name == name && u.Password == password))
            {
                User loggedInUser = Users.First(u => u.Name == name && u.Password == password);
                if (loggedInUser.IsAdmin)
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
            else
            {
                MessageBox.Show("Sikertelen bejelentkezés!");
            }

        }

        private void GoToReg_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}