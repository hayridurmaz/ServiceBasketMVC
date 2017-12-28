﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Transaction;
using ServiceBasket.Models.Persistence;

namespace ServiceBasket.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            User user = ServiceBasket.Models.Persistence.UserPersistence.GetUser(Session["userId"].ToString());
            return View(user);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            return View(UserPersistence.GetUser(Session["UserId"].ToString()));
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            UserManager.ChangeUser(user);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult UserList()
        {
            List<User> users = ServiceBasket.Models.Persistence.UserPersistence.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.Title = "SignUp";
            return View(new RegisterCredential());
        }

        [HttpPost]
        public ActionResult SignUp(RegisterCredential registerCredential)
        {
            ViewBag.Title = "SignUp";
            // Validate book data from the transaction
            if (registerCredential == null)
            {
                TempData["signupMessage"] = "Error: Invalid Request - please try again";
                return View(new RegisterCredential());
            }
            if (registerCredential.Name == null || registerCredential.Name.Length == 0)
            {
                TempData["signupMessage"] = "Error: Name is required";
                return View(registerCredential);
            }
            if (registerCredential.Email == null || registerCredential.Email.Length==0 || !registerCredential.Email.Contains("@"))
            {
                TempData["signupMessage"] = "Error: Please type a valid mail";
                return View(registerCredential);
            }
            if (registerCredential.Password == null)
            {
                TempData["signupMessage"] = "Error: Please type a valid password";
                return View(registerCredential);
            }

            // Create the user
            String salt = EncryptionManager.PasswordSalt;
            System.Diagnostics.Debug.WriteLine("signup passhash: " + EncryptionManager.EncodePassword(registerCredential.Password, salt));
            System.Diagnostics.Debug.WriteLine("signup salt: " + salt);

            User user = new User
            {
                UserId = registerCredential.UserId,
                Salt = salt,
                PasswordHash = EncryptionManager.EncodePassword(registerCredential.Password, salt),
                Name = registerCredential.Name,
                Email = registerCredential.Email,
                IsAdmin = false,
                IsActive = true,
                RegisterDate = DateTime.Now,
                Age = registerCredential.Age,
                IsProvider = registerCredential.IsProvider
            };
            System.Diagnostics.Debug.WriteLine("user passhash: " + user.PasswordHash);
            System.Diagnostics.Debug.WriteLine("user salt: " + user.Salt);
            //Add user

            bool result = UserManager.AddNewUser(user);
            if (result)
            {
                TempData["signupMessage"] = "";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                TempData["signupMessage"] = "User name " +registerCredential.UserId+" already exists, try again";
                return View(registerCredential);
            }

            
        }

        
        [HttpGet]
        public ActionResult Login()
        {
            return View(new Credential());
        }

        
        [HttpPost]
        public ActionResult Login(Credential credential)
        {
            if (credential == null)
            {
                return View(new Credential());

            }
            if (credential.UserId == null || credential.UserId.Length == 0 || credential.Password == null || credential.Password.Length == 0)
            {
                TempData["loginMessage"] = "Re-enter User Id and Password without blank fields.";
                return View(credential);
            }
            bool accaptable = UserManager.AuthenticateUser(credential, Session);
            if (accaptable)
            {
                TempData["loginMessage"] = "Login Successfully";
                Session["userId"] = credential.UserId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["loginMessage"] = "Invalid login attempt";
                return View(credential);
            }
            //return View(credential);
        }

        public ActionResult Logout()
        {
            UserManager.LogoutUser(Session);
            TempData["loginMessage"] = "You are logged out";
            return View();
            
        }
    }
}