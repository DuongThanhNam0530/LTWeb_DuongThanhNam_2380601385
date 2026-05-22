using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string? Name { get; set; }

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        // Cho phần upload ảnh (mục 2.3.4)
        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}