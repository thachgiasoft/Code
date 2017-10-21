using System;
using IdentityManagement.Domain;

namespace EIS.Core.Domain
{
    public class LogSystem
    {
        public virtual user User { get; set; }

        public virtual int ID { get; set; }

        public virtual int ADMINID { get; set; }

        public virtual string EVENT { get; set; }

        public virtual DateTime CREATEDATE { get; set; }

        public virtual string COMMENT_LOG { get; set; }

        public virtual string IPADDRESS { get; set; }

        public virtual string BROWSER { get; set; }
        public virtual short? TRANGTHAI { get; set; }
    }
}
