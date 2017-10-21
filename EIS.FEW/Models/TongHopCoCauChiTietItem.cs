using EIS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Models
{
    /// <summary>
    /// Phan tu trong bang hien thi du lieu
    /// </summary>
    public class TongHopCoCauChiTietItem
    {
        public int Id { set; get; }
        public string Ma { set; get; }
        public string NoiDung { set; get; }
        public decimal SoLuongOld { set; get; }
        public decimal DonGiaOld { set; get; }
        public decimal ThanhTienOld { set; get; }
        public decimal SoLuongNew { set; get; }
        public decimal DonGiaNew { set; get; }
        public decimal ThanhTienNew { set; get; }
        public decimal ChenhLech { set; get; }
        public string TenLoai { set; get; }
    }
}