using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceBasket.Models.Entity;
using ServiceBasket.Models.Repository;

namespace ServiceBasket.Models.Persistence
{
    public class CommentPersistence
    {
        static List<Comment> comments;
        static long ServiceId = 1;

        static CommentPersistence()
        {
            comments = new List<Comment>();
            String sql = "select * from COMMENT";
            List<object[]> rows = RepositoryManager.Repository.DoQuery(sql);

            foreach (object[] dataRow in rows)
            {
                
                Comment comment = new Comment
                {

                    CommentId = (long)dataRow[0],
                    Title = (string)dataRow[1],
                    Content = (string)dataRow[2],
                    Writer = UserPersistence.GetUser((String)dataRow[3]),
                    Service = ServicePersistence.GetService((String)dataRow[4])
                };
                comments.Add(comment);
            }
        }
        public static int getMaxId()
        {
            String sql = "Select * from COMMENT ORDER BY CommentId DESC LIMIT 1";
            List<object[]> rows = RepositoryManager.Repository.DoQuery(sql);
            if (rows.ElementAt(0) == null)
            {
                return 1;
            }
            object[] aRow = rows.ElementAt(0);
            return (int)aRow[0];
        }

        public static bool AddComment(Comment comment)
        {
            //System.Diagnostics.Debug.WriteLine("DateTime: " + service.RegisterDate.ToString("yyyy-MM-dd"));
            //int isadmin = 0, isactive = 0, isprovider = 0;
            int id = getMaxId() + 1;
            string sql = "insert into COMMENT (CommentId, Title, Content, WriterUserId, ServiceTitle) values ('"
                + id + "', '"
                + comment.Title + "', '"
                + comment.Content + "', '"
                + comment.Writer.UserId + "', '"
                + comment.Service.Title + "')";


            int returned = RepositoryManager.Repository.DoCommand(sql);
            System.Diagnostics.Debug.WriteLine("returned: " + returned);
            if (GetComment(comment.CommentId) == null)
            {
                comments.Add(comment);
            }
            if (returned < 0)
            {
                return false;
            }
            return true;
        }

        public static Comment GetComment(long id)
        {
            new ServicePersistence();
            foreach (Comment comment in comments)
            {
                System.Diagnostics.Debug.WriteLine("userID:: " + comment.CommentId);
                if (id==comment.CommentId)
                {
                    return comment;
                }
            }
            return null;
        }

        public static List<Comment> getCommentsForaService(Service service)
        {
            List<Comment> coms = new List<Comment>();
            foreach(Comment com in comments)
            {
                if (com.Service.Equals(service)){
                    coms.Add(com);
                }
            }
            return coms;
        }

        public static List<Comment> GetAllComments()
        {
            return comments;
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