namespace UglyToad.PdfPig.Util
{
    using System.Reflection;

    /// <summary>
    /// Exposes PdfPig version.
    /// </summary>
    public class PdfVersion
    {
        /// <summary>
        /// Returns the version of PdfPig.
        /// </summary>
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }
    }
}
