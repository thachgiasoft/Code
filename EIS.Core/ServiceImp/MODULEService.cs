using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.ServiceImp
{
    public class MODULEService : BaseService<MODULE, int>, IMODULEService
    {
        public MODULEService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }
    }
}
