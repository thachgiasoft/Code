using System;
using System.Collections.Generic;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;

namespace EIS.Core.ServiceImp
{
    public class USER_GROUPService : BaseService<USER_GROUP, int>, IUSER_GROUPService
    {
        public USER_GROUPService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }

        public IList<USER_GROUP> GetUserIdByGroupId(int groupId)
        {
            return Query.Where(n => n.GROUP_ID == groupId).ToList();
        }
    }
}