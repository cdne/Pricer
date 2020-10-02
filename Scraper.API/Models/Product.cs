using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public string PhotoLink { get; set; }
        [Required]
        public string ProductLink { get; set; }
        [Required]
        [NotNull]
        public string OldPrice { get; set; }
        [Required]
        [NotNull]
        public string NewPrice { get; set; }

    
    }
}
