using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Models
{
    public class TypeControl
    {
       public string MAP { set; get; }
       public string TEN { set; get; }
       public string TYPE { set; get; }
       public int LOCAL { set; get; } // 1: lấy toàn bộ lên trước,
       public string dm { set; get; }
       public List<DropdowListDatasource> listData { set; get; }
    }
}