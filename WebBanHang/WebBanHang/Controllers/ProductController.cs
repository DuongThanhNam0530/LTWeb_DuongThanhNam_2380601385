using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            // Truyền danh sách category để hiển thị tên thay vì ID
            ViewBag.Categories = _categoryRepository.GetAll().ToDictionary(c => c.Id, c => c.Name);
            return View(products);
        }

        // Hiển thị chi tiết
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            // Lấy tên category
            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Không xác định";
            return View(product);
        }

        // GET: Add
        public IActionResult Add()
        {
            // Dùng SelectList để dropdown hoạt động đúng
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Add (có upload ảnh)
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls != null && imageUrls.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(product);
        }

        // GET: Update
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Update
        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            var category = _categoryRepository.GetById(product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Không xác định";
            return View(product);
        }

        // POST: Delete confirmed
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        // Hàm lưu ảnh vào wwwroot/images
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            var fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(savePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }
    }
}
