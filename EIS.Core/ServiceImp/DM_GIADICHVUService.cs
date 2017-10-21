using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class DM_GIADICHVUService : BaseService<DM_GIADICHVU, long>, IDM_GIADICHVUService
    {
        public DM_GIADICHVUService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
    }
}