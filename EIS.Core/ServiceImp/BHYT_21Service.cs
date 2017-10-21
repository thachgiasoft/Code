using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.ServiceImp
{
    public class BHYT_21Service : BaseService<BHYT_21, long>, IBHYT_21Service
    {
        public BHYT_21Service(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }
    }
}
