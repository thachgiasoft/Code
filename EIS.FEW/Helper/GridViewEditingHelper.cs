using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using DevExpress.Web;

namespace EIS.FEW.Helper
{
    public class GridViewEditingHelper
    {
        const string
            EditingModeSessionKey = "AA054892-1B4C-4158-96F7-894E1545C05C",
            BatchEditModeSessionKey = "E509E30E-99E3-4CB3-A07B-A08B04784A46",
            BatchStartEditActionSessionKey = "F2014F18-18A5-42F2-B713-B1538D1D1720";

        public static GridViewEditingMode EditMode
        {
            get
            {
                if (Session[EditingModeSessionKey] == null)
                    Session[EditingModeSessionKey] = GridViewEditingMode.PopupEditForm;
                return (GridViewEditingMode)Session[EditingModeSessionKey];
            }
            set { HttpContext.Current.Session[EditingModeSessionKey] = value; }
        }
        static List<GridViewEditingMode> availableEditModesList;
        public static List<GridViewEditingMode> AvailableEditModesList
        {
            get
            {
                if (availableEditModesList == null)
                    availableEditModesList = new List<GridViewEditingMode> {
                        GridViewEditingMode.Inline,
                        GridViewEditingMode.EditForm,
                        GridViewEditingMode.EditFormAndDisplayRow,
                        GridViewEditingMode.PopupEditForm
                    };
                return availableEditModesList;
            }
        }

        public static GridViewBatchEditMode BatchEditMode
        {
            get
            {
                if (Session[BatchEditModeSessionKey] == null)
                    Session[BatchEditModeSessionKey] = GridViewBatchEditMode.Cell;
                return (GridViewBatchEditMode)Session[BatchEditModeSessionKey];
            }
            set { Session[BatchEditModeSessionKey] = value; }
        }
        public static GridViewBatchStartEditAction BatchStartEditAction
        {
            get
            {
                if (Session[BatchStartEditActionSessionKey] == null)
                    Session[BatchStartEditActionSessionKey] = GridViewBatchStartEditAction.Click;
                return (GridViewBatchStartEditAction)Session[BatchStartEditActionSessionKey];
            }
            set { Session[BatchStartEditActionSessionKey] = value; }
        }
        protected static HttpSessionState Session { get { return HttpContext.Current.Session; } }
    }
}