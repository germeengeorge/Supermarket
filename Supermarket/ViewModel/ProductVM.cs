using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Supermarket.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }

        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(10, 1000, ErrorMessage = "Price must between 10 and 1000")]
        public int price { get; set; }

        //  public int? Discount {  get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [DisplayName("Product image")]
        public IFormFile ProductImage { get; set; }

        public int Quantity;

    }
}
