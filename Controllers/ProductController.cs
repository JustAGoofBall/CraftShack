using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftShack.Models;
using CraftShack.Data;

namespace CraftShack.Controllers
{
    public class ProductController : Controller
    {
        private readonly CraftShackDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ProductController> _logger;

        public ProductController(CraftShackDbContext context, IWebHostEnvironment environment, ILogger<ProductController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.ExistingImages = GetExistingImages();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Name,Price,Description,ImagePath,ImageUpload")] Product product)
        {
            try
            {
                _logger.LogInformation("Create POST called");
                
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("ModelState is valid");
                    
                    // Handle image upload
                    if (product.ImageUpload != null)
                    {
                        _logger.LogInformation($"Image upload detected: {product.ImageUpload.FileName}");
                        product.ImagePath = await SaveImageAsync(product.ImageUpload);
                        _logger.LogInformation($"Image saved to: {product.ImagePath}");
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Product saved successfully");
                    return RedirectToAction(nameof(Index));
                }
                
                _logger.LogWarning("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
                }
                
                // If we get here, ModelState had errors — return the view to show validation.
                ViewBag.ExistingImages = GetExistingImages();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                throw;
            }
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            
            ViewBag.ExistingImages = GetExistingImages();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImagePath,ImageUpload")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing product from database
                    var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    
                    // Handle image upload
                    if (product.ImageUpload != null)
                    {
                        product.ImagePath = await SaveImageAsync(product.ImageUpload);
                    }
                    else if (existingProduct != null)
                    {
                        // Preserve existing ImagePath if no new image uploaded
                        product.ImagePath = existingProduct.ImagePath;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ExistingImages = GetExistingImages();
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null) _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                _logger.LogInformation($"Uploads folder: {uploadsFolder}");
                
                // Create uploads directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created uploads directory");
                }

                // Generate unique filename
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                _logger.LogInformation($"Saving to: {filePath}");

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                _logger.LogInformation("File saved successfully");
                return "/uploads/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image");
                throw;
            }
        }

        private List<string> GetExistingImages()
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                return new List<string>();
            }

            var imageFiles = Directory.GetFiles(uploadsFolder)
                .Select(f => "/uploads/" + Path.GetFileName(f))
                .ToList();

            return imageFiles;
        }
    }
}
