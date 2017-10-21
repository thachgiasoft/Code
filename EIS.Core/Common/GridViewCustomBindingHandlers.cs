using DevExpress.Data;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Linq;


namespace EIS.Core.Common
{
   public class GridViewCustomBindingHandlers
    {
        public IQueryable Model { get; set; }
        public void GetDataRowCountSimple(GridViewCustomBindingGetDataRowCountArgs e)
        {
            e.DataRowCount = Model.Count();
        }
        public void GetDataSimple(GridViewCustomBindingGetDataArgs e)
        {
            e.Data = Model
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }

        public void GetDataRowCountAdvanced(GridViewCustomBindingGetDataRowCountArgs e)
        {
            int rowCount;
            if (GridViewCustomBindingSummaryCache.TryGetCount(e.FilterExpression, out rowCount))
                e.DataRowCount = rowCount;
            else
                e.DataRowCount = Model.ApplyFilter(e.FilterExpression).Count();
        }
        public void GetUniqueHeaderFilterValuesAdvanced(GridViewCustomBindingGetUniqueHeaderFilterValuesArgs e)
        {
            e.Data = Model
                .ApplyFilter(e.FilterExpression)
                .UniqueValuesForField(e.FieldName);
        }
        public void GetGroupingInfoAdvanced(GridViewCustomBindingGetGroupingInfoArgs e)
        {
            e.Data = Model
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList)
                .GetGroupInfo(e.FieldName, e.SortOrder);
        }
        public void GetDataAdvanced(GridViewCustomBindingGetDataArgs e)
        {
            e.Data = Model
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList)
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }
        public void GetSummaryValuesAdvanced(GridViewCustomBindingGetSummaryValuesArgs e)
        {
            var query = Model
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList);

            var summaryValues = query.CalculateSummary(e.SummaryItems);
            e.Data = summaryValues;

            var countSummaryItem = e.SummaryItems.FirstOrDefault(i => i.SummaryType == SummaryItemType.Count);
            if (e.GroupInfoList.Count == 0 && countSummaryItem != null)
            {
                var itemIndex = e.SummaryItems.IndexOf(countSummaryItem);
                var count = summaryValues[itemIndex] != null ? Convert.ToInt32(summaryValues[itemIndex]) : 0;
                GridViewCustomBindingSummaryCache.SaveCount(e.FilterExpression, count);
            }
        }
    }
}
