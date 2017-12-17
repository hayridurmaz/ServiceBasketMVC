using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBasket.Models.Entity
{
    public class Service
    {
     
        
        //public int ServiceId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Category { get; set; }
        public User Owner { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime date { get; set; }
        public Service()
        {
            Comments = new List<Comment>();
        }
    }
}