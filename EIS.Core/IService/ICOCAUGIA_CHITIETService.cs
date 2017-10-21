using DevExpress.Web;
using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_CHITIETService : IBaseService<COCAUGIA_CHITIET, long>
    {
        IQueryable<COCAUGIA_CHITIET> getByFilter(int nhomdvId);
        IQueryable<COCAUGIA_CHITIET> getByFilter(string maNhomdv);
        IEnumerable<COCAUGIA_CHITIET> GetByMaTrung4So(string ma);
        IEnumerable<COCAUGIA_CHITIET> GetCOCAUGIA_CHITIETRange(ListEditItemsRequestedByFilterConditionEventArgs args);
        COCAUGIA_CHITIET GetCOCAUGIA_CHITIETByID(ListEditItemRequestedByValueEventArgs args);
    }
}