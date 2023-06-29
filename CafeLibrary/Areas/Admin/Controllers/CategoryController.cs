using CafeLibrary.DataAccess.Repository.IRepository;
using CafeLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoriesList = _unitOfWork.CategoryRepository.GetAll().ToList();

            return View(categoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Display Order cannot be the same as Name!");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult Edit(int? id) // fing category data by ID
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category) //EDIT category by Categories model
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Edited Successfully";
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

            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) //deleting by ID
        {
            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id); //finding the data by ID first

            if (category is null)
            {
                return NotFound();
            }

            //else remove the data found by ID
            _unitOfWork.CategoryRepository.Remove(category);

            //savechanges to the database
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";

            //return back to the index view
            return RedirectToAction("Index");
        }
    }
}
