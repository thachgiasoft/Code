using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.ServiceImp
{
    public class TYPE_ROLEService : BaseService<TYPE_ROLE, long>, ITYPE_ROLEService
    {
        public TYPE_ROLEService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
        public IQueryable<TYPE_ROLE> getAll()
        {
            return Query;
        }
    }
}
