using CafeLibrary.DataAccess.Repository.IRepository;
using CafeLibrary.Models;
using CafeLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // Access to wwwroot folder
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productsList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();

            return View(productsList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(
                data => new SelectListItem
                {
                    Text = data.Name,
                    Value = data.Id.ToString(),
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //updating image by removing prebious one and adding new one
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //deleting the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        //if old image exists on the image path

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            //then delete the old image from the path
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //add new image to the path and save it

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (obj.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(
                    data => new SelectListItem
                    {
                        Text = data.Name,
                        Value = data.Id.ToString(),
                    }
                    );

                return View(obj);
            }
        }

        public IActionResult Delete(int? id) //GET delete data by ID
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) //deleting by ID
        {
            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id); //finding the data by ID first

            if (product is null)
            {
                return NotFound();
            }

            //else remove the data found by ID
            _unitOfWork.ProductRepository.Remove(product);

            //savechanges to the database
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";

            //return back to the index view
            return RedirectToAction("Index");
        }
    }
}
