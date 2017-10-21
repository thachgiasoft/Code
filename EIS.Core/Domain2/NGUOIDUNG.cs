using EIS.Core.CustomView;
using System.ComponentModel.DataAnnotations;

namespace EIS.Core.Domain
{
    public partial class NGUOIDUNG
    {
        public virtual KyFilterSession Time { get; set; }
        public virtual string CapImage { get; set; }
        [Required(ErrorMessage = "Varification code is required.")]
        //[System.ComponentModel.DataAnnotations.Compare("CapImageText", ErrorMessage = "Captcha code Invalid")]
        public virtual string CaptchaCodeText { get; set; }
        public virtual string CapImageText { get; set; }
    }
}
