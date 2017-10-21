using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_DICHVU21Service : IBaseService<COCAUGIA_DICHVU21, long>
    {
        IQueryable<COCAUGIA_DICHVU21> getById(long id);
        IQueryable<COCAUGIA_DICHVU21> getByFilter(string maTinh, string trangThai);
        void UpdateAll();
    }
}