using EIS.Core.Domain;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface ITYPE_ROLEService : IBaseService<TYPE_ROLE, long>
    {
        IQueryable<TYPE_ROLE> getAll();
    }
}