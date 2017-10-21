using DevExpress.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using System.Linq;

namespace EIS.Core.Common
{
    public class DMCOSOKCB_TKDMCSKCBConLargDataHelper
    {
        private DMCOSOKCB_TKDMCSKCBConLargDataHelper()
        {
        }

        private long? DonViId { get; set; }
        private long? coSoKcbId { get; set; }
        private NGUOIDUNG NguoiDung { get; set; }

        public bool? HieuLuc { get; set; }

        public DMCOSOKCB_TKDMCSKCBConLargDataHelper(long? donViId, long? coSoKcbId, NGUOIDUNG nguoiDung)
        {
            this.DonViId = donViId;
            this.NguoiDung = nguoiDung;
            this.coSoKcbId = coSoKcbId;
        }

        public object GetCSKCBRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstCoSoKCB_Con(DonViId, coSoKcbId, Constants.CheckVaiTro(NguoiDung), NguoiDung);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()) || n.MACOSOKCBCHA.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take);
            var test = query.ToList().OrderBy(o => o.MA);
            return test;
        }

        public object GetCSKCBByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstCoSoKCB_Con(DonViId, coSoKcbId, Constants.CheckVaiTro(NguoiDung), NguoiDung);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            return query.Where(p => p.ID == id).Take(1);
        }
    }
}