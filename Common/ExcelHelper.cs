using System.IO;
using System.Data;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Web;
using System.Text;
using System;
using NPOI.XSSF.UserModel;

namespace EADS.Common
{
	public class ExcelHelper
	{
        /// <summary>
        /// 判断Excel是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    ISheet sheet = workbook.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }

        #region DataTable 转换成 Excel

        /// <summary>
        /// DataTable转换成内存流
        /// </summary>
        /// <param name="table">数据源</param>
        /// <returns>内存流</returns>
        public static MemoryStream RenderDataToMemoryStream(DataTable table)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);

                foreach (DataColumn column in table.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                }


                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        
                    }
                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        public static MemoryStream RenderDataToMemoryStream(DataTable table, bool moreColumn)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                XSSFWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                foreach (DataColumn column in table.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                }
                //取得列宽
                int[] arrColWidth = new int[table.Columns.Count];
                foreach (DataColumn item in table.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(table.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                int rowIndex = 1;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        //设置列宽
                        sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                    }
                    rowIndex++;
                }
                workbook.Write(ms);
                //ms.Flush();
                //ms.Position = 0;
            }
            return ms;
        }

        public static MemoryStream RenderDataToMemoryStream(DataSet ds)
        {
            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new HSSFWorkbook();
            foreach (DataTable table in ds.Tables)
            {
                ISheet sheet = workbook.CreateSheet(table.TableName);
                IRow headerRow = sheet.CreateRow(0);
                foreach (DataColumn column in table.Columns)
                {
                    ICell cell = headerRow.CreateCell(column.Ordinal);
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(column.Caption);
                }
                int rowIndex = 1;
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
                cellStyle.Alignment = HorizontalAlignment.Left;
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        ICell cell = dataRow.CreateCell(column.Ordinal);
                        cell.SetCellType(CellType.String);
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                //列宽自适应，只对英文和数字有效
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    //sheet.AutoSizeColumn(i);
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 将内存流保存为文件(保存到服务器)
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <param name="fileName">文件名称</param>
        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }

        /// <summary>
        /// 将文件流输出到浏览器
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.ContentType = "application/ms-excel";
            context.Response.BinaryWrite(ms.ToArray());
        }

        #endregion

        #region Excel 转换成 DataTable

        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        /// <summary>
        /// Excel文件流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream"></param>
        /// <returns></returns>
        public static DataTable ConvertExcelToDataTable(Stream excelFileStream,ref string msg)
        {
            bool pass = true;
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                ISheet sheet = workbook.GetSheetAt(0);//取第一个表
                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行
                int cellCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    string columnname = string.Empty;
                    if (headerRow.GetCell(i) != null && !string.IsNullOrWhiteSpace(headerRow.GetCell(i).StringCellValue))
                    {
                        columnname = headerRow.GetCell(i).StringCellValue;
                        if (table.Columns.Contains(columnname))
                        {
                            pass = false;
                            msg = "第" + (i + 1) + "列和其他列重复";
                        }
                    }
                    else
                    {
                        pass = false;
                        msg = "第" + (i + 1) + "列不能为空";
                    }
                    if (pass)
                    {
                        DataColumn column = new DataColumn(columnname);
                        table.Columns.Add(column);
                    }
                    else
                    {
                        break;
                    }
                }

                if (pass)
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dataRow = table.NewRow();
                        bool isEmptyRow = true;

                        if (row != null)
                        {
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    dataRow[j] = GetCellValue(row.GetCell(j));
                                    isEmptyRow = false;
                                }
                            }
                            if (!isEmptyRow)
                            {
                                table.Rows.Add(dataRow);
                            }
                        }
                    }
                    return table;
                }
                else 
                {
                    return null;
                }
                
            }
        }

        public static DataSet ConvertExcelToDataSet(Stream excelFileStream, ref string msg)
        {
            bool pass = true;
            using (excelFileStream)
            {
                DataSet dsResult = new DataSet();
                try
                {
                    #region
                    IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                    int sheetNumber = workbook.NumberOfSheets;
                    for (int sheetIndex = 0; sheetIndex < sheetNumber; sheetIndex++)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);
                        DataTable table = new DataTable();
                        table.TableName = sheet.SheetName;
                        IRow headerRow = sheet.GetRow(0);
                        if (headerRow != null)
                        {
                            int cellCount = headerRow.LastCellNum;
                            int rowCount = sheet.LastRowNum;
                            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                            {
                                string columnname = string.Empty;
                                if (headerRow.GetCell(i) != null)
                                {
                                    columnname = GetCellValue(headerRow.GetCell(i));
                                    if (table.Columns.Contains(columnname))
                                    {
                                        pass = false;
                                        msg = "第" + (i + 1) + "列和其他列重复";
                                    }
                                }
                                else
                                {
                                    pass = false;
                                    msg = "第" + (i + 1) + "列不能为空";
                                }
                                if (pass)
                                {
                                    DataColumn column = new DataColumn(columnname);
                                    table.Columns.Add(column);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (pass)
                            {
                                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                                {
                                    IRow row = sheet.GetRow(i);
                                    DataRow dataRow = table.NewRow();

                                    if (row != null)
                                    {
                                        for (int j = row.FirstCellNum; j < cellCount; j++)
                                        {
                                            if (row.GetCell(j) != null)
                                            {
                                                dataRow[j] = GetCellValue(row.GetCell(j));
                                            }
                                        }
                                        //多虑掉空行
                                        bool isEmptyRow = true;
                                        foreach (DataColumn column in table.Columns)
                                        {
                                            string tempVal = Convert.IsDBNull(dataRow[column]) ? "" : dataRow[column].ToString();
                                            if (tempVal.Length > 0)
                                            {
                                                isEmptyRow = false;
                                                break;
                                            }
                                        }
                                        if (!isEmptyRow)
                                        {
                                            table.Rows.Add(dataRow);
                                        }
                                    }
                                }
                                dsResult.Tables.Add(table);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            dsResult.Tables.Add(table);
                        }
                    }
                    #endregion
                }
                catch (Exception exp)
                {
                    msg = exp.Message;
                    pass = false;
                }
                return dsResult;
            }
        }

        #endregion

         
        public static MemoryStream GetExhibitorAddModel(string[] areas)
        {
            MemoryStream ms = new MemoryStream(); 
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            headerRow.Height = 30*20;
                 
            ICell cell1 = headerRow.CreateCell(0);
            cell1.SetCellValue("公司名称");                
            ICell cell2 = headerRow.CreateCell(1);
            cell2.SetCellValue("所在展区");                
            ICell cell3 = headerRow.CreateCell(2);
            cell3.SetCellValue("展位号");
                 
            sheet.SetColumnWidth(0, 60 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(2, 20 * 256);

            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.FillBackgroundColor = HSSFColor.Grey25Percent.Index;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont font12 = workbook.CreateFont();
            font12.FontHeightInPoints = 12;
            font12.FontName = "微软雅黑";
            cellStyle.SetFont(font12);

            cell1.CellStyle = cellStyle;
            cell2.CellStyle = cellStyle;
            cell3.CellStyle = cellStyle;
                 
            NPOI.SS.Util.CellRangeAddressList regions = new NPOI.SS.Util.CellRangeAddressList(1, 65535,1,1);
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(areas);
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            sheet.AddValidationData(dataValidate);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0; 

            return ms;

             
        }

    }
}
