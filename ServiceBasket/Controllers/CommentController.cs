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
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View("AddComment");
        }

        [HttpGet]
        public ActionResult AddComment()
        {
            return View(new Comment());
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            if (Session["userId"] == null)
            {
                TempData["commentAdded"] = "Please Log in.";
                return View(comment);
            }
            comment.Writer = UserPersistence.GetUser(Session["userId"].ToString());
            comment.CommentId = CommentPersistence.getMaxId() + 1;

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