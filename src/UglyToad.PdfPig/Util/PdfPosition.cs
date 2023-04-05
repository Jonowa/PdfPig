namespace UglyToad.PdfPig.Util
{
    using System;
    using System.Drawing;
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
        public Size PageSize { get; }

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
            PageSize = new Size(
                (int)Math.Round(pageSize.Width / ScaleFactor, 0),
                (int)Math.Round(pageSize.Height / ScaleFactor, 0)
            );
            //PageSize = new PdfRectangle(0, 0, pageSize.Width / ScaleFactor, pageSize.Height / ScaleFactor);
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
        public PdfPoint this[decimal x, decimal y] => ToPoint((double)x, (double)y);

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint this[double x, double y] => ToPoint(x, y);

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PdfPoint this[float x, float y] => ToPoint(x, y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SizeF this[int a] => new SizeF(ToWidth(a), ToHeight(a));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SizeF this[decimal a] => new SizeF(ToWidth((float)a), ToHeight((float)a));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SizeF this[double a] => new SizeF(ToWidth((float)a), ToHeight((float)a));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public SizeF this[float a] => new SizeF(ToWidth(a), ToHeight(a));

        /// <summary>
        /// Create a new <see cref="PdfPoint"/> at this position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private PdfPoint ToPoint(double x, double y)
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
        private static float ToHeight(float height)
        {
            if (CalculateFromTop)
            {
                return -height * (float)ScaleFactor;
            }
            return height * (float)ScaleFactor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        private static float ToWidth(float width)
        {
            return width * (float)ScaleFactor;
        }
    }
}
