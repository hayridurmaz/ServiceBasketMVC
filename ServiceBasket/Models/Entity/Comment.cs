using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Entity
{
    public class Comment
    {
        public long CommentId { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public User Writer { get; set; }
        public Service Service { get; set; }

        public Comment()
        {

        }

    }
}