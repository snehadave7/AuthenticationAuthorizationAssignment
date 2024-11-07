using AuthenticationDemo.Authentication;
using AuthenticationDemo.Models;

namespace AuthenticationDemo.Repository {

    public class ProductService : IProductService {

        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context) {
            _context = context;
        }

        public int AddNewProducts(Products Products) {
            if (Products != null) {
                _context.Products.Add(Products);
                _context.SaveChanges();
                return Products.Id;
            }
            return 0;
        }

        public string DeleteProducts(int id) {
            if (id != 0 || id != null) {
                var Products = _context.Products.FirstOrDefault(x => x.Id == id);
                if (Products != null) {
                    _context.Products.Remove(Products);
                    _context.SaveChanges();
                    return "Products with id: " + id + " is deleted";
                }
                return "Products does not exist";
            }
            return "Id cannot be 0 or null";
        }

        public List<Products> GetAllProductss() {
            var Products = _context.Products.ToList();
            if (Products.Count > 0) return Products;
            else return null;
        }

        public Products GetProductsById(int id) {
            if (id != 0 || id != null) {
                var Products = _context.Products.FirstOrDefault(x => x.Id == id);
                if (Products != null) {
                    return Products;
                }
                else return null;
            }
            return null;
        }

        public string UpdateProducts(Products Products) {
            var existingProducts = _context.Products.FirstOrDefault(x => x.Id == Products.Id);
            if (existingProducts != null) {
                existingProducts.Name = Products.Name;
                existingProducts.Price = Products.Price;
                existingProducts.Description = Products.Description;
                existingProducts.SellerName = Products.SellerName;
                existingProducts.Stock = Products.Stock;
                _context.Entry(existingProducts).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return "Products is updated";
            }
            return "Products not updated";
        }
    }
}

