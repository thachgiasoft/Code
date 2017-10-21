using System;
using EIS.Core.Domain;
using FX.Data;
using System.Linq;
using System.Collections.Generic;
using EIS.Core.CustomView;
using log4net;
using DevExpress.Web;
namespace EIS.Core.IService
{
    public interface IDMCOSOKCBService : IBaseService<DM_COSOKCB, long>
    {
        IQueryable<DM_COSOKCB> getAll();
     
        IQueryable<DM_COSOKCB> getByTinhThanhID(long tinhThanhID);

        IQueryable<DM_COSOKCB> GetByMa(string ma);

        IEnumerable<DM_COSOKCB> GetDM_COSOKCBRange(ListEditItemsRequestedByFilterConditionEventArgs args);

        DM_COSOKCB GetDM_COSOKCBByID(ListEditItemRequestedByValueEventArgs args);

        DM_COSOKCB GetDM_COSOKCBByMa(ListEditItemRequestedByValueEventArgs args);
    }
}
