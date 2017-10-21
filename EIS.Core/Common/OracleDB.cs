using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EIS.Core.Domain;
using NPOI.SS.Formula.Functions;
using Oracle.DataAccess.Client;
using System.Configuration;

namespace EIS.Core.Common
{
    public class OracleDB
    {
        private OracleConnection oracleConnection;
        private OracleConnectionStringBuilder stringBuilder;

        public OracleDB(string dbUsername, string dbPassword, string dbServer)
        {
            stringBuilder = new OracleConnectionStringBuilder();
            stringBuilder.DataSource = dbServer;
            stringBuilder.UserID = dbUsername;
            stringBuilder.Password = dbPassword;
            oracleConnection = new OracleConnection();
            oracleConnection.ConnectionString = stringBuilder.ConnectionString;
        }
        public OracleDB(string connectString)
        {
            string[] arr = connectString.Split(';');
            stringBuilder = new OracleConnectionStringBuilder();
            stringBuilder.DataSource = arr[0].Replace("Data Source=","").Trim();
            stringBuilder.UserID = arr[1].Replace("User Id=", "").Trim();
            stringBuilder.Password = arr[2].Replace("Password=", "").Trim();
            oracleConnection = new OracleConnection();
            oracleConnection.ConnectionString = stringBuilder.ConnectionString;
        }
        public OracleDB()
        {

        }
        public void ExecuteNonQuery(string query)
        {
            OracleConnection con;
            con = new OracleConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            con.Open();
            OracleCommand cmd = new OracleCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
    }
}
