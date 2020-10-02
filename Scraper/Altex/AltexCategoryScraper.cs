using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Scraper.Altex
{
    public class AltexCategoryScraper
    {
        private readonly Dictionary<string, bool> _incorrectCategoryLinksDictionary;
        private readonly List<string> _correctcategoryLinksList;
        private static readonly List<string> categoryInStockLinks =new List<string>();

        public AltexCategoryScraper()
        {
            _incorrectCategoryLinksDictionary = new Dictionary<string, bool>();
            _correctcategoryLinksList = new List<string>();
        }
        
        /// <summary>
        /// Connect to site map and get all categories links
        /// </summary>
        /// <param name="siteMap"></param>
        /// <returns></returns>
        public  IEnumerable<string> GetAllCategories(string siteMap)
        {

            var websiteLink = StoreConnection.ConnectToStoreAddress(siteMap);
            var allHrefNodes = GetAllHrefNodesFromLink(websiteLink);
            
            AddNonCategoryLinksToDictionary(_incorrectCategoryLinksDictionary);

            foreach (var node in allHrefNodes)
            {
                var siteLink = node.GetAttributeValue("href", string.Empty);
                if (!_incorrectCategoryLinksDictionary.ContainsKey(siteLink) && siteLink.Contains("http"))
                {
                    _correctcategoryLinksList.Add(siteLink);
                }
            }
            return GetDistinctCategories(_correctcategoryLinksList);
        }


        /// <summary>
        /// Get all href nodes from a link
        /// </summary>
        /// <param name="websiteLink">site link</param>
        /// <returns>all href nodes</returns>
        private static HtmlNodeCollection GetAllHrefNodesFromLink(HtmlDocument websiteLink)
        {
            var allHrefNodes = websiteLink.DocumentNode.SelectNodes("//a['@href']");
            return allHrefNodes;
        }
        

        /// <summary>
        /// Add non category links in a dictionary
        /// </summary>
        /// <param name="badLinks">non category links</param>
        private static void AddNonCategoryLinksToDictionary(Dictionary<string, bool> badLinks)
        {
            string[] badLinksArray =
            {
                "https://altex.ro/promo/", "https://altex.ro/branduri/",
                "https://altex.ro/solutii-finantare/credit-online/",
                "https://altex.ro/solutii-finantare/credit-consum-traditional/",
                "https://altex.ro/solutii-finantare/sisteme-rate-valabile-pentru-posesorii-cardurilor-credit/pag/",
                "https://altex.ro/suport-clienti/", "https://altex.ro/magazine/",
                "https://altex.ro/donatie-impreuna/", "https://altex.ro",
                "https://altex.ro/best-buy-it-electrocasnice-numarul-unu-romania-2019/pag/",
                "https://altex.ro/best-buy-it-electrocasnice-numarul-unu-romania-2019/pag/",
                "https://altex.ro/despre-noi/pag/", "https://altex.ro/cariere/", "https://altex.ro/branduri/",
                "https://altex.ro/harta-site/", "http://afiliere.altex.ro/", "https://altex.ro/ecotic/pag/",
                "https://altex.ro/vanzari-corporate/pag/", "https://altex.ro/tax-free/pag/",
                "https://altex.ro/suport-clienti/", "https://altex.ro/contact/", "https://altex.ro/cont/retururi/",
                "https://altex.ro/magazine/", "https://altex.ro/termeni-conditii/pag/",
                "https://altex.ro/politica-utilizare-cookie/pag/", "https://altex.ro/informatii-privind-deee/pag/",
                "http://www.anpc.gov.ro/", "https://altex.ro/protectia-datelor-caracter-personal/pag/",
                "https://ec.europa.eu/consumers/odr", "https://altex.ro/newsletter/abonare/?referrer=footer",
                "https://www.facebook.com/AltexRomania/", "https://twitter.com/altexro",
                "https://ro.linkedin.com/company/altex-romania", "https://www.youtube.com/user/AltexRomania",
                "https://altex.ro/cont/", "https://altex.ro/cos-cumparaturi/"
            };

            foreach (var badLink in badLinksArray)
            {
                if (badLinks.ContainsKey(badLink)) continue;
                badLinks.Add(badLink, true);
            }
        }

        /// <summary>
        /// Find and return unique values from a IEnumerable type
        /// </summary>
        /// <param name="cateogryLinks"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetDistinctCategories(IEnumerable<string> cateogryLinks)
        {
            return cateogryLinks.Distinct();
        }
        
        /// <summary>
        /// Get nodes
        /// </summary>
        /// <param name="categoryAddress"></param>
        /// <returns></returns>
        public static HtmlNodeCollection StoreCategoryHtmlNodeCollection(
            string categoryAddress)
        {
            var connectToStoreCategory = StoreConnection.ConnectToStoreAddress(categoryAddress);
            var storeCategoryHtmlNodeCollection =
                connectToStoreCategory.DocumentNode.SelectNodes($"//select[@class='js-trigger-catalog-toolbar-apply-filters']/option");
            return storeCategoryHtmlNodeCollection;
        }



        
        //todo write to file in stock categories
        // return a list with data
        public static IEnumerable<string> GetInStockCategoryLinks(IEnumerable<string> categoryLinks)
        {
            try
            {
                foreach (var link in categoryLinks)
                {
                    var navigateToPage = StoreConnection.ConnectToStoreAddress(link);
                    var allHrefsOnPage = navigateToPage.DocumentNode.SelectNodes("//a[@href]");
                    foreach (var href in allHrefsOnPage)
                    {
                        var hrefLink = href.Attributes["href"].Value;
                        if (hrefLink.Contains("in-stoc") && !string.IsNullOrWhiteSpace(hrefLink))
                        {
                            Console.WriteLine(hrefLink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetInStockCategoryLinks: " + ex.Message);
            }

            return categoryInStockLinks;
        }

        /// <summary>
        /// Get all category pages
        /// </summary>
        /// <param name="categoryLink"></param>
        /// <param name="storeCategoryHtmlNodeCollection"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetCategoryPages(string categoryLink,
            HtmlNodeCollection storeCategoryHtmlNodeCollection)
        {
            var categoryPages = from result in storeCategoryHtmlNodeCollection
                select result.GetAttributeValue("value")
                into valueFromResult
                select Regex.Match(valueFromResult, categoryLink + @"p\/\d*\.?\d+\/$")
                into match
                where !string.IsNullOrWhiteSpace(match.Value)
                select match.Value.Trim();
            return categoryPages;
        }
    }
}