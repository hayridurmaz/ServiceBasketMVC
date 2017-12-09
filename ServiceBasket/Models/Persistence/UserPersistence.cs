using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Repository;
using ServiceBasket.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Persistence
{
    public class UserPersistence
    {
        private static List<User> users;//DATABASE???

        static UserPersistence()
        {
            users = new List<User>();

            /*string salt = EncryptionManager.PasswordSalt;
            users.Add(new User
            {
                UserId = "user1",
                Name = "Alpha Romeo",
                Salt = salt,
                PasswordHash = EncryptionManager.EncodePassword("abc123", salt),
                IsAdmin = false
            });

            salt = EncryptionManager.PasswordSalt;
            users.Add(new User
            {
                UserId = "admin1",
                Name = "Charlie Eagle",
                Salt = salt,
                PasswordHash = EncryptionManager.EncodePassword("abcd1234", salt),
                IsAdmin = true
            });*/
            String sql = "select * from user";
            List<object[]> rows = RepositoryManager.Repository.DoQuery(sql);

            foreach (object[] dataRow in rows)
            {
                DateTime regDate = Convert.ToDateTime(dataRow[7]);
                bool admin = false, provider = false, active = false;
                if ((int)dataRow[5] == 1)
                {
                    admin = true;
                }
                if ((int)dataRow[6] == 1)
                {
                    active = true;
                }
                if ((int)dataRow[8] == 1)
                {
                    provider = true;
                }
                User user = new User
                {
                    UserId = (string)dataRow[0],
                    Name = (string)dataRow[1],
                    Email = (string)dataRow[2],
                    Salt = (string)dataRow[3],
                    PasswordHash = (string)dataRow[4],
                    IsAdmin = admin,
                    IsActive = active,
                    RegisterDate = regDate,
                    IsProvider = provider,
                    Age=(int)dataRow[9]
                };
                users.Add(user);
            }
        }
        /*
         * Add a Book to the database.
         * Return true iff the add succeeds.
         */
        public static bool AddUser(User user)
        {
            System.Diagnostics.Debug.WriteLine("DateTime: " + user.RegisterDate.ToString("yyyy-MM-dd"));
            int isadmin=0, isactive=0, isprovider=0;
            if (user.IsActive)
            {
                isactive = 1;
            }
            if (user.IsAdmin)
            {
                isadmin = 1;
            }
            if (user.IsProvider)
            {
                isprovider = 1;
            }
            

            string sql = "insert into USER (UserId, Name, Email, Salt, PasswordHash, IsAdmin, IsActive, RegisterDate, IsProvider, Age) values ('"
                + user.UserId + "', '"
                + user.Name + "', '"
                + user.Email +"', '"
                + user.Salt +"', '"
                + user.PasswordHash +"', "
                + isadmin + ", "
                + isactive + ", '"
                + user.RegisterDate.ToString("yyyy-MM-dd") + "', "
                + isprovider +", '"
                + user.Age+"')";
            

            RepositoryManager.Repository.DoCommand(sql);
            if (GetUser(user.UserId) == null)
            {
                users.Add(user);
            }
            return true;
        }

        /*
         * Get one user from the repository, identified by userId
         */
        public static User GetUser(string userId)
        {
            new UserPersistence();
            foreach (User user in users)
            {
                System.Diagnostics.Debug.WriteLine("userID:: "+user.UserId);
                if (userId == user.UserId)
                {
                    return user;
                }
            }
            return null;
        }

        public static List<User> GetAllUsers()
        {
            return users;
        }

        // Not Implemented
        public static bool UpdateUser(User user)
        {
            return false;
        }
        public static bool DeleteUser(User user)
        {
            return false;
        }
        
    }
}