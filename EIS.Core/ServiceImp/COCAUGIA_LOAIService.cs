using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_LOAIService : BaseService<COCAUGIA_LOAI, long>, ICOCAUGIA_LOAIService
    {
        public COCAUGIA_LOAIService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<COCAUGIA_LOAI> getById(long id)
        {
            return Query.Where(o => o.ID == id);
        }
    }
}