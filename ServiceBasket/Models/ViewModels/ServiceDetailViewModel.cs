using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;

namespace ServiceBasket.Models.ViewModels
{
    public class ServiceDetailViewModel
    {
        public IEnumerable<Service> Serv { get; set; }
        public IEnumerable<Comment> Com { get; set; }
    }
}