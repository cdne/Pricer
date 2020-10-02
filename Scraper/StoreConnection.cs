using HtmlAgilityPack;

namespace Scraper
{
    public static class StoreConnection
    {
        /// <summary>
        /// Connect to website and load page
        /// </summary>
        /// <param name="pageLink">Link to connect to page</param>
        /// <returns>Html document</returns>
        public static HtmlDocument ConnectToStoreAddress(string pageLink)
        {
            return new HtmlWeb().Load(pageLink);
        }
    }
}