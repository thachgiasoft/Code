using System;
using System.ComponentModel.DataAnnotations;

namespace EIS.Core.CustomView
{
    public class ValidateNguoiDungAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var property = context.ObjectType.GetProperty("ID");
            var id = property.GetValue(context.ObjectInstance, null);

            if (Convert.ToInt64(id) != 0)
            {
                return ValidationResult.Success;
            }
            //return value != null ? ValidationResult.Success : new ValidationResult("Nhập" + context.DisplayName + "!");
            return value != null ? ValidationResult.Success : new ValidationResult("Nhập mật khẩu !");
        }
    }
}
