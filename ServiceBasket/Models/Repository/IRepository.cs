using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBasket.Models.Repository
{
    public interface IRepository
    {
        bool IsOpen { get; }

        /* Initialize the database */
        bool Initialize();

        /* Open the database */
        bool Open();

        /* Close the database */
        void Close();

        /* Execute an SQL command */
        int DoCommand(string sqlCommand);

        /* Execute an SQL query */
        List<object[]> DoQuery(string sqlQuery);
    }
}
