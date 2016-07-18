using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Controllers
{
    public class MySQLConnection
    {
        private String CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString;
        private MySqlConnection dbConnection;

        public MySQLConnection()
        {
            initialize();
        }

        private void initialize()
        {
            dbConnection = new MySqlConnection(CONNECTION_STRING);
        }

        private Boolean openConnection()
        {
            try
            {
                dbConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public Boolean closeConnection()
        {
            try
            {
                dbConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader getMySqlDataReader(String query)
        {
            try
            {
                if (this.openConnection() == true)
                {
                    MySqlCommand cmd = dbConnection.CreateCommand();
                    cmd.CommandText = query;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    return reader;
                }
                else
                {
                    return null;
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}