using BookShoppingProject.DataAccess.Data;
using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    { private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var prodectInDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if(prodectInDb!=null)
            {
               if(prodectInDb.ImageUrl!="")
                {
                    prodectInDb.ImageUrl = product.ImageUrl;
                    prodectInDb.Title = product.Title;
                    prodectInDb.Description = product.Description;
                    prodectInDb.ISBN = product.ISBN;
                    prodectInDb.Author = product.Author;
                    prodectInDb.ListPrice = product.ListPrice;
                    prodectInDb.Price50 = product.Price50;
                    prodectInDb.Price100 = product.Price100;
                    prodectInDb.Price = product.Price;
                    prodectInDb.CategoryId = product.CategoryId;
                    prodectInDb.CoverTypeId = product.CoverTypeId;
                }
            }
        }
    }
}
