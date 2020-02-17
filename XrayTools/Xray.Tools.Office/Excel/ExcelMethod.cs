#region 说明
//---------------------------------------名称:封装的Excel通用操作类 基于Aspose.Cells.dll
//---------------------------------------依赖DLL:Aspose.Cells.dll
//---------------------------------------版本:1.0.0.0
//---------------------------------------更新时间:2017/11/10
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Xray.Tools.Office.Excel
{
    public class ExcelMethod
    {

        #region 常规方式 没有样式
        /// <summary>
        /// 创建具有行列(可选)标题的Excel文件
        /// </summary>
        /// <param name="filename">保存路径 有后缀名</param>
        /// <param name="Columns">列标题</param>
        /// <param name="Rows">行标题</param>
        /// <param name="FillModel">对第一个单元格的填充方式</param>
        /// <param name="SheetIndex">Sheet索引  默认使用第一个</param>
        /// <returns></returns>
        public static Workbook CreateExcel(String filename, String[] Columns = null, String[] Rows = null, ExcelFormat.FillModel FillModel = ExcelFormat.FillModel.空出第一个单元格, int SheetIndex = 0)
        {
            //实例化workbook对象
            Workbook workbook = new Workbook();
            //获得工作簿中的表
            Worksheet sheet = (Worksheet)workbook.Worksheets[SheetIndex];
            //获得指定Sheet中的所有单元格
            Cells cells = sheet.Cells;

            //有列标题
            if (Columns != null)
            {
                for (int i = 0; i < Columns.Length; i++)
                {
                    if (FillModel == ExcelFormat.FillModel.列标题填充)
                    {
                        //默认列宽25
                        cells.SetColumnWidth(i, 25);
                        //单元格赋值
                        cells[0, i].PutValue(Columns[i]);
                    }
                    else
                    {
                        //默认列宽25
                        cells.SetColumnWidth(i + 1, 25);
                        //单元格赋值
                        cells[0, i + 1].PutValue(Columns[i]);
                    }
                }
            }
            //有行标题
            if (Rows != null)
            {
                for (int i = 0; i < Rows.Length; i++)
                {
                    if (FillModel == ExcelFormat.FillModel.行标题填充)
                    {
                        //默认行高10
                        cells.SetColumnWidth(i, 10);
                        //单元格赋值
                        cells[i, 0].PutValue(Rows[i]);
                    }
                    else
                    {
                        //默认行高10
                        cells.SetColumnWidth(i + 1, 10);
                        //单元格赋值
                        cells[i + 1, 0].PutValue(Rows[i]);
                    }

                }
            }

            return workbook;
        }
        /// <summary>
        /// 内容数据插入 传入基类单元格数据 没有样式
        /// <param name="data">传入基类单元格数据</param>
        /// <param name="book">Workbook 对象</param>
        /// <param name="SavePath">保存路径 带后缀名</param>
        /// <param name="Scale">图片比例</param>
        /// <param name="SheetIndex">Sheet索引 默认为0</param>
        /// </summary>
        public static void ExcelDataInsert(IList<CellBase> data, Workbook book, String SavePath, int Scale = 20, int SheetIndex = 0)
        {
            //获得目标Sheet
            var sheet = book.Worksheets[SheetIndex];
            //获得目标Sheet的所有单元格
            var cells = book.Worksheets[SheetIndex].Cells;

            data.ToList().ForEach(item =>
            {
                if (item.IsPic)
                {
                    sheet.Pictures.Add(item.X, item.Y, item.Image_Ms, Scale, Scale);
                }
                else
                {
                    cells[item.X, item.Y].PutValue(item.Txt_Obj);
                }
            });

            //Parallel.ForEach(data, item => { 
            //   if(item.IsPic)
            //   {
            //       sheet.Pictures.Add(item.X, item.Y, item.Image_Ms);
            //   }
            //    else
            //   {
            //       cells[item.X,item.Y].PutValue(item.Txt_Obj);
            //   }
            //});

            try
            {
                book.Save(SavePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("保存失败");
                throw ex;
            }

        }
        #endregion

        #region 使用模板
        /// <summary>
        /// 使用指定的Excel模板创建Excel文件 标题可以直接写入SCells中
        /// </summary>
        /// <param name="FormatType">模板</param>
        /// <param name="Save">是否直接保存</param>
        /// <returns></returns>
        [Obsolete("已过时  请使用InsertData 新版不再将标题单独区分")]
        public static Workbook CreateExcel(ExcelFormat FormatType, bool Save = true)
        {
            //实例化workbook对象
            Workbook workbook = new Workbook();
            //获得工作簿中的表
            Worksheet sheet = (Worksheet)workbook.Worksheets[FormatType.SheetIndex];
            //获得指定Sheet中的所有单元格
            Cells cells = sheet.Cells;

            //有行标题
            if (FormatType.Columns.Count > 0)
            {
                //设置第一行行高
                cells.SetRowHeight(0, FormatType.RowsSize);
                for (int i = 0; i < FormatType.Columns.Count; i++)
                {
                    //设置列宽
                    cells.SetColumnWidth(i, FormatType.ColumnsSize);
                    //单元格赋值
                    cells[0, FormatType.Columns[i].Y].PutValue(FormatType.Columns[i].Txt_Obj);
                    //设置样式
                    cells[0, FormatType.Columns[i].Y].SetStyle(FormatType.Columns[i].CreateStyle());
                }
            }
            //有行标题
            if (FormatType.Rows.Count > 0)
            {
                //设置第一列列宽
                cells.SetColumnWidth(0, FormatType.ColumnsSize);
                for (int i = 0; i < FormatType.Rows.Count; i++)
                {
                    //设置行高
                    cells.SetRowHeight(i, FormatType.RowsSize);
                    //单元格赋值
                    cells[FormatType.Rows[i].X, 0].PutValue(FormatType.Rows[i].Txt_Obj);
                    //设置样式
                    cells[FormatType.Rows[i].X, 0].SetStyle(FormatType.Rows[i].CreateStyle());
                    //cells[i, 0].SetStyle(FormatType.Rows[i].CreateStyle());
                }
            }

            if (FormatType.SCells.Count > 0)
            {
                foreach (var cell in FormatType.SCells)
                {
                    //设置行高
                    cells.SetRowHeight(cell.X, FormatType.RowsSize);
                    //设置列宽
                    cells.SetColumnWidth(cell.Y, FormatType.ColumnsSize);
                    //设置文字换行
                    //单元格赋值
                    cells[cell.X, cell.Y].PutValue(cell.Txt_Obj);
                    //设置样式
                    cells[cell.X, cell.Y].SetStyle(cell.CreateStyle());
                }
            }

            if (Save)
            {
                try
                {
                    workbook.Save(FormatType.SavePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("保存失败");
                    throw ex;
                }
            }

            return workbook;
        }

        /// <summary>
        /// 插入数据到Excel
        /// </summary>
        /// <param name="EFormat">Excel模板</param>
        /// <param name="Save">是否自动保存</param>
        /// <param name="book">Workbook对象</param>
        /// <returns></returns>
        public static void InsertData(ExcelFormat EFormat, bool Save = true, Workbook book = null)
        {
            if (book == null)
            {
                book = new Workbook();
            }
            if (String.IsNullOrEmpty(EFormat.SavePath))
            {
                Exception ex = new Exception("保存路径为空");
                throw ex;
            }
            var cells = book.Worksheets[EFormat.SheetIndex].Cells;

            if (EFormat.SCells.Count > 0)
            {
                foreach (var cell in EFormat.SCells)
                {
                    //设置行高
                    cells.SetRowHeight(cell.X, EFormat.RowsSize);
                    //设置列宽
                    cells.SetColumnWidth(cell.Y, EFormat.ColumnsSize);
                    //单元格赋值
                    cells[cell.X, cell.Y].PutValue(cell.Txt_Obj);
                    //设置样式
                    cells[cell.X, cell.Y].SetStyle(cell.CreateStyle());
                }
            }
            if (Save)
            {
                try
                {
                    book.Save(EFormat.SavePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"保存失败:{ex.Message}");
                }
            }
        }


        #endregion
    }
}
