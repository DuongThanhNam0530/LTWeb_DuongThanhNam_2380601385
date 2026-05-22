using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Iphone 15", Price = 35000000, Description = "Iphone 15 pro max", CategoryId = 1 },
            new Product { Id = 2, Name = "Laptop Asus", Price = 25000000, Description = "Laptop gaming", CategoryId = 2 }
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Description = product.Description;
                existing.CategoryId = product.CategoryId;
                existing.ImageUrl = product.ImageUrl;
                existing.ImageUrls = product.ImageUrls;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null) _products.Remove(product);
        }
    }
}