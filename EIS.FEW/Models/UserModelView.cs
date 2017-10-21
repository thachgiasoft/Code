using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIS.FEW.Models
{
    public class UserModelView
    {
        public int userid { get; set; }
        public string username { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }

        [RegularExpression(@"(?=.*\d)(?=.*[A-Z])(?=.*\W).{8,100}", ErrorMessage = "Mật khẩu mới phải chứa tối thiểu 8 kí tự, ít nhất một kí tự HOA, một số và một kí tự đặc biệt!")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Nhập mật khẩu mới!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu mới không trùng với mật khẩu mới!")]
        public string PasswordRe { get; set; }
    }
}