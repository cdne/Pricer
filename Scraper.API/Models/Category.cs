using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scraper.API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public string Name { get; set; }

    }
}
