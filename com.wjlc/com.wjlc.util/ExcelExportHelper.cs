using System;
using System.IO;
using System.Web;
using System.Data;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace com.wjlc.util
{
    public class ExcelExportHelper
    {
        HSSFWorkbook hssfworkbook = new HSSFWorkbook();

        /// <summary>
        /// 导出 XLS 文件
        /// </summary>
        /// <param name="sheetName">sheet 名称</param>
        /// <param name="titleArray">标题列表</param>
        /// <param name="dTable">导出数据</param>
        public void CreateXLS(string sheetName, string[] titleArray, DataTable dTable)
        {
            var sheet = hssfworkbook.CreateSheet(sheetName);
            int rowid = 0, n = 1;
            IRow row = sheet.CreateRow(rowid);

            //  创建第一行标题
            ICell cell;
            ICellStyle icellStyle = HanderStyle(hssfworkbook.CreateCellStyle(), hssfworkbook.CreateFont(), HSSFColor.RoyalBlue.Index, HSSFColor.White.Index, 240);
            for (int i = 0; i < titleArray.Length; i++)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue(titleArray[i]);
                cell.CellStyle = icellStyle;
            }

            //  写入数据
            foreach (DataRow item in dTable.Rows)
            {
                row = sheet.CreateRow(++rowid);
                for (int i = 0; i < titleArray.Length; i++)
                {
                    if (i == 0)
                        row.CreateCell(i).SetCellValue(n);
                    else
                        row.CreateCell(i).SetCellValue(item[i-1].ToString());
                }
                n++;
            }

            //  下载 XLS 文件
            using (MemoryStream ms = new MemoryStream())
            {
                hssfworkbook.Write(ms);
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}_{1}.xls", sheetName, DateTime.Now.ToString("yy_MM_ddHHmmss")));
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
        }

        /// <summary>
        /// 设置标题样式
        /// </summary>
        /// <returns></returns>
        private ICellStyle HanderStyle(ICellStyle cellStyle, IFont font, short fillColor, short color, short height)
        {
            cellStyle.FillForegroundColor = fillColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Center;

            font.FontHeight = height;
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.FontName = "微软雅黑";
            font.Color = color;
            cellStyle.SetFont(font);
            return cellStyle;
        }
    }
}
