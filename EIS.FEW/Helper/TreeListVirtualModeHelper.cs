using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.ASPxTreeList;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;

namespace EIS.FEW.Helper
{
    public class TreeListVirtualModeHelper
    {
        //public static void VirtualModeCreateChildren(TreeListVirtualModeCreateChildrenEventArgs e)
        //{
        //    string path = e.NodeObject == null ? Request.MapPath("~/") : e.NodeObject.ToString();

        //    List<string> children = new List<string>();
        //    if (Directory.Exists(path))
        //    {
        //        foreach (string name in Directory.GetDirectories(path))
        //        {
        //            if (!IsSystemName(name))
        //                children.Add(name);
        //        }
        //        foreach (string name in Directory.GetFiles(path))
        //            if (!IsSystemName(name))
        //                children.Add(name);
        //    }
        //    e.Children = children;
        //}
        //public static void VirtualModeNodeCreating(TreeListVirtualModeNodeCreatingEventArgs e)
        //{
        //    string nodePath = e.NodeObject.ToString();
        //    e.NodeKeyValue = GetNodeGuid(nodePath);
        //    e.IsLeaf = !Directory.Exists(nodePath);
        //    e.SetNodeValue("name", Path.GetFileName(nodePath));
        //    e.SetNodeValue("date", Directory.GetCreationTime(nodePath));
        //}

        //static HttpRequest Request { get { return HttpContext.Current.Request; } }
        //static Dictionary<string, Guid> Map
        //{
        //    get
        //    {
        //        const string key = "DX_PATH_GUID_MAP";
        //        if (HttpContext.Current.Session[key] == null)
        //            HttpContext.Current.Session[key] = new Dictionary<string, Guid>();
        //        return HttpContext.Current.Session[key] as Dictionary<string, Guid>;
        //    }
        //}
        //static bool IsSystemName(string name)
        //{
        //    name = Path.GetFileName(name).ToLower();
        //    return name.StartsWith("app_") || name == "bin" || name == "obj";
        //}
        //static Guid GetNodeGuid(string path)
        //{
        //    if (!Map.ContainsKey(path))
        //        Map[path] = Guid.NewGuid();
        //    return Map[path];
        //}

        //public static void VirtualModeCreateChildren(TreeListVirtualModeCreateChildrenEventArgs e)
        //{
        //    var iDmOngService = IoC.Resolve<IDMICDCHUONGService>();
        //    var iDmChaService = IoC.Resolve<IDMICDNHOMService>();
        //    var nhom = new List<NHOMCHA>
        //    {
        //        new NHOMCHA()
        //        {
        //            ID = -1,
        //            NODENAME = "folder",
        //            TEN = "Danh mục nhóm ICD",
        //            NHOMCHA_ID = null
        //        }
        //    };
        //    nhom.AddRange(iDmOngService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "folder",
        //        TEN = "CHƯƠNG " + x.MA + ". " + x.TEN,
        //        NHOMCHA_ID = -1
        //    }).OrderBy(x => x.ID));
        //    nhom.AddRange(iDmChaService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "items",
        //        TEN = x.MA + " - " + x.TEN,
        //        NHOMCHA_ID = x.ICDCHUONG_ID
        //    }).OrderBy(x => x.ID));

        //    var parent = e.NodeObject as NHOMCHA;
        //    e.Children = parent == null ? nhom.Where(x => x.NHOMCHA_ID == null).ToList() : nhom.Where(x => x.NHOMCHA_ID == parent.ID).Select(x => x).ToList();
        //}

        //public static void VirtualModeNodeCreating(TreeListVirtualModeNodeCreatingEventArgs e)
        //{
        //    var iDmOngService = IoC.Resolve<IDMICDCHUONGService>();
        //    var iDmChaService = IoC.Resolve<IDMICDNHOMService>();
        //    var nhom = new List<NHOMCHA>
        //    {
        //        new NHOMCHA()
        //        {
        //            ID = -1,
        //            NODENAME = "folder",
        //            TEN = "Danh mục nhóm ICD",
        //            NHOMCHA_ID = null
        //        }
        //    };
        //    nhom.AddRange(iDmOngService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "folder",
        //        TEN = "CHƯƠNG " + x.MA + ". " + x.TEN,
        //        NHOMCHA_ID = -1
        //    }).OrderBy(x => x.ID));
        //    nhom.AddRange(iDmChaService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "items",
        //        TEN = x.MA + " - " + x.TEN,
        //        NHOMCHA_ID = x.ICDCHUONG_ID
        //    }).OrderBy(x => x.ID));

        //    var node = e.NodeObject as NHOMCHA;

        //    if (node == null)
        //        return;
        //    e.NodeKeyValue = node.ID;
        //    var childEmployeeCount = nhom.Where(x => x.NHOMCHA_ID == node.ID).ToList().Count;
        //    e.IsLeaf = !(childEmployeeCount > 0);
        //    e.SetNodeValue("TEN", node.TEN);
        //    e.SetNodeValue("NODENAME", node.NODENAME);
        //}

        //public static void MoveNode(long id, long? nhomChaId)
        //{
        //    var newParentId = Convert.ToInt64(nhomChaId);
        //    var node = GetNode(id);
        //    if (node.ID == newParentId)
        //        return;
        //    if (newParentId == 0)
        //        node.NHOMCHA_ID = -1;
        //    else
        //        node.NHOMCHA_ID = newParentId;
        //}

        //private static NHOMCHA GetNode(long id)
        //{
        //    var iDmOngService = IoC.Resolve<IDMICDCHUONGService>();
        //    var iDmChaService = IoC.Resolve<IDMICDNHOMService>();
        //    var nhom = new List<NHOMCHA>
        //    {
        //        new NHOMCHA()
        //        {
        //            ID = -1,
        //            NODENAME = "folder",
        //            TEN = "Danh mục nhóm ICD",
        //            NHOMCHA_ID = null
        //        }
        //    };
        //    nhom.AddRange(iDmOngService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "folder",
        //        TEN = "CHƯƠNG " + x.MA + ". " + x.TEN,
        //        NHOMCHA_ID = -1
        //    }).OrderBy(x => x.ID));
        //    nhom.AddRange(iDmChaService.Query.Select(x => new NHOMCHA
        //    {
        //        ID = x.ID,
        //        NODENAME = "items",
        //        TEN = x.MA + " - " + x.TEN,
        //        NHOMCHA_ID = x.ICDCHUONG_ID
        //    }).OrderBy(x => x.ID));

        //    return nhom.FirstOrDefault(x => x.ID == id);
        //}     
    }
}