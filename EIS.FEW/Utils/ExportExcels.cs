
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;
using NPOI.XSSF.UserModel;

namespace EIS.FEW.Utils
{
    public class ExportExcels
    {

        public static System.IO.MemoryStream createExcelReport(DataTable dt)
        {
    
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
            {
                var workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                #region Cell Styles
                #region HeaderLabel Cell Style
                var headerLabelCellStyle = workbook.CreateCellStyle();
                headerLabelCellStyle.Alignment = HorizontalAlignment.Center;

                headerLabelCellStyle.BorderBottom = BorderStyle.Thin;
                headerLabelCellStyle.BorderRight = BorderStyle.Thin;
                headerLabelCellStyle.BorderTop = BorderStyle.Thin;
                headerLabelCellStyle.BorderLeft = BorderStyle.Thin;
                var headerLabelFont = workbook.CreateFont();
                headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
                headerLabelCellStyle.SetFont(headerLabelFont);
                #endregion

                #region RightAligned Cell Style
                var rightAlignedCellStyle = workbook.CreateCellStyle();
                rightAlignedCellStyle.Alignment = HorizontalAlignment.Right;
                rightAlignedCellStyle.BorderBottom = BorderStyle.Thin;
                rightAlignedCellStyle.BorderRight = BorderStyle.Thin;
                rightAlignedCellStyle.BorderTop = BorderStyle.Thin;
                rightAlignedCellStyle.BorderLeft = BorderStyle.Thin;
                #endregion

                #region Currency Cell Style
                var CellStyleRight = workbook.CreateCellStyle();
                CellStyleRight.Alignment = HorizontalAlignment.Right;
                CellStyleRight.BorderBottom = BorderStyle.Thin;
                CellStyleRight.BorderRight = BorderStyle.Thin;
                CellStyleRight.BorderTop = BorderStyle.Thin;
                CellStyleRight.BorderLeft = BorderStyle.Thin;
                #endregion

               
                #region Detail Currency Subtotal Style
                var CellStyleNormal = workbook.CreateCellStyle();
                CellStyleNormal.BorderBottom = BorderStyle.Thin;
                CellStyleNormal.BorderRight = BorderStyle.Thin;
                CellStyleNormal.BorderTop = BorderStyle.Thin;
                CellStyleNormal.BorderLeft = BorderStyle.Thin;
                var detailCurrencySubtotalFont = workbook.CreateFont();
                detailCurrencySubtotalFont.Boldweight = (short)FontBoldWeight.Normal;
                CellStyleNormal.SetFont(detailCurrencySubtotalFont);
                #endregion
                #endregion
                var sheet = workbook.CreateSheet(dt.TableName);
                var rowIndex = 0;
                var row = sheet.CreateRow(rowIndex);
                List<String> columns = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columns.Add(dt.Columns[i].ColumnName);
                    var cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = headerLabelCellStyle;
                }
                rowIndex++;

                //// Add data rows
                foreach (System.Data.DataRow item in dt.Rows)
                {
                    row = sheet.CreateRow(rowIndex);
                    for (int ii = 0; ii < dt.Columns.Count; ii++)
                    {
                        var cell = row.CreateCell(ii);
                        cell.SetCellValue(dt.Columns[ii].ColumnName);
                        if (dt.Columns[ii].DataType == typeof(System.Decimal))
                            cell.CellStyle = CellStyleRight;
                        else
                            cell.CellStyle = CellStyleNormal;
                        cell.SetCellValue(item[dt.Columns[ii].ColumnName].ToString());
                    }
                    rowIndex++;
                }
                for (var i = 0; i < sheet.GetRow(0).LastCellNum; i++)
                    sheet.AutoSizeColumn(i);
               workbook.Write(mem);

                return mem;
            }
        }
    }
}