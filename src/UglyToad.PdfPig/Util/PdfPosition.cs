namespace UglyToad.PdfPig.Util
{
    using Core;

    /// <summary>
    /// Unit of measure
    /// </summary>
    public enum UnitOfMeasure
    {
        /// <summary>
        /// base unit (1/72 inch)
        /// </summary>
        Point = 0,

        /// <summary>
        /// 1 inch = 72 point
        /// </summary>
        Inch = 1,

        /// <summary>
        /// 1 centimeter = 72 point / 2.54 inch
        /// </summary>
        cm = 2,

        /// <summary>
        /// 1 millimeter = 72 point / 25.4 inch
        /// </summary>
        mm = 3
    }

    /// <summary>
    /// Unit converter
    /// </summary>
    public class PdfPosition
    {
        /// <summary>
        /// Page size in unit
        /// </summary>
        public PdfRectangle PageSize { get; }

        internal static double ScaleFactor = 1;

        internal readonly double[] unitInPoints = { 1, 72, 72 / 2.54, 72 / 25.4 };

        internal static bool CalculateFromTop = true;

        internal static PdfRectangle PageSizeInPoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfMeasure"></param>
        /// <param name="pageSize"></param>
        public PdfPosition(UnitOfMeasure unitOfMeasure, PdfRectangle pageSize) : this(unitOfMeasure, pageSize, false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfMeasure"></param>
        /// <param name="pageSize"></param>
        /// <param name="calculateFromTop"></param>
        public PdfPosition(UnitOfMeasure unitOfMeasure, PdfRectangle pageSize, bool calculateFromTop)
        {
            ScaleFactor = unitInPoints[(int)unitOfMeasure];
            PageSize = new PdfRectangle(0, 0, pageSize.Width / ScaleFactor, pageSize.Height / ScaleFactor);
            PageSizeInPoint = pageSize;
            CalculateFromTop = calculateFromTop;
        }

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint this[int x, int y] => ToPoint(x, y);

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint this[decimal x, decimal y] => ToPoint(x, y);

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint this[double x, double y] => ToPoint(x, y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Dimension this[decimal a] => new Dimension(a);

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint ToPoint(int x, int y)
        {
            if (CalculateFromTop)
            {
                return new PdfPoint(x * ScaleFactor, PageSizeInPoint.Height - y * ScaleFactor);
            }
            return new PdfPoint(x * ScaleFactor, y * ScaleFactor);
        }

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint ToPoint(decimal x, decimal y)
        {
            if (CalculateFromTop)
            {
                return new PdfPoint((double)x * ScaleFactor, PageSizeInPoint.Height - (double)y * ScaleFactor);
            }
            return new PdfPoint((double)x * ScaleFactor, (double)y * ScaleFactor);
        }

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint ToPoint(double x, double y)
        {
            if (CalculateFromTop)
            {
                return new PdfPoint(x * ScaleFactor, PageSizeInPoint.Height - y * ScaleFactor);
            }
            return new PdfPoint(x * ScaleFactor, y * ScaleFactor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        internal static decimal ToHeight(decimal height)
        {
            if (CalculateFromTop)
            {
                return -height * (decimal)ScaleFactor;
            }
            return height * (decimal)ScaleFactor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        internal static decimal ToWidth(decimal width)
        {
            return width * (decimal)ScaleFactor;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct Dimension
        {
            /// <summary>
            /// 
            /// </summary>
            public decimal Height;

            /// <summary>
            /// 
            /// </summary>
            public decimal Width;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public Dimension(decimal value)
            {
                Width = ToWidth(value);
                Height = ToHeight(value);
            }
        }
    }
}
