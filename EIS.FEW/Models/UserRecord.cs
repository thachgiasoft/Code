using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIS.FEW.Models
{
    public class UserRecord
    {
        public int userid { get; set; }
        public string username { get; set; }

        [Display(Name = "Mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Nhập mật khẩu hiện tại!")]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Nhập mật khẩu mới!")]
        [StringLength(100, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự , tối đa 100 kí tự!", MinimumLength = 6)]
        public string NewPassword { get; set; }


        [Display(Name = "Nhập lại mật khẩu mới")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu mới không trùng với mật khẩu mới!")]
        public string PasswordRe { get; set; }
        public string FullName { get; set; }
    }
}