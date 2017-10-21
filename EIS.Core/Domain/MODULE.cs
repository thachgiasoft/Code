using System.ComponentModel.DataAnnotations;
namespace EIS.Core.Domain
{
    public class MODULE
    {
        public virtual int MODULEID { get; set; }
        [Required(ErrorMessage = "Nhập tên Module!")]
        public virtual string DESCRIPTION { get; set; }
    }
}
