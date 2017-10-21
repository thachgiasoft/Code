using System.Globalization;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface INGUOIDUNGService : IBaseService<NGUOIDUNG, long>
    {
        List<NGUOIDUNG> getAll();
        IQueryable<NGUOIDUNG> getByNguoidung(NGUOIDUNG nguoidung);
        bool CheckPermission(string username, string pmsname);
        string CheckquyenPermission(string lstTacVu, NGUOIDUNG nguoidung); // 
        bool DoCheckPermission(string username, string pmsname);
        string CheckquyenPermission_tinh(string lstTacVu, NGUOIDUNG nguoidung);
    }
}
