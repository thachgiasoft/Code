using EIS.Core.Domain;
using FX.Data;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface IDMTINHTHANHService : IBaseService<DM_TINHTHANH, long>
    {
        IQueryable<DM_TINHTHANH> getAll();

        IQueryable<DM_TINHTHANH> getByFilter(string text, bool? hieuluc, long? quocgiaid);

        /// <summary>
        /// Lấy về các tỉnh thành theo quyền người dùng được cấp
        /// </summary>
        /// <param name="user">Người dùng</param>
        /// <returns></returns>
        IQueryable<DM_TINHTHANH> GetProviceByUserRole(NGUOIDUNG user);

        DM_TINHTHANH getDmTinhThanhByID(long ID);

        IQueryable<DM_TINHTHANH> getbyMa(string ma);

        IQueryable<DM_TINHTHANH> getbyID(long? id);

        DM_TINHTHANH getbyFirstOrDefaultMa(string ma);

        DM_TINHTHANH getbyFirstOrDefaultID(long? id);

        IList<DM_TINHTHANH> GetAllInHieuLucTrue();
        IQueryable<DM_TINHTHANH> GetlstTinhthanh(int vaiTroId, long? tinhThanhID);

        void GetNameCity(long? idCity, out string nameCity);

        string GetNameCityExcelMedicalFacility(long? idCity);
        IQueryable<DM_TINHTHANH> GetbyIdHieuLuc(long? id);

        string GetMaCity(long idCity);


        string GetNameCity(long idCiy);

        string GetNameCity(string maCity);
    }
}