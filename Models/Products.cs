namespace AuthenticationDemo.Models {
    public class Products {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; } 
        public string SellerName { get; set; }
    }
    public record ProductDTO (int Id, string Name, string Description, decimal Price);
}
