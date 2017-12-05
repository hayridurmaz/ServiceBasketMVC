using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Entity
{
    public class User
    {
        public String UserId { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Salt { get; set; }
        public String PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int Age { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsProvider { get; set; }

        public User()
        {
            IsActive = true;
        }
    }
}