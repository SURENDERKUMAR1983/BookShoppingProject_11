using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.Model
{
   public class Product
    {
        public int Id { get; set; }
        [Required] 
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,1000)]
        public Double ListPrice { get; set; }
        [Required]
        [Range(1,1000)]
        public Double Price50 { get; set; }
        [Required]
        [Range(1,1000)]
        public Double Price100 { get; set; } 
        [Required]
        [Range(1,1000)]
        
        public Double Price { get; set; }
        public string ImageUrl { get; set; }
        
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        
        public int CoverTypeId { get; set; }
        [Display(Name = "CoverType")]
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType  { get; set; }
    }
}
