using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.ServiceImp
{
    public class TYPE_PERMISSIONService : BaseService<TYPE_PERMISSION, int>, ITYPE_PERMISSIONService
    {
        public TYPE_PERMISSIONService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
    }
}
