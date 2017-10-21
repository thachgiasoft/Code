using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EIS.FEW.Utils
{
    public class Utils
    {
        public static string ConvertToUnsign(string str)
        {
            string[] signs = new string[] { 
                    "aAeEoOuUiIdDyY",
                    "áàạảãâấầậẩẫăắằặẳẵ",
                    "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                    "éèẹẻẽêếềệểễ",
                    "ÉÈẸẺẼÊẾỀỆỂỄ",
                    "óòọỏõôốồộổỗơớờợởỡ",
                    "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                    "úùụủũưứừựửữ",
                    "ÚÙỤỦŨƯỨỪỰỬỮ",
                    "íìịỉĩ",
                    "ÍÌỊỈĨ",
                    "đ",
                    "Đ",
                    "ýỳỵỷỹ",
                    "ÝỲỴỶỸ"
               };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
                }
            }
            return str;
        }
        public static System.IO.MemoryStream createExcelReport(DataSet ds)
        {
            using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
            {
                SpreadsheetDocument workbook = SpreadsheetDocument.
                    Create(mem, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = workbook.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                foreach (System.Data.DataTable table in ds.Tables)
                {

                    var sheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    sheetPart.Worksheet = new Worksheet(new SheetData());
                    var sheetData = sheetPart.Worksheet.GetFirstChild<SheetData>();
                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());// workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                  
                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);
                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            if (col.GetType() == typeof(Decimal))
                                cell.DataType = CellValues.Number;
                            else
                                cell.DataType = CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                    uint sheetId = 1;
                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);
                }
                workbookpart.Workbook.Save();
                workbook.Close();
                return mem;
            }
        }
        private static Border GenerateBorder()
        {
            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color1 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color1);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color2 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color2);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color3);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color4);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            return border2;
        }
        public static string getFromHttp(string url)
        {
            var rt = "";
            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;

            //request.Accept = "application/xrds+xml";  
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

            System.Net.WebHeaderCollection header = response.Headers;

            var encoding = System.Text.ASCIIEncoding.UTF8;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(responseText) && responseText != "[]" && responseText.Length>2)
                {
                    responseText = responseText.Replace("\\", "");
                    responseText = responseText.Substring(1, responseText.Length - 2);
                }
                rt = responseText;
            }
            return rt;
        }

        public static string handerExepsion(){
            string str = "";
            return str;
        }
        public static List<EIS.FEW.Models.DropdowListDatasource> getListThangQuyNam(int type)
        {
            List<EIS.FEW.Models.DropdowListDatasource> lst = new List<EIS.FEW.Models.DropdowListDatasource>();
            int start = 12;
            int limit= 12;
            if (type == 4)
            {
                start = 4;
                limit = 4;
            }
            else if (type == 5)
            {
                DateTime dNow = DateTime.Now;
                start = dNow.Year;
                limit = 5;
            }
            while (limit > 0)
            {
                limit--;
                var a = new EIS.FEW.Models.DropdowListDatasource();
                var value = start - limit;
                a.value = value.ToString();
                a.text = value.ToString();
                lst.Add(a);
            }
            if (type == 5)
            {
               lst= lst.OrderByDescending(t => int.Parse(t.value)).ToList();
            }
            return lst;
        } 
    }
}