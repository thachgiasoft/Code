using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace EIS.Core.Common
{
    public static class GridViewCustomOperationDataHelper
    {
        static ICriteriaToExpressionConverter Converter { get { return new CriteriaToExpressionConverter(); } }

        public static IQueryable Select(this IQueryable query, string fieldName)
        {
            return query.MakeSelect(Converter, new OperandProperty(fieldName));
        }

        public static IQueryable ApplySorting(this IQueryable query, IEnumerable<GridViewColumnState> sortedColumns)
        {
            ServerModeOrderDescriptor[] orderDescriptors = sortedColumns
                .Select(c => new ServerModeOrderDescriptor(new OperandProperty(c.FieldName), c.SortOrder == ColumnSortOrder.Descending))
                .ToArray();
            return query.MakeOrderBy(Converter, orderDescriptors);
        }

        public static IQueryable ApplyFilter(this IQueryable query, IList<GridViewGroupInfo> groupInfoList)
        {
            var criteria = GroupOperator.And(
                groupInfoList.Select(i => new BinaryOperator(i.FieldName, i.KeyValue, BinaryOperatorType.Equal))
            );
            return query.ApplyFilter(CriteriaOperator.ToString(criteria));
        }
        public static IQueryable ApplyFilter(this IQueryable query, string filterExpression)
        {
            return query.AppendWhere(Converter, CriteriaOperator.Parse(filterExpression));
        }

        public static IQueryable UniqueValuesForField(this IQueryable query, string fieldName)
        {
            query = query.Select(fieldName);
            var expression = Expression.Call(typeof(Queryable), "Distinct", new Type[] { query.ElementType }, query.Expression);
            return query.Provider.CreateQuery(expression);
        }

        public static IEnumerable<GridViewGroupInfo> GetGroupInfo(this IQueryable query, string fieldName, ColumnSortOrder order)
        {
            var rowType = query.ElementType;
            query = query.MakeGroupBy(Converter, new OperandProperty(fieldName));
            query = query.MakeOrderBy(Converter, new ServerModeOrderDescriptor(new OperandProperty("Key"), order == ColumnSortOrder.Descending));
            query = query.ApplyGroupInfoExpression(rowType);

            var list = new List<GridViewGroupInfo>();
            foreach (var item in query)
            {
                var obj = (object[])item;
                list.Add(new GridViewGroupInfo() { KeyValue = obj[0], DataRowCount = (int)obj[1] });
            }
            return list;
        }

        public static object[] CalculateSummary(this IQueryable query, List<GridViewSummaryItemState> summaryItems)
        {
            var elementType = query.ElementType;

            query = query.MakeGroupBy(Converter, new OperandValue(0));
            var groupParam = Expression.Parameter(query.ElementType, string.Empty);

            var expressions = GetAggregateExpressions(elementType, summaryItems, groupParam);
            query = query.ApplyExpressions(expressions, groupParam);

            var groupValue = query.ToArray();
            return groupValue.Length > 0 ? groupValue[0] as object[] : new object[summaryItems.Count];
        }

        static List<Expression> GetAggregateExpressions(Type elementType, List<GridViewSummaryItemState> summaryItems, ParameterExpression groupParam)
        {
            var list = new List<Expression>();
            var elementParam = Expression.Parameter(elementType, "elem");
            foreach (var item in summaryItems)
            {
                Expression e;
                LambdaExpression elementExpr = null;
                if (!string.IsNullOrEmpty(item.FieldName))
                    elementExpr = Expression.Lambda(Converter.Convert(elementParam, new OperandProperty(item.FieldName)), elementParam);

                switch (item.SummaryType)
                {
                    case SummaryItemType.Count:
                        e = Expression.Call(typeof(Enumerable), "Count", new Type[] { elementType }, groupParam);
                        break;
                    case SummaryItemType.Sum:
                        e = Expression.Call(typeof(Enumerable), "Sum", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Min:
                        e = Expression.Call(typeof(Enumerable), "Min", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Max:
                        e = Expression.Call(typeof(Enumerable), "Max", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    case SummaryItemType.Average:
                        e = Expression.Call(typeof(Enumerable), "Average", new Type[] { elementType }, groupParam, elementExpr);
                        break;
                    default:
                        throw new NotSupportedException(item.SummaryType.ToString());
                }
                list.Add(e);
            }
            return list;
        }

        static IQueryable ApplyExpressions(this IQueryable query, IEnumerable<Expression> expressions, ParameterExpression param)
        {
            var combinedExpr = Expression.NewArrayInit(typeof(object), expressions.Select(expr => Expression.Convert(expr, typeof(object))).ToArray());
            return query.ApplyExpression(combinedExpr, param);
        }

        static IQueryable ApplyExpression(this IQueryable query, Expression expression, ParameterExpression param)
        {
            var lambda = Expression.Lambda(expression, param);
            var callExpr = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, lambda.Body.Type }, query.Expression, Expression.Quote(lambda));
            return query.Provider.CreateQuery(callExpr);
        }

        static object[] ToArray(this IQueryable query)
        {
            var list = new ArrayList();
            foreach (var item in query)
                list.Add(item);
            return list.ToArray();
        }

        static IQueryable ApplyGroupInfoExpression(this IQueryable query, Type rowType)
        {
            var param = Expression.Parameter(query.ElementType, string.Empty);
            return query.ApplyExpressions(new Expression[] {
                Expression.Property(param, "Key"),
                Expression.Call(typeof(Enumerable), "Count", new Type[] { rowType }, param) },
            param);
        }
    }

}

