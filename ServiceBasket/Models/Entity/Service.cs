using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Entity
{
    public class Service
    {
        long ServiceId { get; set; }
        String Title { get; set; }
        String Description { get; set; }
        User Owner { get; set; }
        List<Comment> Comments { get; set; }
        Service()
        {
            Comments = new List<Comment>();
        }
    }
}