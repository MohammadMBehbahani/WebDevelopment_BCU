using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class Product:ModelBase
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        [StringLength(64, ErrorMessage = "Name is too long.")]
        [MinLength(2, ErrorMessage = "Enter a complete Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public string Description { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<UserOrderDetails> UserOrderDetails { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItem { get; set; }
    }
}
