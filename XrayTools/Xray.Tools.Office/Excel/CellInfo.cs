#region 说明
//---------------------------------------名称:单元格相关类
//---------------------------------------版本:1.0.0.0
//---------------------------------------更新时间:2017/11/10
//---------------------------------------作者:献丑
//---------------------------------------CSDN:http://blog.csdn.net/qq_26712977
//---------------------------------------GitHub:https://github.com/a462247201/Tools 
#endregion

#region 名空间
using Aspose.Cells;
using System.Drawing;
using System.IO; 
#endregion

//单元格相关
namespace Xray.Tools.Office.Excel
{
    /// <summary>
    /// 单元格填充数据
    /// </summary>
    public class CellInner
    {
        /// <summary>
        /// 是否是图片数据
        /// </summary>
        bool _IsPic = false;

        public bool IsPic
        {
            get { return _IsPic; }
            set { _IsPic = value; }
        }
        /// <summary>
        /// 普通文本
        /// </summary>
        object _Txt_Obj = new object();

        public object Txt_Obj
        {
            get { return _Txt_Obj; }
            set { _Txt_Obj = value; }
        }
        /// <summary>
        /// 图片流
        /// </summary>
        MemoryStream _Image_Ms = new MemoryStream();

        public MemoryStream Image_Ms
        {
            get { return _Image_Ms; }
            set { _Image_Ms = value; }
        }
    }
    /// <summary>
    /// 单元格基类 支持坐标和内容
    /// </summary>
    public  class CellBase:CellInner
    {
        #region 构造函数
        public CellBase()
        {

        }

        /// <summary>
        /// 普通文本
        /// </summary>
        /// <param name="x">行号</param>
        /// <param name="y">列号</param>
        /// <param name="data">填充数据</param>
        public CellBase(int x, int y, object data)
        {
            this.X = x;
            this.Y = y;
            this.Txt_Obj = data;
            this.IsPic = false;
        }
        /// <summary>
        /// 图片流
        /// </summary>
        /// <param name="x">行号</param>
        /// <param name="y">列号</param>
        /// <param name="data">填充数据</param>
        public CellBase(int x, int y, MemoryStream data)
        {
            this.X = x;
            this.Y = y;
            this.Image_Ms = data;
            this.IsPic = true;
        }

        /// <summary>
        /// 图片流
        /// </summary>
        /// <param name="x">行号</param>
        /// <param name="y">列号</param>
        /// <param name="data">填充数据</param>
        public CellBase(int x, int y, Image data,System.Drawing.Imaging.ImageFormat format_type)
        {
            this.X = x;
            this.Y = y;
            data.Save(Image_Ms, format_type);
            this.IsPic = true;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 纵坐标
        /// </summary>
        int _Y = 0;

        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        /// <summary>
        /// 横坐标
        /// </summary>
        int _X = 0;

        public int X
        {
            get { return _X; }
            set { _X = value; }
        }


        #endregion
    }

    /// <summary>
    /// 可配置样式的单元格 继承与CellBase
    /// </summary>
    public class SCell : CellBase
    {
        #region 方法
        /// <summary>
        /// 通过当前设置 生成样式
        /// </summary>
        /// <returns></returns>
        public Style CreateStyle()
        {
            Style style = new Style();
            //边框颜色
            if (CStyle.BorderColor != Color.White)
            {
                style.Borders[BorderType.TopBorder].Color = CStyle.BorderColor;
                style.Borders[BorderType.BottomBorder].Color = CStyle.BorderColor;
                style.Borders[BorderType.LeftBorder].Color = CStyle.BorderColor;
                style.Borders[BorderType.RightBorder].Color = CStyle.BorderColor;
            }
            //边框线
            style.Borders[BorderType.TopBorder].LineStyle = CStyle.TopBorder;
            style.Borders[BorderType.BottomBorder].LineStyle = CStyle.BottomBorder;
            style.Borders[BorderType.LeftBorder].LineStyle = CStyle.LeftBorder;
            style.Borders[BorderType.RightBorder].LineStyle = CStyle.RightBorder;

            //单元格
            if (CStyle.ForegroundColor != Color.White)
            {
                style.ForegroundColor = CStyle.ForegroundColor;
                style.Pattern = BackgroundType.Solid;
            }

            //对齐
            style.HorizontalAlignment = CStyle.HorizontalAlignment;
            style.VerticalAlignment = CStyle.VerticalAlignment;

            //字体
            style.Font.IsBold = CStyle.IsBold;
            style.Font.IsItalic = CStyle.IsItalic;
            style.Font.Size = CStyle.FontSize;
            style.Font.Color = CStyle.FontColor;

            //换行
            style.IsTextWrapped = CStyle.IsTextWrapped;
            return style;
        }
        #endregion

        /// <summary>
        /// 单元格样式
        /// </summary>
        CellStyle _CStyle = new CellStyle();

        public CellStyle CStyle
        {
            get { return _CStyle; }
            set { _CStyle = value; }
        }

    }
}
