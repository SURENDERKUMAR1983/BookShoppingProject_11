using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Areas.Admin.Controllers
{[Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; 
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert (int? id) 
        {
            Category category = new Category();            
            //for Create
            if (id == null)
            return View(category);
            //for edit
            var CategoryInDb = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (CategoryInDb == null)
                return NotFound();
            return View(CategoryInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null)
                return NotFound();
            if (!ModelState.IsValid) return View(category);
            if (category.Id == 0)
                _unitOfWork.Category.Add(category);
            else
                _unitOfWork.Category.update(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        [AllowAnonymous]
        public IActionResult  GetAll()
        {
            var CategoryList = _unitOfWork.Category.GetAll();
            return Json(new { data = CategoryList });

            //return View("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CategoryInDb = _unitOfWork.Category.Get(id);
            if (CategoryInDb == null)
                return Json(new { success = false, message = "Error while delete data!!!" });
            _unitOfWork.Category.Remove(CategoryInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "data successfully deleted!!!" });
        }
        #endregion
    }
}
