using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace EIS.Core.CustomView
{
    public class QuanLyNguoiDungModels
    {
        public int ID { get; set; }
        public long NguoiDungId { get; set; }
        [Required(ErrorMessage = "Nhập tên tài khoản!")]
        [Remote("CheckUserName", "QuanLyNguoiDung",AdditionalFields = "ID", HttpMethod = "POST", ErrorMessage = "Người dùng đã tồn tại, mời bạn nhập nhập tài khoản khác!")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự , tối đa 100 kí tự!", MinimumLength = 6)]
        ////[Required(ErrorMessage = "Nhập mật khẩu!")]
        //[Remote("CheckPassword", "QuanLyNguoiDung", AdditionalFields = "ID", HttpMethod = "POST", ErrorMessage = "Nhập mật khẩu!")]
        [ValidateNguoiDung]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu trùng với mật khẩu!")]
        public string PasswordRe { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Địa chỉ email sai định dạng!")]
        //[Required(ErrorMessage = "Nhập địa chỉ email!")]
        [Remote("CheckEmail", "QuanLyNguoiDung", AdditionalFields = "ID", HttpMethod = "POST", ErrorMessage = "Email đã tồn tại, mời bạn nhập nhập email khác!")]
        public string Email { get; set; }
        [RegularExpression(@"^(\+84|0)\d{9,10}$", ErrorMessage = "Số điện thoại sai định dạng!")]
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Số chứng minh nhân dân sai định dạng!")]
        public string SoCMND { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsLockedOut { get; set; }
        public string Roles { get; set; }
        //[Required(ErrorMessage = "Nhập đơn vị!")]
        public long? DonVi_ID { get; set; }
        public string COSO_KCBID { get; set; }
        //[Required(ErrorMessage = "Nhập vai trò!")]
        public int? VaiTro_ID { get; set; }
        public bool? IsAdmin { get; set; }
        //[Required(ErrorMessage = "Nhập tên người dùng!")]
        public string Ten { get; set; }
        public bool? IsPQ { get; set; }
    }
}
