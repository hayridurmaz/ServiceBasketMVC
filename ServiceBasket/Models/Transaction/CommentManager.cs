using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Persistence;

namespace ServiceBasket.Models.Transaction
{
    public class CommentManager
    {

        public static bool AddNewComment(Comment newComment)
        {
            // Verify that the service doesn't already exist
            Comment oldComment = CommentPersistence.GetComment(newComment.CommentId);
            // oldService should be null, if this is a new service
            if (oldComment != null)
            {
                return false;
            }

            return CommentPersistence.AddComment(newComment);
        }
        /* NOT IMPLEMENTED YET
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
        }*/

    }
}