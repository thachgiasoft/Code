using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class DM_DICHVU50Service : BaseService<DM_DICHVU50, long>, IDM_DICHVU50Service
    {
        public DM_DICHVU50Service(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
    }
}