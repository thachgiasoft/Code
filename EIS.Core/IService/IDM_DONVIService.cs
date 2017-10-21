using System;
using System.Collections.Generic;
using System.Text;
using EIS.Core.Domain;
using FX.Data;
using System.Collections;
using System.Linq;
namespace EIS.Core.IService
{
    public interface IDM_DONVIService : IBaseService<DM_DONVI, long>
    {
        IQueryable<DM_DONVI> getAll();
        IList<DM_DONVI> SearchAll(int comid, int pageIndex, int pageSize, out int totalRecords);
        List<DM_DONVI> filter(string searchText, Boolean? isHieuLuc = null);
        IQueryable<DM_DONVI> getByFilter(string strTimKiem, long? cbTinhThanh,NGUOIDUNG nguoidung);
        List<DM_DONVI> getByTinhThanhID(long tinhThanhID);
        DM_DONVI getDonviMatinh(string matinh);
        List<DM_DONVI> getByTThanhID(long tinhThanhID);
        IQueryable<DM_DONVI> getByDonviID(long? id);
        List<DM_DONVI> GetCities(long? countryID);
        /// <summary>
        /// Lấy danh sách đơn vị bhxh theo người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IQueryable<DM_DONVI> GetByUserRole(NGUOIDUNG user);

        long? IddonviBhxh();

        IQueryable<DM_DONVI> getByDonviIDDonvichaID(long? id);
        IQueryable<DM_DONVI> getByDonvichaID(long? id);
        DM_DONVI getFirstOrDefaultByDonviID(long? id);
        bool chekcAnyMa(string MA);
        bool chekcAnyMa_MaDonvi(string MA, string donvi_MA);
        DM_DONVI getFirstOrDefaultByMACHA(string MACHA);
        IQueryable<DM_DONVI> Getlst_DonVi_DK_CosoKCB(long? tinhThanh);
        List<long> Getiddonvi(IQueryable<DM_DONVI> lstDonvi);
        List<long> GetlstIddonvi(long idDonvi);
        long? GetlstDonviTheoTT(long? idTinhThanh);
        IQueryable<DM_DONVI> GetlstDonViTK_DMTCSKCB(string TWT,long? idDonVi);

        DM_DONVI GetByMa(string ma);

        bool CheckAnyChild(long parentId);

        IQueryable<DM_DONVI> GetByDonVi();

        long? GetidCity(long? idTinh);

        long GetidUnit(long? idCity);

        IQueryable<DM_DONVI> getByID(long? ID);

        DM_DONVI GetIdDonvi(long idDonvi);


        void GetNameMaTinhThanhByIdDonVi(long idDonvi, out string nameCity, out string  maCity);

        /// <summary>
        /// Author: duonghd - 15/05/2017
        /// method lấy về các đơn vị thuộc vai trò của người dùng khi đăng nhập hệ thống
        /// </summary>
        /// <param name="user">người dùng khi đăng nhập hệ thống</param>
        /// <param name="df_donviId">out donviId default khi lên màn hình</param>
        /// <returns>Danh sách đơn vị của người dùng được nhìn thấy trên màn hình</returns>
        IQueryable<DM_DONVI> GetByUserRole(NGUOIDUNG user, out long df_donviId);

        string GetNameDonVi(long idDonvi);

        /// <summary>
        /// Author: duonghd
        /// Get Parent And Childs
        /// </summary>
        /// <param name="parentId">Don Vị Cha Id</param>
        /// <returns>Danh sách đơn vị cha và con</returns>
        IList<DM_DONVI> GetParentAndChilds(long parentId);

        string GetNamTinhThanhByIdDonVi(long idDonvi);
    }
}
