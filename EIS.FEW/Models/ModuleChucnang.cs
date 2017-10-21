
using System.ComponentModel.DataAnnotations;

namespace EIS.FEW.Models
{
    public class ModuleChucnang
    {
        public virtual int NHOMCHUCNANGID { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn một Module!")]
        public virtual int MODULEID { get; set; }
        public virtual string ModuleDESCRIPTION { get; set; }
        [Required(ErrorMessage = "Nhập tên nhóm chức năng!")]
        public virtual string ChucnangDESCRIPTION { get; set; }
    }
}