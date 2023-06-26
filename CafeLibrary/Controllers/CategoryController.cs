using CafeLibrary.Data;
using CafeLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CafeLibrary.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoriesList = _context.Categories.ToList();

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
                _context.Categories.Add(category);
                _context.SaveChanges();
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

            Category? category = _context.Categories.Find(id);

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
                _context.Categories.Update(category);
                _context.SaveChanges();
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

            Category? category = _context.Categories.Find(id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int?id) //deleting by ID
        {
            Category? category = _context.Categories.Find(id); //finding the data by ID first

            if (category is null)
            {
                return NotFound();
            }

            //else remove the data found by ID
            _context.Categories.Remove(category);

            //savechanges to the database
            _context.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";

            //return back to the index view
            return RedirectToAction("Index");
        }

    }
}
