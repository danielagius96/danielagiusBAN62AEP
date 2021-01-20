using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;
        private IMapper _autoMapper;
        public ProductsService(IProductsRepository productsRepo, IMapper autoMapper)
        {
            _productsRepo = productsRepo;
            _autoMapper = autoMapper;
        }

        public void AddProduct(ProductViewModel model)
        {
            //ProductViewModel >>>>>> Product

            /* Product p = new Product()
             {
                 Name = model.Name,
                 Description = model.Description,
                 ImageUrl = model.ImageUrl,
                 Price = model.Price,
                 Stock = model.Stock,
                 CategoryId = model.Category.Id
             };

             _productsRepo.AddProduct(p);
            */

            _productsRepo.AddProduct(_autoMapper.Map<Product>(model));
        }

        public void DeleteProduct(Guid id)
        {
            _productsRepo.DeleteProduct(id);
        }

        //Retrieve the Hide Id of the product and updating the product repository accordingly.
        public void HideProduct(Guid id)
        {
            _productsRepo.HideProduct(id);
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var p = _productsRepo.GetProduct(id);
            if (p == null) return null;
            else
            {
                /*  return new ProductViewModel()
                    {
                        Id = p.Id,
                        Description = p.Description,
                        ImageUrl = p.ImageUrl,
                        Name = p.Name,
                        Price = p.Price
                        ,
                        Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name }

                    };
             */

                var result = _autoMapper.Map<ProductViewModel>(p);
                return result;
            }

          
           // var p = GetProducts().SingleOrDefault(x => x.Id == id);
         //   return p;
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            //this whole method will use linq to convert from Iqueryable<Product> to Iqueryable<ProductViewModel>
            /* var list = from p in _productsRepo.GetProducts()//.Include(x=>x.Category)  
                        select new ProductViewModel()
                        {
                            Id = p.Id,
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.Price,
                            Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name }
                        };

             return list;*/

            //IQueryable<Product> >>>>>> IQueryable<ProductViewModel>
            return _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_autoMapper.ConfigurationProvider);

        }


        public IQueryable<ProductViewModel> GetProducts(string keyword)
        {
            
            return _productsRepo.GetProducts().Where(p => p.Description.Contains(keyword)  
                                                     || p.Name.Contains(keyword))          
                .ProjectTo<ProductViewModel>(_autoMapper.ConfigurationProvider);

        }


    }
}
