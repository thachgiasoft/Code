using DevExpress.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using System.Linq;

namespace EIS.Core.Common
{
    public class DMCOSOKCB_TKDMLargDataHelper
    {
        private DMCOSOKCB_TKDMLargDataHelper()
        {
        }

        private long DonViId { get; set; }
        private NGUOIDUNG NguoiDung { get; set; }

        public bool? HieuLuc { get; set; }

        public DMCOSOKCB_TKDMLargDataHelper(long donViId, NGUOIDUNG nguoiDung)
        {
            this.DonViId = donViId;
            this.NguoiDung = nguoiDung;
        }

        public object GetCSKCBRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstcskDM(NguoiDung, DonViId);
            //if (HieuLuc.HasValue)
            //{
            //    query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            //}
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take).OrderBy(o => o.MA);
            return query;
        }

        public object GetCSKCBByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstcskDM(NguoiDung, DonViId);
            //if (HieuLuc.HasValue)
            //{
            //    query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            //}
            query = query.Where(p => p.ID == id).Take(1).OrderBy(o => o.MA);
            return query;
        }
    }
}