using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class ProductViewModel
    {
        //Here we are including all reuired so that fields will have full validation
     
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Please input name of product")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please input a Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please input the price of the product")]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please input a category")]
        public CategoryViewModel Category { get; set; }

        [Required(ErrorMessage = "Please input an image")]
        public string ImageUrl { get; set; }

        public int Stock { get; set; }
        //public List<CategoryViewModel> Categories { get; set; }


    }
}
