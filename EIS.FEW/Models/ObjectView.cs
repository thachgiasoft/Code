using System.ComponentModel.DataAnnotations;

namespace EIS.FEW.Models
{
    public class ObjectView
    {
        public int AppID { get; set; }
        public bool locked { get; set; }
        [Required(ErrorMessage = "Nhập tên nhóm quyền!")]
        public string name { get; set; }
        public int objectid { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn một nhóm chức năng!")]
        public int nhomchucnangid { get; set; }
        public string DESCRIPTION { get; set; }
    }
}