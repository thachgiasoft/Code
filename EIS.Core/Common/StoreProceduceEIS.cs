using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using log4net;
using EIS.Core.Domain;
using Oracle.DataAccess.Types;
using FX.Core;
using EIS.Core.IService;

namespace EIS.Core.Common
{
    public class StoreProcedureEIS
    {
        public string OracleDataIp;
        public string ID;
        public string Password;
        public OracleConnection con;

        public void Open()
        {
            con = new OracleConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            con.Open();
        }
        public void Close()
        {
            con.Close();
            con.Dispose();
        }
        public DataTable getDataByProcedure(string name, List<OracleParameter> listPr)
        {
            List<OracleParameter> lst= new List<OracleParameter>();
            DataTable dt = new DataTable();
            try
            {
                Open();
                OracleCommand cmd = new OracleCommand(name, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;
                for (int i =listPr.Count-1; i >= 0; i --)
                {
                     cmd.Parameters.Add(listPr[i]);
                }
                //foreach (OracleParameter p in listPr)
                //{
                //    cmd.Parameters.Add(p);
                //}
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
                return dt;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Close();
            }
        }

    }
}
