using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_PHATSINHService : IBaseService<COCAUGIA_PHATSINH, long>
    {
        IQueryable<COCAUGIA_PHATSINH> getByFilter(long chitietId, int cskcbId);
    }
}