using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DigiKonyvtarKez
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }

        public User(string name, string password, string email, bool isAdmin)
        {
            Name = name;
            Password = password;
            Email = email;
            IsAdmin = isAdmin;
        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public override string ToString()
        {
            return $"{Name},{Password},{Email},{IsAdmin}";
        }

        public static User FromString(string userData)
        {
            var parts = userData.Split(",");
            if (parts.Length != 4)
                throw new FormatException("A felhasználói adatok formátuma nem megfelelő");
            bool res1;
            if (!bool.TryParse(parts[3], out res1))
                throw new FormatException("isAndmin nem bool");

            return new User(parts[0], parts[1], parts[2], res1);
        }
    }
}
