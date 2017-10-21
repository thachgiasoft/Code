using System.ComponentModel.DataAnnotations;

namespace EIS.Core.CustomView
{
    public class ObjectPermission
    {
        public int AppID { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn một nhóm chức năng!")]
        public int ObjectId { get; set; }
        public int PermissionId { get; set; }
        [Required(ErrorMessage = "Nhập tên chức năng!")]
        public string PmsName { get; set; }
        public string ObjName { get; set; }
        [Required(ErrorMessage = "Nhập mô tả tác vụ!")]
        public string DesPms { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn một loại chức năng!")]
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string MultiTypeID { get; set; }
        public string MultiTypeName { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn một loại tác vụ!")]
        public int LoaiPermission { get; set; }

    }
}
