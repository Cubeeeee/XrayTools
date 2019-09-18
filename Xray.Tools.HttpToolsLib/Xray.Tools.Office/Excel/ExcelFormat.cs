#region 说明
//---------------------------------------名称:可自定义的Excel格式模板
//---------------------------------------版本:1.0.0.0
//---------------------------------------更新时间:2017/11/10
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace Xray.Tools.Office.Excel
{
    /// <summary>
    /// 可自定义的Excel格式模板 定义全局样式和标题样式内容
    /// </summary>
    public class ExcelFormat:StyleFormat
    {
        #region 枚举声明
        /// <summary>
        /// 对位于1,1的单元格的填充方式
        /// </summary>
        public enum FillModel { 空出第一个单元格, 列标题填充, 行标题填充, 没有标题 };
        /// <summary>
        /// 标题类型
        /// </summary>
        public enum TitleType { 列标题, 行标题 };
        #endregion

        #region 构造函数
        public ExcelFormat()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SavePath"></param>
        public ExcelFormat(String SavePath)
        {
            this.SavePath = SavePath;
        }
        #endregion

        #region Excel内容相关
        /// <summary>
        /// Sheet索引
        /// </summary>
        int _SheetIndex = 0;

        public int SheetIndex
        {
            get { return _SheetIndex; }
            set { _SheetIndex = value; }
        }
     
        /// <summary>
        /// 列标题
        /// </summary>
        /// 
        List<SCell> _Columns = new List<SCell>();

        [Obsolete("已过时 新版不再将标题单独区分")]
        public List<SCell> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }
        /// <summary>
        /// 行标题
        /// </summary>
        List<SCell> _Rows = new List<SCell>();
        [Obsolete("已过时 新版不再将标题单独区分")]
        public List<SCell> Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        /// <summary>
        /// 单元格List
        /// </summary>
        List<SCell> _SCells = new List<SCell>();

        public List<SCell> SCells
        {
            get { return _SCells; }
            set { _SCells = value; }
        }

        #endregion

        #region 文件路径和保存相关

        /// <summary>
        /// 相同路径文件存在时是否覆盖文件  默认覆盖
        /// </summary>
        bool _IsCover = true;

        public bool IsCover
        {
            get { return _IsCover; }
            set { _IsCover = value; }
        }

        /// <summary>
        /// 保存路径
        /// </summary>
        String _SavePath = "Temp.xlsx";

        public String SavePath
        {
            get { return _SavePath; }
            set { _SavePath = value; }
        }
        
        #endregion

        #region 内容插入方法
        /// <summary>
        /// 插入标题
        /// </summary>
        /// <param name="Inner_objlist">标题填充内容List</param>
        /// <param name="Style">标题单元格样式</param>
        /// <param name="TitleType">标题类型 1列标题 2行标题</param>
        /// <param name="FillModel">(0,0)单元格填充模式</param>
        public void InsertTitle(IList<object> Inner_objlist, CellStyle Style = null, TitleType TitleType = TitleType.列标题, FillModel FillModel = FillModel.空出第一个单元格)
        {
            if (FillModel == FillModel.没有标题)
            {
                return;
            }
            for (int i = 0; i < Inner_objlist.Count; i++)
            {
                SCell scell = new SCell
                {
                    CStyle = Style ?? new CellStyle(),
                    Txt_Obj = Inner_objlist[i]
                };
                if (TitleType == ExcelFormat.TitleType.列标题)
                {
                    if (FillModel == FillModel.列标题填充)
                    {
                        scell.Y = i;
                    }
                    else
                    {
                        scell.Y = i + 1;
                    }
                    this.SCells.Add(scell);
                }
                else if (TitleType == ExcelFormat.TitleType.行标题)
                {
                    if (FillModel == FillModel.行标题填充)
                    {
                        scell.X = i;
                    }
                    else
                    {
                        scell.X = i + 1;
                    }
                    this.SCells.Add(scell);
                }
            }
        }

        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="row">待插入的数据行</param>
        /// <param name="CStyle">数据行统一单元格样式</param>
        public void InsertCellRow(CellRow row, CellStyle CStyle = null)
        {
            for (int i = row.LeftIndex; i <= row.RightIndex; i++)
            {
                SCell scell = new SCell();
                scell.CStyle = CStyle ?? new CellStyle() ;
                scell.Y = i;
                scell.X = row.RowIndex;
                if (row.Is_Pic)
                {
                    scell.Image_Ms = (MemoryStream)(object)row.Obj_List[i - row.LeftIndex];
                }
                else
                {
                    scell.Txt_Obj = (object)row.Obj_List[i - row.LeftIndex];
                }
                this.SCells.Add(scell);
            }
        }
        /// <summary>
        /// 插入多行数据块
        /// </summary>
        /// <param name="rowlist">待插入数据行列表</param>
        /// <param name="CStyle">数据块统一单元格样式</param>
        public void InsetCellBlock(List<CellRow> rowlist, CellStyle CStyle = null)
        {
            rowlist.ForEach((item) =>
            {
                InsertCellRow(item, CStyle);
            });
        }

        /// <summary>
        /// 插入一列数据
        /// </summary>
        /// <param name="rowlist">待插入数据列列表</param>
        /// <param name="CStyle">数据列统一单元格样式</param>
        public void InsertCellColm(CellColm clom, CellStyle CStyle = null)
        {
            for (int i = clom.TopIndex; i <= clom.ButtomIndex; i++)
            {
                SCell scell = new SCell();
                scell.CStyle = CStyle ?? new CellStyle();
                scell.Y = clom.ColmIndex;
                scell.X = i;
                if (clom.Is_Pic)
                {
                    scell.Image_Ms = (MemoryStream)(object)clom.Obj_List[i - clom.TopIndex];
                }
                else
                {
                    scell.Txt_Obj = (object)clom.Obj_List[i - clom.TopIndex];
                }
                this.SCells.Add(scell);
            }
        }

        /// <summary>
        /// 插入多列数据块
        /// </summary>
        /// <param name="rowlist">待插入数据列列表</param>
        /// <param name="CStyle">数据块统一单元格样式</param>
        public void InsetCellBlock(List<CellColm> rowlist, CellStyle CStyle = null)
        {
            rowlist.ForEach((item) =>
            {
                InsertCellColm(item, CStyle);
            });
        }
        #endregion

    }
}
