using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class DM_DICHVU43Service : BaseService<DM_DICHVU43, long>, IDM_DICHVU43Service
    {
        public DM_DICHVU43Service(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
    }
}