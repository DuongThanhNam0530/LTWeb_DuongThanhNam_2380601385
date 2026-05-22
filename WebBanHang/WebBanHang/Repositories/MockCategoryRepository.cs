using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private static List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop" },
            new Category { Id = 3, Name = "Phụ kiện" }
        };

        public IEnumerable<Category> GetAll() => _categories;

        public Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);
    }
}
