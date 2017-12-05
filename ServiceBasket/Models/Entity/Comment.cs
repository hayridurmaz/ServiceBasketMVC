using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Entity
{
    public class Comment
    {
        long CommentId { get; set; }
        String Title { get; set; }
        String Content { get; set; }
        User Writer { get; set; }
    }
}