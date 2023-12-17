using System.Data.Common;

namespace Inventory.Entities
{
    public class User
    {

        public User(string Email, string Name)
        {   
            this.Email = Email;
            this.Name = Name;

            DateCreated = DateTime.Now;
            Active = true;
        }
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        //Siguientes dos campos son para la autenticacion para encriptar el password y despues poder hacer la validacion de la misma :v
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}