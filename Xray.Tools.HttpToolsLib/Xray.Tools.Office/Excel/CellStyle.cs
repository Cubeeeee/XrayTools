#region 说明
//---------------------------------------名称:单元格样式
//---------------------------------------依赖:Aspose.Cells.dll
//---------------------------------------版本:1.0.0.0
//---------------------------------------更新时间:2017/11/13
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using Aspose.Cells;
using System.Drawing; 
#endregion

namespace Xray.Tools.Office.Excel
{
    /// <summary>
    /// 单元格样式
    /// </summary>
    public class CellStyle
    {
        #region 单元格边框线

        /// <summary>
        /// 四周边框线
        /// </summary>
        CellBorderType _AllBorder = CellBorderType.None;

        public CellBorderType AllBorder
        {
            get { return _AllBorder; }
            set
            {
                this.TopBorder = value;
                this.BottomBorder = value;
                this.LeftBorder = value;
                this.RightBorder = value;
            }
        }

        /// <summary>
        /// 单元格上边框线
        /// </summary>
        CellBorderType _TopBorder = CellBorderType.None;

        public CellBorderType TopBorder
        {
            get { return _TopBorder; }
            set { _TopBorder = value; }
        }

        /// <summary>
        /// 单元格下边框线
        /// </summary>
        CellBorderType _BottomBorder = CellBorderType.None;

        public CellBorderType BottomBorder
        {
            get { return _BottomBorder; }
            set { _BottomBorder = value; }
        }
        /// <summary>
        /// 单元格左边框线
        /// </summary>
        CellBorderType _LeftBorder = CellBorderType.None;

        public CellBorderType LeftBorder
        {
            get { return _LeftBorder; }
            set { _LeftBorder = value; }
        }
        /// <summary>
        /// 单元格右边框线
        /// </summary>
        CellBorderType _RightBorder = CellBorderType.None;

        public CellBorderType RightBorder
        {
            get { return _RightBorder; }
            set { _RightBorder = value; }
        }

        #endregion

        #region 颜色

        /// <summary>
        /// 背景色
        /// </summary>
        Color _ForegroundColor = Color.White;

        public Color ForegroundColor
        {
            get { return _ForegroundColor; }
            set { _ForegroundColor = value; }
        }
        /// <summary>
        /// 边框线
        /// </summary>
        Color _BorderColor = Color.White;

        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        Color _FontColor = Color.Black;

        public Color FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; }
        }

        #endregion

        #region 字体

        /// <summary>
        /// 是否斜体
        /// </summary>
        bool _IsItalic = false;

        public bool IsItalic
        {
            get { return _IsItalic; }
            set { _IsItalic = value; }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        int _FontSize = 10;

        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }

        /// <summary>
        /// 加粗
        /// </summary>
        bool _IsBold = false;

        public bool IsBold
        {
            get { return _IsBold; }
            set { _IsBold = value; }
        }

        #endregion

        #region 对齐
       /// <summary>
        /// 文字水平对齐方式 默认居中
        /// </summary>
        TextAlignmentType _HorizontalAlignment = TextAlignmentType.Center;

        public TextAlignmentType HorizontalAlignment
        {
            get { return _HorizontalAlignment; }
            set { _HorizontalAlignment = value; }
        }

        /// <summary>
        /// 文字竖直对齐方式 默认居中
        /// </summary>
        TextAlignmentType _VerticalAlignment = TextAlignmentType.Center;

        public TextAlignmentType VerticalAlignment
        {
            get { return _VerticalAlignment; }
            set { _VerticalAlignment = value; }
        }
        #endregion

        #region 换行
        /// <summary>
        /// 是否自动换行
        /// </summary>
        bool _IsTextWrapped = false;

        public bool IsTextWrapped
        {
            get { return _IsTextWrapped; }
            set { _IsTextWrapped = value; }
        }
        #endregion
    }
}
