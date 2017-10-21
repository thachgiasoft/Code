using System.ComponentModel.DataAnnotations;

namespace EIS.Core.Domain
{
    public class NHOMCHUCNANG
    {
        public virtual int NHOMCHUCNANGID { get; set; }
        public virtual int MODULEID { get; set; }
        [Required(ErrorMessage = "Nhập tên nhóm chức năng!")]
        public virtual string DESCRIPTION { get; set; }
    }
}
