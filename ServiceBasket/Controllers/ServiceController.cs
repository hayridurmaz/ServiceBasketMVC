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
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult AddService()
        {
            return View(new Service());
        }


        [HttpPost]
        public ActionResult AddService(Service service)
        {
            if (Session["userId"]==null)
            {
                TempData["serviceAdded"] = "Please Log in.";
                return View(service);
            }
            service.Owner = UserPersistence.GetUser(Session["userId"].ToString());
            service.Comments = null;
            service.date = DateTime.Now;
            bool acceptible = ServicePersistence.AddService(service);
            if ((acceptible!=null))
            {

                if (acceptible==true)
                {
                    TempData["serviceAdded"] = "Service is added successfully.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["serviceAdded"] = "Service could not be added.";
                    return View(service);
                }
            }
            else
            {
                TempData["serviceAdded"] = "Service could not be added.";
                return View(service);
            }
            
        }
    }
}