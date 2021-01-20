using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        ShoppingCartDbContext _context;
        public ProductsRepository(ShoppingCartDbContext context )
        {

            _context =   context;
        }

        public Guid AddProduct(Product p)
        {
          // p.Category = null; //because the runtime thinks that it needs to add a new category
           
            _context.Products.Add(p);
            _context.SaveChanges();

            return p.Id;
        }

        public void DeleteProduct(Guid id)
        { 
            Product p = GetProduct(id);
            _context.Products.Remove(p);
            _context.SaveChanges();
        }


        //Here we are getting the Id of the product and Hide in the database is going to be set to a value of
        // 1 instead of 0 (ie true). Then, using _context, we are saving the changes in the database. A migration had
        // to be carried out to update the database with the column hide in the Products Table.
        public void HideProduct(Guid id)
        {
            var p = GetProduct(id);
            p.hide = true;
            _context.SaveChanges();
        }

        public Product GetProduct(Guid id)
        { 
            return _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Product> GetProducts()
        { 
            return _context.Products.Include(x=>x.Category).Where(x=>x.hide==false);//"where" is getting all those products which have Hide set as false in the database.
        }
    }
}
