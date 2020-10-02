using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Scraper.Altex
{
    public class AltexProductsScraper
    {
        private readonly List<Product> _productsInCategory;

        public AltexProductsScraper()
        {
            _productsInCategory = new List<Product>();
        }

        /// <summary>
        /// Scrape products in category
        /// </summary>
        /// <param name="categoryPages"></param>
        private void ScrapeAllProductsInCategoryPage(IEnumerable<string> categoryPages)
        {
            var id = 0;
            foreach (var categoryPage in categoryPages)
            {
                var page = StoreConnection.ConnectToStoreAddress(categoryPage);
                var allNodes = page.DocumentNode.SelectNodes("//div[@class='Product']");
                foreach (var node in allNodes)
                {
                    var prices = FindPrices(node);
                    AddProducts(id, node, prices);
                    id++;
                }
            }
        }

        /// <summary>
        /// Find altex product price
        /// </summary>
        /// <param name="node">html node</param>
        /// <returns>array with new price and old price</returns>
        private static string[] FindPrices(HtmlNode node)
        {
            var prices = node.FirstChild.NextSibling.FirstChild.InnerText.ToLower().Replace("lei", " ")
                .Replace("in stoc", "").Split(" ");
            return prices;
        }
        
        /// <summary>
        /// Add altex products to list
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="node">Html node</param>
        /// <param name="prices">Product new prince and old price</param>
        /// <returns></returns>
        private void AddProducts(int id, HtmlNode node, string[] prices)
        {
            _productsInCategory.Add(
                new Product()
                {
                    Id = id,
                    Name = node.FirstChild.FirstChild.GetAttributeValue("title"),
                    PhotoLink = node.FirstChild.FirstChild.FirstChild.GetAttributeValue("src"),
                    ProductLink = node.FirstChild.FirstChild.GetAttributeValue("href"),
                    NewPrice = prices[0],
                    OldPrice = prices[1].Length == 0 ? "0" : prices[1]
                });
        }

       
        /// <summary>
        /// Return all products from list
        /// </summary>
        /// <param name="categoryPages">list of category pages</param>
        /// <returns>list of prodcut objects</returns>
        internal IEnumerable<Product> GetProducts(IEnumerable<string> categoryPages)
        {
            try
            {
                ScrapeAllProductsInCategoryPage(categoryPages);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _productsInCategory;
        }


        
        
        
    }
}