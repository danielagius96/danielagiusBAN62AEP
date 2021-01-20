using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IProductsService
    {
        IQueryable<ProductViewModel> GetProducts();
        IQueryable<ProductViewModel> GetProducts(string keyword);

        ProductViewModel GetProduct(Guid id);

        void AddProduct(ProductViewModel model);


        void DeleteProduct(Guid id);

        void HideProduct(Guid id);
    }
}
