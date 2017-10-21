using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;

namespace EIS.Core.IService
{
    public interface INHOM_USERService : IBaseService<NHOM_USER, int>
    {
        bool AddNewGroup(string groupName, List<int> userId, out string mess);

        bool UpdateGroup(int groupId, string groupName, List<int> userId, out string mess);

        bool DeleteGroup(int groupId, out string mess);
    }
}