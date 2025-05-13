using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.Models
{
    public class Category
    {
        [Key]
        public int id {  get; set; }
        [DisplayName("Category Name")]
        [Required]
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be 1 or greater.")]
        public int DisplayOrder {  get; set; }
        public string? logoUrl {  get; set; }

        public List<Product> products = new List<Product>();
    }
}
