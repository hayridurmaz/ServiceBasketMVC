using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Persistence;
using ServiceBasket.Models.Transaction;

namespace ServiceBasket.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServiceList()
        {
            List<Service> services = ServiceBasket.Models.Persistence.ServicePersistence.GetAllServices();
            return View(services);
        }
        public ActionResult ServiceDetail(String title)
        {
            Service service = ServicePersistence.GetService(title);
            service.Comments = CommentPersistence.getCommentsForaService(service);
            Comment comment = new Comment();

            return View(Tuple.Create(service,comment));
        }

     
    
        /*
        public ActionResult ServiceDetail(String title)
        {
            Service service = ServiceBasket.Models.Persistence.ServicePersistence.GetService(title);
            if (service == null)
            {
                return RedirectToAction("Index", "Service");
            }
                return View(service);
            
        }
        */

        [HttpGet]
        public ActionResult AddService()
        {
            return View(new Service());
        }


        [HttpPost]
        public ActionResult AddService(Service service)
        {
            if (Session["userId"]==null || Session["IsProvider"].Equals(false))
            {
                TempData["serviceAdded"] = "Please Log in.";
                return View(service);
            }
            service.Owner = UserPersistence.GetUser(Session["userId"].ToString());
            service.Comments = CommentPersistence.getCommentsForaService(service);
            service.date = DateTime.Now;
            
            string t = service.Description.Replace("<", "&lt");
            string t1 = t.Replace(">", "&gt");
            string t2 = t1.Replace("(", "&#40");
            string t3 = t2.Replace(")", "&#41");
            string t4 = t3.Replace("&", "&#38");
            string tfinal = t4.Replace("|", "&#124");
            service.Description = tfinal;
            bool? acceptible = false; 
            acceptible = ServiceManager.AddNewService(service);
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

        [HttpGet]
        public ActionResult AddComment(Service service)
        {
            Comment comment = new Comment();
            comment.Service = ServicePersistence.GetService(service.Title);
            return View("ServiceDetail");
        }

        [HttpPost]
        public ActionResult AddComment(Service service, Comment comment)
        {
            if (Session["userId"] == null)
            {
                TempData["commentAdded"] = "Please Log in.";
                return View(comment);
            }
            comment.Writer = UserPersistence.GetUser(Session["userId"].ToString());
            comment.CommentId = CommentPersistence.getMaxId() + 1;
            comment.Service = ServicePersistence.GetService(service.Title);
            bool? acceptible = false;
            acceptible = CommentManager.AddNewComment(comment);
            if ((acceptible != null))
            {

                if (acceptible == true)
                {
                    TempData["commentAdded"] = "Comment is added successfully.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["commenteAdded"] = "Comment could not be added.";
                    return View(comment);
                }
            }
            else
            {
                TempData["commentAdded"] = "Comment could not be added.";
                return View(comment);
            }
        }

    }
}