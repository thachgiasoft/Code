using DevExpress.Web;
using System.Web;
using System.Web.SessionState;

namespace EIS.FEW.Helper
{
    public class GridViewSelectionHelper
    {
        const string SelectAllModeSessionKey = "4C0A9E6A-5D76-48F9-9086-CD5E9D481928";

        public static GridViewSelectAllCheckBoxMode SelectAllButtonMode
        {
            get
            {
                if (Session[SelectAllModeSessionKey] == null)
                    Session[SelectAllModeSessionKey] = GridViewSelectAllCheckBoxMode.Page;
                return (GridViewSelectAllCheckBoxMode)Session[SelectAllModeSessionKey];
            }
            set
            {
                Session[SelectAllModeSessionKey] = value;
            }
        }

        static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    }
}