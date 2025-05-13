using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermarket.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Range(1,100,ErrorMessage ="the quantity must between 1 and 100")]
        public int quantity {  get; set; }

        public int productId {  get; set; }
        [ForeignKey("productId")]
        [ValidateNever]
        public Product Product { get; set; }

        [NotMapped]
        public double price {  get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")] 
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
