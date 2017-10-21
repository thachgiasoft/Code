using System;
using DevExpress.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using System.Linq;

namespace EIS.Core.Common
{
    public class DMCOSOKCBLargDataHelper
    {
        private DMCOSOKCBLargDataHelper()
        {
        }

        private long DonViId { get; set; }
        private NGUOIDUNG NguoiDung { get; set; }

        public bool? HieuLuc { get; set; }

        public DMCOSOKCBLargDataHelper(long donViId, NGUOIDUNG nguoiDung)
        {
            this.DonViId = donViId;
            this.NguoiDung = nguoiDung;
        }

        public object GetCSKCBRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take);
            return query;
        }

        public object GetCSKCBByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            return query.Where(p => p.ID == id).Take(1);
        }



        public object GetCsTreEmKhongTheRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var vaiTro = Constants.CheckVaiTro(NguoiDung);
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = vaiTro == (int)Common.VaiTroND.TruongNhom ? dmcskcbSrv.GetLstCosoTruongNhom(DonViId) : dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take);
            return query;
        }

        public object GetCsTreEmKhongTheByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            var vaiTro = Constants.CheckVaiTro(NguoiDung);
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = vaiTro == (int)Common.VaiTroND.TruongNhom ? dmcskcbSrv.GetLstCosoTruongNhom(DonViId) : dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            return query.Where(p => p.ID == id).Take(1);
        }



        public object GetCSKCBFormDm_DungChungRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstcskDM(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take).OrderBy(o => o.MA);
            return query;
        }

        public object GetCSKCBFormDm_DungChungByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetlstcskDM(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(p => p.ID == id).Take(1);
            return query;
        }

        public object GetCskcbChaRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.Getlstcskcbb(DonViId, Constants.CheckVaiTro(NguoiDung), NguoiDung);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            

            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take);
            var test = query.ToList().OrderBy(o => o.MA);
            return test;
        }

        public object GetCskcbChaById(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.Getlstcskcbb(DonViId, Constants.CheckVaiTro(NguoiDung), NguoiDung);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            return query.Where(p => p.ID == id).Take(1);
        }

        public object GetCSKCBRange_tracuu(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(o => o.MACOSOKCBCHA == null);
            query = query.Where(n => n.MA.ToUpper().Contains(args.Filter.ToUpper()) || n.TEN.ToUpper().Contains(args.Filter.ToUpper()))
                .Skip(skip).Take(take);
            return query;
        }

        public object GetCSKCBByID_tracuu(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            var dmcskcbSrv = IoC.Resolve<IDMCOSOKCBService>();
            var query = dmcskcbSrv.GetMedicalFacilitiesByUserRoleInHospital(NguoiDung, DonViId);
            if (HieuLuc.HasValue)
            {
                query = query.Where(n => n.HIEULUC == HieuLuc.Value);
            }
            query = query.Where(o => o.MACOSOKCBCHA == null);
            return query.Where(p => p.ID == id).Take(1);
        }
    }
}