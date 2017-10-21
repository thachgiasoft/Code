using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_PHATSINHService : BaseService<COCAUGIA_PHATSINH, long>, ICOCAUGIA_PHATSINHService
    {
        public COCAUGIA_PHATSINHService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<COCAUGIA_PHATSINH> getByFilter(long chitietId, int cskcbId)
        {
            IQueryable<COCAUGIA_PHATSINH> query = Query;

            query = query.Where(o => o.CHITIET_ID == chitietId && o.COSOKCB_ID == cskcbId);

            return query;
        }
    }
}