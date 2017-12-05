using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Repository
{
    public class RepositoryManager
    {
     /*
      * This class instantiates an instance of IRepository, and provides
      * it to Persistence classes as needed.
      */
        public static IRepository Repository { get; set; }

        /*
         * Create an instance of a concrete Repository and open it. 
         * The Repository should close itself on shutdown.
         */
        static RepositoryManager()
        {
            Repository = new SqliteRepository();
            Repository.Open();
            Repository.Initialize();
        }
    }
}