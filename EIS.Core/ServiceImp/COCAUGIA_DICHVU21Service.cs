using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using EIS.Core.Common;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_DICHVU21Service : BaseService<COCAUGIA_DICHVU21, long>, ICOCAUGIA_DICHVU21Service
    {
        public COCAUGIA_DICHVU21Service(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<COCAUGIA_DICHVU21> getById(long id)
        {
            return Query.Where(o => o.ID == id);
        }

        public IQueryable<COCAUGIA_DICHVU21> getByFilter(string maTinh, string trangThai)
        {
            IQueryable<COCAUGIA_DICHVU21> query = Query;

            if (!string.IsNullOrEmpty(trangThai))
            {
                if (trangThai.Equals("Chờ duyệt")) query = query.Where(o => o.TRANGTHAI == 0);
                if (trangThai.Equals("Đã duyệt")) query = query.Where(o => o.TRANGTHAI == 1);
            }
            if (!string.IsNullOrEmpty(maTinh)) query = query.Where(o => o.MATINH == maTinh);

            return query;
        }

        public void UpdateAll()
        {
            OracleDB db = new OracleDB();
            string query = "UPDATE COCAUGIA_DICHVU21 SET TRANGTHAI = 1 WHERE TRANGTHAI = 0";

            db.ExecuteNonQuery(query);
        }
    }
}