using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_LOAIService : IBaseService<COCAUGIA_LOAI, long>
    {
        IQueryable<COCAUGIA_LOAI> getById(long id);
    }
}