using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIS.Core.Domain
{
    public partial class NGUOIDUNG
    {
        public virtual long ID { get; set; }
        public virtual string TEN { get; set; }
        public virtual string TENDANGNHAP { get; set; }
        public virtual int? VAITRO { get; set; }
        public virtual int TRANGTHAI { get; set; }
        public virtual long? DONVI_ID { get; set; }
        public virtual long? DF_COSOKCB_ID { get; set; }
        public virtual int? DF_LOAITG { get; set; }
        public virtual int? DF_THANG { get; set; }
        public virtual int? DF_QUY { get; set; }
        public virtual int? DF_NAM { get; set; }
        public virtual string PHONE { get; set; }
        public virtual string ADDRESS { get; set; }
        public virtual string SOCMT { get; set; }
        public virtual bool? ISPQ { get; set; }

        public virtual DM_DONVI DONVI { get; set; }
        public virtual DM_COSOKCB COSOKCB { get; set; }
        public virtual IList<DM_COSOKCB> COSOKCBS { get; set; }
            
    }
}
