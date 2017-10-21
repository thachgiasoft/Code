using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.Core.Common;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_THUCTEService : BaseService<COCAUGIA_THUCTE, long>, ICOCAUGIA_THUCTEService
    {
        public COCAUGIA_THUCTEService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }

        public COCAUGIA_THUCTE GetByFilter(string maCskcb, string maThe, DateTime? ngayVao, DateTime? ngayRa, string maDichVu)
        {
            string subMa = maDichVu.Right(4);
            var query = Query.Where(
                o => o.COSOKCB.MA.Equals(maCskcb) && 
                o.MA_THE.Equals(maThe) && 
                o.NGAY_RA == ngayRa && 
                o.NGAY_VAO == ngayVao)
                .ToList();

            foreach (var item in query)
            {
                try
                {
                    if (item.DICHVU21 != null &&
                        !string.IsNullOrEmpty(item.DICHVU21.MA_DICHVU21) &&
                        item.DICHVU21.MA_DICHVU21.EndsWith(subMa))
                        return item;
                }
                catch (Exception e)
                {

                }
            }

            return null;
        }
    }
}
