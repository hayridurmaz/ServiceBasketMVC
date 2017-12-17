using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Repository;

namespace ServiceBasket.Models.Persistence
{
    public class ServicePersistence
    {
        static List<Service> services;
        static ServicePersistence()
        {
            services = new List<Service>();
            String sql = "select * from SERVICE";
            List<object[]> rows = RepositoryManager.Repository.DoQuery(sql);

            foreach (object[] dataRow in rows)
            {
                DateTime addDate = Convert.ToDateTime(dataRow[5]);
                Service service = new Service
                {
                    ServiceId = (Guid)dataRow[0],
                    Title = (string)dataRow[1],
                    Description = (string)dataRow[2],
                    Owner = UserPersistence.GetUser(dataRow[3].ToString()),
                    Category = (string)dataRow[4],
                    Comments = null,//will change!!!!!
                    date = addDate
                };
                services.Add(service);
            }
        }

        public static bool AddService(Service service)
        {
            //System.Diagnostics.Debug.WriteLine("DateTime: " + service.RegisterDate.ToString("yyyy-MM-dd"));
            //int isadmin = 0, isactive = 0, isprovider = 0;

            string sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('"
                + service.Title + "', '"
                + service.Description + "', '"
                + service.Owner.UserId + "', '"
                + service.Category + "', '"
                + service.date.ToString("yyyy-MM-dd") + "')";


            int returned = RepositoryManager.Repository.DoCommand(sql);
            System.Diagnostics.Debug.WriteLine("returned: " + returned);
            if (GetService(service.ServiceId) == null)
            {
                services.Add(service);
            }
            if (returned < 0)
            {
                return false;
            }
            return true;
        }
        public static Service GetService(Guid serviceId)
        {
            new ServicePersistence();
            foreach (Service service in services)
            {
                //System.Diagnostics.Debug.WriteLine("userID:: " + service.ServiceId);
                if (serviceId == service.ServiceId)
                {
                    return service;
                }
            }
            return null;
        }

        public static List<Service> GetAllServices()
        {
            return services;
        }

        // Not Implemented
        public static bool UpdateService(Service service)
        {
            return false;
        }
        public static bool DeleteService(Service service)
        {
            return false;
        }

    }
}