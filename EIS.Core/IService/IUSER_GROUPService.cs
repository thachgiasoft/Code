using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;

namespace EIS.Core.IService
{
    public interface IUSER_GROUPService : IBaseService<USER_GROUP, int>
    {
        IList<USER_GROUP> GetUserIdByGroupId(int groupId);
    }
}