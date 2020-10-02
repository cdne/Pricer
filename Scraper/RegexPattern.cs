namespace Scraper
{
    public class RegexPattern
    {
        /// <summary>
        /// Pattern to find all categories pages.
        /// </summary>
        /// <returns>category pattern</returns>
        public static string CategoryPattern(string category)
        {
            return category + @"p\/\d*\.?\d+\/$";
        }
    }
}