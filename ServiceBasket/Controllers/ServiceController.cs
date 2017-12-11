using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Persistence;

namespace ServiceBasket.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddService()
        {
            return View(new Service());
        }


        [HttpPost]
        public ActionResult AddService(Service service)
        {
            service.Owner = UserPersistence.GetUser(Session["userId"].ToString());
            return View();
        }
    }
}