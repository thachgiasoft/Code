using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Models
{
    public class CoCauThucTeChiTietItem
    {
        public long ID { set; get; }
        public long ID_ThucTe { set; get; }
        public long IDLoai { set; get; }
        public string MaCoCau { set; get; }
        public string TenCoCauOld { set; get; }
        public double SoLuongOld { set; get; }
        public double DonGiaOld { set; get; }
        public string TenCoCauNew { set; get; }
        public double SoLuongNew { set; get; }
        public double DonGiaNew { set; get; }
        public string LoaiName { set; get; }
    }
}