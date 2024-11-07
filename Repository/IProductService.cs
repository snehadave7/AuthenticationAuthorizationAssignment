using AuthenticationDemo.Models;

namespace AuthenticationDemo.Repository {
    public interface IProductService {
        List<Products> GetAllProductss();
        Products GetProductsById(int id);
        int AddNewProducts(Products Products);
        string UpdateProducts(Products Products);
        string DeleteProducts(int id);
    }
}
