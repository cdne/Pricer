
using System;

namespace Scraper.Altex

{
    public class AltexScraper
    {
        public void ReturnTest()
        {
            var connection = StoreConnection.ConnectToStoreAddress("https://www.thisiswhyimbroke.com/");
            Console.WriteLine(connection.ParsedText);
        }
    }
}