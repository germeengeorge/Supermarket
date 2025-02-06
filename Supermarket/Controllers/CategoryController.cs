using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Controllers
{
    //[Area("Admin")]
    // [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoriesList = _unitOfWork.Categories.GetAll().ToList();
            return View("Index", objCategoriesList);
        }
        public IActionResult Index2(string search)
        {
            List<Category> objCategoriesList = _unitOfWork.Categories.GetAll().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                objCategoriesList = objCategoriesList.Where(x => x.Name.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            return View("ViewForCustomer", objCategoriesList);
        }

        public IActionResult Search(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                List<Category> objCategoriesList = _unitOfWork.Categories.GetAll().ToList();
                objCategoriesList = objCategoriesList.Where(x => x.Name.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View("ViewForCustomer", objCategoriesList);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Categories created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoriesFromDb = _unitOfWork.Categories.Get(u => u.id == id);

            return View(CategoriesFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Categories updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            Category? obj = _unitOfWork.Categories.Get(u => u.id == id);
            _unitOfWork.Categories.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public IActionResult Pagination(int page = 1, int pageSize = 4)
        {
            return View("Pagination", _unitOfWork.Categories.GetAll());
        }
    }
}