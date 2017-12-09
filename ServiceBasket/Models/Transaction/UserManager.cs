using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Persistence;

namespace ServiceBasket.Models.Transaction
{
    public class UserManager
    {
        public static bool AuthenticateUser(Credential credential, HttpSessionStateBase httpSessionStateBase)
        {
            httpSessionStateBase["LoggedIn"] = false;//comment1
            httpSessionStateBase["IsAdmin"] = false;
            httpSessionStateBase["IsProvider"] = false;
            User user = UserPersistence.GetUser(credential.UserId);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("Nulllll user ");

                return false;
            }
            String pHash = EncryptionManager.EncodePassword(credential.Password, user.Salt);
            System.Diagnostics.Debug.WriteLine("realOne: " + EncryptionManager.EncodePassword("sa", user.Salt));
            System.Diagnostics.Debug.WriteLine("phash: "+pHash);
            System.Diagnostics.Debug.WriteLine("user passsHash: "+user.PasswordHash);
            if (pHash != user.PasswordHash)
            {
                return false;
            }
            else
            {
                httpSessionStateBase["LoggedIn"] = true;
                httpSessionStateBase["IsAdmin"] = user.IsAdmin;
                httpSessionStateBase["IsProvider"] = user.IsProvider;
                return true;
            }


        }
        public static void LogoutUser(HttpSessionStateBase httpSessionStateBase)
        {
            httpSessionStateBase["LoggedIn"] = false;
            httpSessionStateBase["IsAdmin"] = false;
            httpSessionStateBase["IsProvider"] = false;
        }
        /*
         * Transaction: Add a new user to the database
         * Returns true iff the new user has a unique userId
         * and it was successfully added.
         */
        public static bool AddNewUser(User newUser)
        {
            // Verify that the book doesn't already exist
            User oldUser = UserPersistence.GetUser(newUser.UserId);
            // oldBook should be null, if this is a new book
            if (oldUser != null)
            {
                return false;
            }

            // set tomorrow as the official date added
            newUser.RegisterDate = DateTime.Now;
            newUser.RegisterDate.AddDays(1);

            return UserPersistence.AddUser(newUser);
        }
        /*
         * Transaction: Delete a book from the database
         * Returns true iff the book exists in the database and
         * it was successfully deleted.
         */
        public static bool DeleteUser(User delUser)
        {
            // Verify that the user already exists
            User oldUser = UserPersistence.GetUser(delUser.UserId);
            // oldUser shouldnot be null, if this is a old user
            if (oldUser == null)
            {
                return false;
            }
            return UserPersistence.DeleteUser(delUser);
        }


        /*
         * Transaction: Update a user in the database
         * Returns true iff the user exists in the database and
         * it was successfully changed.
         */
        public static bool ChangeUser(User upUser)
        {
            // Verify that the user  already exists
            User oldUser = UserPersistence.GetUser(upUser.UserId);
            // oldUser should not be null, if this is a existing user
            if (oldUser == null)
            {
                return false;
            }
            return UserPersistence.UpdateUser(upUser);
        }
    }
}