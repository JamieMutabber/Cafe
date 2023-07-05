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
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productsList = _unitOfWork.ProductRepository.GetAll().ToList();

            return View(productsList);
        }

        public IActionResult Create()
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

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(obj.Product);
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
        public IActionResult Edit(int? id) // fing category data by ID
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
        [HttpPost]
        public IActionResult Edit(Product product) //EDIT category by Categories model
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product Edited Successfully";
                return RedirectToAction("Index");
            }

            return View();
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
