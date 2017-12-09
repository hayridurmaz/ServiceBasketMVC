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

                sql = "CREATE TABLE SERVICE (ServiceId decimal, " +
                    "Title VARCHAR(50),Description VARCHAR(500), OwnerUserId VARCHAR(50), PRIMARY KEY(ServiceId))";//kategori ekle!!
                DoCommand(sql);

                sql = "CREATE TABLE COMMENT (CommentId decimal, " +
                    "Title VARCHAR(50), Content VARCHAR(500), WriterUserId VARCHAR(50), PRIMARY KEY(CommentId))";
                DoCommand(sql);

                sql = "CREATE TABLE WRITTENTO (CommentId decimal, ServiceId decimal)";
                DoCommand(sql);

            }

            return success;
        }
    }
}