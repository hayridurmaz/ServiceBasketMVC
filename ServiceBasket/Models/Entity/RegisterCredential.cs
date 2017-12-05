using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Entity
{
    public class RegisterCredential
    {
        public String UserId { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public int Age { get; set; }
        public bool IsProvider { get; set; }

        public RegisterCredential()
        {

        }
    }
}