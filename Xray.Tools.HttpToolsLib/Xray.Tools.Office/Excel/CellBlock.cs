#region 说明
//---------------------------------------名称:单元格组成的区块数据
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
using System.Linq;
using System.Text; 
#endregion

//单元格组成的区块
namespace Xray.Tools.Office.Excel
{
    /// <summary>
    /// 单元格区块填充
    /// </summary>
    public class BlockInner
    {
        /// <summary>
        /// 单元格填充数据  文本
        /// </summary>
        IList<object> _Obj_List = new List<object>();

        public IList<object> Obj_List
        {
            get { return _Obj_List; }
            set { _Obj_List = value; }
        }
        /// <summary>
        /// 单元格填充数据  图片
        /// </summary>
        IList<MemoryStream> _PicList = new List<MemoryStream>();

        public IList<MemoryStream> Pic_List
        {
            get { return _PicList; }
            set { _PicList = value; }
        }
        /// <summary>
        /// 是否是图片
        /// </summary>
        bool _Is_Pic = false;

        public bool Is_Pic
        {
            get { return _Is_Pic; }
            set { _Is_Pic = value; }
        }
    }

    /// <summary>
    /// Excel数据行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CellRow : BlockInner
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="left">起始列 从0开始</param>
        /// <param name="rowindex">行号 从0开始</param>
        /// <param name="objlist">插入单元格的数据列表 object类型</param>
        public CellRow(int left,int rowindex , IList<object> objlist )
        {
            this.LeftIndex = left;
            this.RightIndex = left+objlist.Count()-1;
            this.RowIndex = rowindex;
            this.Obj_List = objlist;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="left">起始列 从0开始</param>
        /// <param name="rowindex">行号 从0开始</param>
        /// <param name="objlist">插入单元格的数据列表 MemoryStream图片数据流</param>
        public CellRow(int left, int rowindex, IList<MemoryStream> Pic_List)
        {
            this.LeftIndex = left;
            this.RightIndex = left + Pic_List.Count() - 1;
            this.RowIndex = rowindex;
            this.Pic_List = Pic_List;
            Is_Pic = true;
        }


        /// <summary>
        /// 最左边单元格索引
        /// </summary>
        int _LeftIndex = 0;

        public int LeftIndex
        {
            get { return _LeftIndex; }
            set { _LeftIndex = value; }
        }
        /// <summary>
        /// 最右边单元格索引
        /// </summary>
        int _RightIndex = 0;

        public int RightIndex
        {
            get { return _RightIndex; }
            set { _RightIndex = value; }
        }

        /// <summary>
        /// 行号
        /// </summary>
        int _RowIndex = 0;

        public int RowIndex
        {
            get { return _RowIndex; }
            set { _RowIndex = value; }
        }

    }

    /// <summary>
    /// Excel数据列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CellColm : BlockInner
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="top">数据列起始行 从0开始</param>
        /// <param name="clomindex">列号 从0开始</param>
        /// <param name="objlist">插入单元格的数据列表 object类型</param>
        public CellColm(int top, int clomindex, IList<object> objlist)
        {
            this.TopIndex = top;
            this.ButtomIndex = top + objlist.Count() - 1;
            this.ColmIndex = clomindex;
            this.Obj_List = objlist;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="top">数据列起始行 从0开始</param>
        /// <param name="clomindex">列号 从0开始</param>
        /// <param name="objlist">插入单元格的数据列表 MemoryStream图片数据流</param>
        public CellColm(int top, int clomindex, IList<MemoryStream> Pic_List)
        {
            this.TopIndex = top;
            this.ButtomIndex = top + Pic_List.Count() - 1;
            this.ColmIndex = clomindex;
            this.Pic_List = Pic_List;
            Is_Pic = true;
        }

        /// <summary>
        /// 最左边单元格索引
        /// </summary>
        int _TopIndex = 0;

        public int TopIndex 
        {
            get { return _TopIndex; }
            set { _TopIndex = value; }
        }
        /// <summary>
        /// 最右边单元格索引
        /// </summary>
        int _ButtomIndex = 0;

        public int ButtomIndex
        {
            get { return _ButtomIndex; }
            set { _ButtomIndex = value; }
        }

        /// <summary>
        /// 列号
        /// </summary>
        int _ColmIndex = 0;

        public int ColmIndex
        {
            get { return _ColmIndex; }
            set { _ColmIndex = value; }
        }
    }

}
