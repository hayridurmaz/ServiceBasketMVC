using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Persistence;

namespace ServiceBasket.Models.Transaction
{
    public class ServiceManager
        
    {      public static bool AddNewService(Service newService)
        {
            // Verify that the service doesn't already exist
            Service oldService = ServicePersistence.GetService(newService.Title);
            // oldService should be null, if this is a new service
            if (oldService != null)
            {
                return false;
            }

            return ServicePersistence.AddService(newService);
        }
        public static bool DeleteService(Service delService)
        {
            // Verify that the service already exists
            Service oldService = ServicePersistence.GetService(delService.Title);
            // oldUser shouldnot be null, if this is a old user
            if (oldService == null)
            {
                return false;
            }
            return ServicePersistence.DeleteService(delService);
        }

        public static bool ChangeService(Service upService)
        {
            // Verify that the service already exists
            Service oldService = ServicePersistence.GetService(upService.Title);
            // oldUser shouldnot be null, if this is a old user
            if (oldService == null)
            {
                return false;
            }
            return ServicePersistence.UpdateService(upService);
        }

    }
}