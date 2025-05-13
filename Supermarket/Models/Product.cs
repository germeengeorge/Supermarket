using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Supermarket.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string imgURL {  get; set; }

        [Range(10,1000,ErrorMessage ="Price must between 10 and 1000")]
        public int price {  get; set; }
        public int? Discount {  get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        public bool IsFavorited { get; set; } 

    }
}
