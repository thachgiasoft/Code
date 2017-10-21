using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EIS.Core.ServiceImp
{
    public class DMTINHTHANHService : BaseService<DM_TINHTHANH, long>, IDMTINHTHANHService
    {
        public DMTINHTHANHService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<DM_TINHTHANH> getAll()
        {
            return Query.OrderByDescending(o => o.ID);
        }

        public IQueryable<DM_TINHTHANH> getByFilter(string text, bool? hieuluc, long? quocgiaid)
        {
            var textUpper = (text ?? "").Trim().ToUpper();
            return Query.Where(o => hieuluc == null || o.HIEULUC == hieuluc)
                            .Where(o => quocgiaid == null || o.QUOCGIA_ID == quocgiaid)
                            .Where(o => string.IsNullOrEmpty(text) || (o.MA).ToUpper().Contains(textUpper) || (o.TEN).ToUpper().Contains(textUpper));
        }

        public IQueryable<DM_TINHTHANH> GetProviceByUserRole(NGUOIDUNG user)
        {
            if (user == null)   // tránh chuyện vào 1 ngày đẹp trời người dùng bị null - thaipv
            {
                user = ((EISContext)FX.Context.FXContext.Current).CurrentNguoidung;
            }
            var vaitro = Common.Constants.CheckVaiTro(user);
            var query = from a in Query select a;

            //user is provice or Expertiser
            if (vaitro != (int)Common.Common.VaiTroND.TrungUong)
            {
                query = query.Where(n => n.ID == user.DONVI.TINHTHANH_ID);
            }
            return query;
        }
        public DM_TINHTHANH getDmTinhThanhByID(long ID)
        {
            return Query.FirstOrDefault(o => o.ID == ID);
        }
        public IQueryable<DM_TINHTHANH> getbyMa(string ma)
        {
            return Query.Where(o => o.MA == ma);
        }
        public IQueryable<DM_TINHTHANH> getbyID(long? id)
        {
            return Query.Where(o => o.ID == id);
        }
        public DM_TINHTHANH getbyFirstOrDefaultMa(string ma)
        {
            return Query.Where(o => o.MA == ma).FirstOrDefault();
        }
        public DM_TINHTHANH getbyFirstOrDefaultID(long? id)
        {
            return Query.Where(o => o.ID == id).FirstOrDefault();
        }

        public IList<DM_TINHTHANH> GetAllInHieuLucTrue()
        {
            var query = from a in Query where a.HIEULUC == true select a;

            query = query.OrderBy(n => n.MA);

            return query.ToList();
        }


        public IQueryable<DM_TINHTHANH> GetlstTinhthanh(int vaiTroId, long? tinhThanhID)
        {
            if (vaiTroId == (int)EIS.Core.Common.Common.VaiTroND.TrungUong)
            {
                return Query.Where(q => q.HIEULUC == true).OrderBy(q => q.TEN);
            }
            else
            {
                return Query.Where(q => q.ID == tinhThanhID && q.HIEULUC == true).OrderBy(q => q.TEN);
            }
        }


        public void GetNameCity(long? idCity, out string nameCity)
        {
            var item = Query.FirstOrDefault(o => o.ID == idCity);
            if(item != null)
            {
                nameCity = item.TEN;
            }
            else
            {
                nameCity = string.Empty;
            }
        }


        public string GetNameCityExcelMedicalFacility(long? idCity)
        {
            var matinh = Query.Where(o => o.ID == idCity);
            string matinhthanh = null;
            if (matinh.Any())
            {
                matinhthanh = matinh.FirstOrDefault().MA;
            }
            return matinhthanh;
        }  //lấy id truyền dữ liệu vào store TK Ngoại tỉnh

        public IQueryable<DM_TINHTHANH> GetbyIdHieuLuc(long? id)
        {
            return Query.Where(o=>o.HIEULUC == true && o.ID == id).OrderBy(o=>o.MA);
        }


        public string GetMaCity(long idCity)
        {
            var item = Query.FirstOrDefault(o => o.ID == idCity);
            var maCity = item != null ? item.MA : "CHUNG";
            return maCity;
        }


        public string GetNameCity(long idCiy)
        {
            var item = Query.FirstOrDefault(o => o.ID == idCiy);
            var nameCity = item != null ? item.TEN : string.Empty;
            return nameCity;
        }


        public string GetNameCity(string maCity)
        {
            var item = Query.FirstOrDefault(o => o.MA == maCity);
            return item != null ? item.TEN : string.Empty;
        }
    }
}