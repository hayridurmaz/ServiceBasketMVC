using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace ServiceBasket.Models.Repository
{
    public class SqliteRepository : IRepository
    {
        // Location of the database file 
        private string databaseFile = "C:\\DATABASE\\Database.sqlite";//son halinde değişmesi gerekebilir

        private SQLiteConnection dbConnection;

        public bool IsOpen { get { return isOpen; } }
        private bool isOpen = false;
       
        /*
         * When the Repository shuts down, it should close the DB if it's open.
         */
        ~SqliteRepository()
        {
            if (IsOpen)
            {
                Close();
            }
        }

        /*
         * Open the database. Return true iff the open succeeds, or it was
         * already open.
         */
        public bool Open()
        {
            if (IsOpen)
            {
                return true;
            }
            dbConnection =
                new SQLiteConnection("Data Source=" + databaseFile + ";Version=3;");
            if (dbConnection == null) { return false; }
            dbConnection.Open();
            isOpen = true;
            return true;
        }

        /*
         * Close the database, if it's open.
         */
        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }
            isOpen = false;
            dbConnection.Close();
        }

        /*
         * Execute an SQL command. 
         * The return value is the number of rows affected by the command.
         */
        public int DoCommand(string sqlCommand)
        {
            if (!IsOpen)
            {
                return -1;
            }
            SQLiteCommand command = new SQLiteCommand(sqlCommand, dbConnection);
            int result = command.ExecuteNonQuery();
            return result;
        }

        /*
         * Execute an SQL query. 
         * The return value is a List of object arrays, in which each array 
         * represents one row of data returned.
         */
        public List<object[]> DoQuery(string sqlQuery)
        {
            if (!IsOpen)
            {
                return null;
            }
            List<object[]> rows = new List<object[]>();
            SQLiteCommand command = new SQLiteCommand(sqlQuery, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                object[] row = new object[reader.FieldCount];
                reader.GetValues(row);
                rows.Add(row);
            }
            return rows;
        }

        /*
         * Recreate and reinitialize the database.
         * The return value is true iff the initialization succeeds.
         */
        public bool Initialize()
        {
            bool success = true;

            Close();

            try
            {
                SQLiteConnection.CreateFile(databaseFile);
            }
            catch (IOException e)
            {
                success = false;
            }

            bool openResult = Open();
            if (success & openResult)
            {
                string sql = "CREATE TABLE USER (UserId VARCHAR(15), " +
                    "Name VARCHAR(50),Email VARCHAR(50), Salt VARCHAR(50), " +
                    "PasswordHash varchar(50),IsAdmin INT, IsActive INT, " +
                    "RegisterDate DATE, IsProvider INT, Age INT, PRIMARY KEY(UserId))";
                DoCommand(sql);
                sql = "insert into USER (UserId, Name, Email, Salt, PasswordHash, IsAdmin, IsActive, RegisterDate, IsProvider, Age) values ('admin', 'admin', 'admin@servicebasket', 'zu080TcKlSws6gIl96do125gV9Xluqs3T5OTJGYdrxI=', 'xF/4mXi/eTz3usjtZpzSetJg3G8=', 1, 1, '2017-12-26', 1, '21')";
                DoCommand(sql);

                sql = "CREATE TABLE SERVICE (" +
                    "Title VARCHAR(50) NOT NULL, Description VARCHAR(500), OwnerUserId VARCHAR(50), Category VARCHAR(50), addDate DATE, PRIMARY KEY(Title))";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service1', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi eleifend ullamcorper eros. Maecenas nec quam ac eros consectetur scelerisque accumsan eu enim. Donec eu consequat enim. Etiam tempus enim at viverra egestas. In lacus risus, aliquam eget odio pulvinar, dictum pellentesque leo. Maecenas at dolor eu ante pulvinar vehicula quis vel est. Etiam sed risus magna. Phasellus vel libero ut tellus pulvinar dapibus non sit amet augue. Sed ante ipsum, tincidunt sed molestie a, commodo ac nisi. Curabitur scelerisque mauris ac hendrerit sodales. Pellentesque blandit lectus et posuere gravida. Fusce semper varius elementum. Duis sagittis lorem id mauris auctor, quis cursus justo vulputate. Praesent facilisis risus eu eros suscipit consectetur. Vestibulum odio orci, ullamcorper ultrices aliquet tristique, ultrices vel ipsum. Integer laoreet dapibus laoreet.', 'admin', 'Plumbing', '2017-12-26')";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service2', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam laoreet lectus ac nisi pretium, et sagittis purus aliquet. Vestibulum quis aliquet lectus. Sed rhoncus, quam eu euismod finibus, mi sapien sodales lacus, vitae tempus felis erat eget ex. Integer blandit, lacus sit amet tempus feugiat, nibh enim fringilla felis, dapibus pellentesque ex arcu nec sapien. Sed ultrices nisl sit amet libero vehicula lacinia. Vestibulum hendrerit lorem at tempus interdum. Sed ac odio viverra, porttitor mauris at, dapibus leo. Sed orci nibh, gravida et egestas vel, porttitor non nisl. Suspendisse euismod lectus nisi, eget placerat risus interdum auctor. Integer lobortis felis metus, ac laoreet ligula varius sed. Suspendisse quis rhoncus odio. Morbi bibendum tellus eget felis finibus, nec cursus ligula tincidunt. Suspendisse tempor odio eu molestie rhoncus. Etiam nec enim sit amet purus hendrerit tempor eget non urna. Morbi in sagittis tellus.', 'admin', 'Plumbing', '2017-12-26')";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service3', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam laoreet lectus ac nisi pretium, et sagittis purus aliquet. Vestibulum quis aliquet lectus. Sed rhoncus, quam eu euismod finibus, mi sapien sodales lacus, vitae tempus felis erat eget ex. Integer blandit, lacus sit amet tempus feugiat, nibh enim fringilla felis, dapibus pellentesque ex arcu nec sapien. Sed ultrices nisl sit amet libero vehicula lacinia. Vestibulum hendrerit lorem at tempus interdum. Sed ac odio viverra, porttitor mauris at, dapibus leo. Sed orci nibh, gravida et egestas vel, porttitor non nisl. Suspendisse euismod lectus nisi, eget placerat risus interdum auctor. Integer lobortis felis metus, ac laoreet ligula varius sed. Suspendisse quis rhoncus odio. Morbi bibendum tellus eget felis finibus, nec cursus ligula tincidunt. Suspendisse tempor odio eu molestie rhoncus. Etiam nec enim sit amet purus hendrerit tempor eget non urna. Morbi in sagittis tellus.', 'admin', 'Electrical installation', '2017-12-26')";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service5', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam laoreet lectus ac nisi pretium, et sagittis purus aliquet. Vestibulum quis aliquet lectus. Sed rhoncus, quam eu euismod finibus, mi sapien sodales lacus, vitae tempus felis erat eget ex. Integer blandit, lacus sit amet tempus feugiat, nibh enim fringilla felis, dapibus pellentesque ex arcu nec sapien. Sed ultrices nisl sit amet libero vehicula lacinia. Vestibulum hendrerit lorem at tempus interdum. Sed ac odio viverra, porttitor mauris at, dapibus leo. Sed orci nibh, gravida et egestas vel, porttitor non nisl. Suspendisse euismod lectus nisi, eget placerat risus interdum auctor. Integer lobortis felis metus, ac laoreet ligula varius sed. Suspendisse quis rhoncus odio. Morbi bibendum tellus eget felis finibus, nec cursus ligula tincidunt. Suspendisse tempor odio eu molestie rhoncus. Etiam nec enim sit amet purus hendrerit tempor eget non urna. Morbi in sagittis tellus.', 'admin', 'Cleaner', '2017-12-26')";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service6', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam laoreet lectus ac nisi pretium, et sagittis purus aliquet. Vestibulum quis aliquet lectus. Sed rhoncus, quam eu euismod finibus, mi sapien sodales lacus, vitae tempus felis erat eget ex. Integer blandit, lacus sit amet tempus feugiat, nibh enim fringilla felis, dapibus pellentesque ex arcu nec sapien. Sed ultrices nisl sit amet libero vehicula lacinia. Vestibulum hendrerit lorem at tempus interdum. Sed ac odio viverra, porttitor mauris at, dapibus leo. Sed orci nibh, gravida et egestas vel, porttitor non nisl. Suspendisse euismod lectus nisi, eget placerat risus interdum auctor. Integer lobortis felis metus, ac laoreet ligula varius sed. Suspendisse quis rhoncus odio. Morbi bibendum tellus eget felis finibus, nec cursus ligula tincidunt. Suspendisse tempor odio eu molestie rhoncus. Etiam nec enim sit amet purus hendrerit tempor eget non urna. Morbi in sagittis tellus.', 'admin', 'House Painter', '2017-12-26')";
                DoCommand(sql);
                sql = "insert into SERVICE (Title, Description, OwnerUserId, Category, addDate) values ('service4', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam laoreet lectus ac nisi pretium, et sagittis purus aliquet. Vestibulum quis aliquet lectus. Sed rhoncus, quam eu euismod finibus, mi sapien sodales lacus, vitae tempus felis erat eget ex. Integer blandit, lacus sit amet tempus feugiat, nibh enim fringilla felis, dapibus pellentesque ex arcu nec sapien. Sed ultrices nisl sit amet libero vehicula lacinia. Vestibulum hendrerit lorem at tempus interdum. Sed ac odio viverra, porttitor mauris at, dapibus leo. Sed orci nibh, gravida et egestas vel, porttitor non nisl. Suspendisse euismod lectus nisi, eget placerat risus interdum auctor. Integer lobortis felis metus, ac laoreet ligula varius sed. Suspendisse quis rhoncus odio. Morbi bibendum tellus eget felis finibus, nec cursus ligula tincidunt. Suspendisse tempor odio eu molestie rhoncus. Etiam nec enim sit amet purus hendrerit tempor eget non urna. Morbi in sagittis tellus.', 'admin', 'House Painter', '2017-12-26')";
                DoCommand(sql);

                sql = "CREATE TABLE COMMENT (CommentId decimal NOT NULL, " +
                    "Title VARCHAR(50), Content VARCHAR(500), WriterUserId VARCHAR(50), ServiceTitle VarChar(50), PRIMARY KEY(CommentId))";
                DoCommand(sql);

            }

            return success;
        }
    }
}