using System;
using System.Linq;
using EIS.Core.Domain;

namespace EIS.Core.IService
{
    public interface ILogSystemService : FX.Data.IBaseService<LogSystem, int>
    {
        IQueryable<LogSystem> GetByFilter(string keyword, DateTime? fromdate, DateTime? todate, bool? IsAdmin,string TenNguoiDung);
        bool CreateNew(string userName, string mEvent, string comment, string ipaddress, string browser);
        int CreateLog(string userName, string mEvent, string comment, string ipaddress, string browser);
        bool UpdateError(int? logId);
    }
}
