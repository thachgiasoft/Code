using EIS.Core.Common;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Helpers;
using FX.Core;

namespace EIS.Core.ServiceImp
{
    public class DM_DONVIService : BaseService<DM_DONVI, long>, IDM_DONVIService
    {
        public DM_DONVIService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<DM_DONVI> getAll()
        {
            return Query;
        }

        public IList<DM_DONVI> SearchAll(int comid, int pageIndex, int pageSize, out int totalRecords)
        {
            IQueryable<DM_DONVI> qr = from s in Query.OrderBy(o => o.STT) select s;
            totalRecords = qr.Count();
            if (pageSize > 0)
                return qr.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return qr.ToList();
        }

        public List<DM_DONVI> filter(string searchText, Boolean? isHieuLuc = null)
        {
            return Query.Where(o => (isHieuLuc == null || o.HIEULUC == isHieuLuc)
                                    && (searchText == "" || o.MA.Contains((searchText ?? "").Trim()) || o.TEN.Contains((searchText ?? "").Trim()) || o.MIEUTA.Contains((searchText ?? "").Trim())))
                        .OrderBy(o => o.STT)
                        .ToList();
        }

        public IQueryable<DM_DONVI> getByFilter(string str_TimKiem, long? cbTinhThanh, NGUOIDUNG nguoidung)
        {
            var textUpper = (str_TimKiem ?? "").Trim().ToUpper();
            var tinhthanhid = nguoidung.DONVI.TINHTHANH_ID;
            //var textMacha = cbTinhThanh.Substring(0, 2);
            return Query
                //.Where(o => string.IsNullOrEmpty(cbTinhThanh) || o.MA.ToUpper().Contains(cbTinhThanh))
                //.Where(o => cbTinhThanh == null || o.TINHTHANH_ID == cbTinhThanh)
                //.Where(o =>
                //    string.IsNullOrEmpty(str_TimKiem) ||
                //    o.MA.ToUpper().Contains(textUpper) ||
                //    o.TEN.ToUpper().Contains(textUpper)
                //    );
                .Where(o => cbTinhThanh == null || o.TINHTHANH_ID == cbTinhThanh)
                .Where(o => string.IsNullOrEmpty(str_TimKiem)
                            || o.MA.ToUpper().Contains(textUpper)
                            || o.TEN.ToUpper().Contains(textUpper));
        }

        public List<DM_DONVI> getByTinhThanhID(long tinhThanhID)
        {
            return Query.Where(w => w.DONVICHA_ID == tinhThanhID).ToList();
        }

        /// <summary>
        /// lấy đơn vị bảo hiểm xã hội tỉnh theo mã tỉnh thành
        /// </summary>
        /// <param name="matinh"></param>
        /// <returns></returns>
        public DM_DONVI getDonviMatinh(string matinh)
        {
            return Query.Where(w => w.MATINHTHANH == matinh && w.MACHA == "BHXHVN").FirstOrDefault();
        }

        public List<DM_DONVI> getByTThanhID(long tinhThanhID)
        {
            return Query.Where(w => w.TINHTHANH_ID == tinhThanhID).ToList();
        }

        public IQueryable<DM_DONVI> getByDonviID(long? id)
        {
            return Query.Where(w => w.DONVICHA_ID == id && w.HIEULUC == true);
        }

        public List<DM_DONVI> GetCities(long? countryID)
        {
            var listcskcb = Query.Where(o => o.TINHTHANH_ID == countryID).OrderBy(o => o.MA).ToList();
            return listcskcb;
        }

        public IQueryable<DM_DONVI> GetByUserRole(NGUOIDUNG user)
        {
            if (user == null)   // tránh chuyện vào 1 ngày đẹp trời người dùng bị null - thaipv
            {
                user = ((EISContext)FX.Context.FXContext.Current).CurrentNguoidung;
            }
            var query = from a in Query where a.HIEULUC == true select a;
            var vaiTro = Constants.CheckVaiTro(user);
            if (vaiTro == (int)(Common.Common.VaiTroND.TrungUong))
            {
                query = query.Where(q => q.DONVICHA_ID == user.DONVI_ID);
            }
            else if (vaiTro == (int)(Common.Common.VaiTroND.Tinh))
            {
                query = query.Where(n => n.ID == user.DONVI_ID || n.DONVICHA_ID == user.DONVI_ID);
            }
            else
            {
                query = query.Where(n => n.ID == user.DONVI_ID);
            }
            return query.OrderBy(n => n.MA);
        }

        public long? IddonviBhxh()
        {
            var idfirst = Query.Where(o => o.MA == "BHXHVN").FirstOrDefault() != null ? Query.Where(o => o.MA == "BHXHVN").FirstOrDefault().ID : 0;
            return idfirst;
        }

        public IQueryable<DM_DONVI> getByDonviIDDonvichaID(long? id)
        {
            //return Query.Where(w => w.DONVICHA_ID == id && w.HIEULUC == true);
            return Query.Where(o => o.ID == id || o.DONVICHA_ID == id).OrderBy(o => o.MA);
        }
        public IQueryable<DM_DONVI> getByDonvichaID(long? id)
        {
            return Query.Where(o => o.DONVICHA_ID == id).OrderBy(o => o.MA);
        }
        #region List Donvi: ID = DONVI_ID

        #endregion

        #region Obj Donvi: FirstOrDefault(ID = DONVI_ID)
        public DM_DONVI getFirstOrDefaultByDonviID(long? id)
        {
            return Query.FirstOrDefault(o => o.ID == id);
        }
        #endregion

        #region check trung ma: Any(MA)
        public bool chekcAnyMa(string MA)
        {
            return Query.Any(o => o.MA == MA.Trim());
        }
        #endregion

        #region check trung ma: Any(MA, donvi.MA)
        public bool chekcAnyMa_MaDonvi(string MA, string MaDonvi)
        {
            return Query.Any(o => o.MA == MA.Trim() && o.MA != MaDonvi);
        }
        #endregion

        #region Obj Donvi: FirstOrDefault(MA = MACHA)
        public DM_DONVI getFirstOrDefaultByMACHA(string MACHA)
        {
            return Query.FirstOrDefault(t => t.MA == MACHA);
        }
        #endregion

        /// <summary>
        /// Lấy mã bảo hiểm xã hội việt nam
        /// </summary>
        /// <returns></returns>
        /// THOHD - 25/11/2016 17:00
        public string maBHXHVN()
        {
            return Query.Where(o => o.MATINHTHANH == null && o.MACHA == null && o.HIEULUC == true).Select(o => o.MA).FirstOrDefault();
        }

        public IQueryable<DM_DONVI> Getlst_DonVi_DK_CosoKCB(long? tinhThanh)
        {
            var query = Query.Where(q => q.TINHTHANH_ID == tinhThanh && q.HIEULUC == true).ToList();

            //thaipv : bổ sung thêm đơn vị Bộ Quốc Phòng vào list đơn vị
            var checkbqp = query.Where(q => q.MA.Equals("97"));
            if (!checkbqp.Any())
            {
                var bqp = Query.FirstOrDefault(x => x.MA.Equals("97"));
                query.Insert(1, bqp);
            }
            return query.AsQueryable();
        }

        public List<long> Getiddonvi(IQueryable<DM_DONVI> lstDonvi)
        {
            var query = lstDonvi.Select(o => o.ID);
            return query.ToList();
        }



        public List<long> GetlstIddonvi(long idDonvi)
        {
            var query = Query.Where(o => o.ID == idDonvi || o.DONVICHA_ID == idDonvi).Select(o => o.ID).ToList();
            return query;

        }


        public long? GetlstDonviTheoTT(long? idTinhThanh)
        {
            long? idDonVi = null;
            var dv = Query.Where(q =>
                           q.TINHTHANH_ID == idTinhThanh && q.HIEULUC == true && q.MA == "BHXHVN");
            var dvtb = Query.Where(q =>
                    q.TINHTHANH_ID == idTinhThanh && q.HIEULUC == true);
            if (dv.Any()) idDonVi = dv.FirstOrDefault().ID;
            else idDonVi = dvtb.FirstOrDefault().ID;
            return idDonVi;
        }


        public IQueryable<DM_DONVI> GetlstDonViTK_DMTCSKCB(string TWT, long? idDonVi)
        {
            if (TWT == "TW")
            {
                var idDonviBhxh = IddonviBhxh();
                var lst = Query.Where(o => o.DONVICHA_ID == idDonviBhxh);
                return lst;
            }
            else
            {
                var lst = Query.Where(o => o.ID == idDonVi);
                return lst;
            }

        }

        public DM_DONVI GetByMa(string ma)
        {
            return Query.FirstOrDefault(n => n.MA.Equals(ma) && n.HIEULUC == true);
        }

        public bool CheckAnyChild(long parentId)
        {
            var query = Query.Where(n => n.DONVICHA_ID == parentId && n.HIEULUC == true);

            return query.Any();
        }


        public IQueryable<DM_DONVI> GetByDonVi()
        {
            var bhxhvnModel = GetByMa(Helpper.MA_BHXHVN);
            return Query.Where(o => o.DONVICHA_ID == bhxhvnModel.ID || o.ID == bhxhvnModel.ID);
        }


        public long? GetidCity(long? idTinh)
        {
            var bhxhvnModel = GetByMa(Helpper.MA_BHXHVN);
            if (bhxhvnModel.ID == idTinh)
            {
                return null;
            }
            else
            {
                return idTinh;
            }
        } //lấy id truyền dữ liệu vào store TK bệnh viện


        public long GetidUnit(long? idCity)
        {
            var data = Getlst_DonVi_DK_CosoKCB(idCity);
            var result = data.FirstOrDefault();
            var item = result != null ? result.ID : 0;
            return item;
        }

        public IQueryable<DM_DONVI> getByID(long? ID)
        {
            return Query.Where(o => o.ID == ID);
        }


        public DM_DONVI GetIdDonvi(long idDonvi)
        {
            return Query.FirstOrDefault(o => o.ID == idDonvi);
        }


        public void GetNameMaTinhThanhByIdDonVi(long idDonvi, out string nameCity, out string maCity)
        {
            var itemdonvi = Query.FirstOrDefault(o => o.ID == idDonvi);
            var idTinhthanh = itemdonvi != null ? itemdonvi.TINHTHANH_ID : 0;
            var itemtinhthanh = IoC.Resolve<IDMTINHTHANHService>().Query.FirstOrDefault(o => o.ID == idTinhthanh);
            nameCity = itemtinhthanh != null ? itemtinhthanh.TEN : string.Empty;
            maCity = itemtinhthanh != null ? itemtinhthanh.MA : string.Empty;
        }

        public IQueryable<DM_DONVI> GetByUserRole(NGUOIDUNG user, out long df_donviId)
        {
            var query = GetByUserRole(user);

            if (query.Any(n => n.ID == user.DONVI_ID.Value))
            {
                df_donviId = user.DONVI_ID.Value;
            }
            else
            {
                df_donviId = query.FirstOrDefault().ID;
            }

            return query;
        }

        public string GetNameDonVi(long idDonvi)
        {
            var item = Query.FirstOrDefault(o => o.ID == idDonvi);
            return item != null ? item.TEN : string.Empty;
        }

        public IList<DM_DONVI> GetParentAndChilds(long parentId)
        {
            return Query.Where(n => n.ID == parentId || n.DONVICHA_ID == parentId).ToList();
        }

        public string GetNamTinhThanhByIdDonVi(long idDonVi)
        {
            var itemdonvi = Query.FirstOrDefault(o => o.ID == idDonVi);
            var idTinhthanh = itemdonvi != null ? itemdonvi.TINHTHANH_ID : 0;
            var itemtinhthanh = IoC.Resolve<IDMTINHTHANHService>().Query.FirstOrDefault(o => o.ID == idTinhthanh);
            return  itemtinhthanh != null ? itemtinhthanh.TEN : string.Empty;
        }
    }
}