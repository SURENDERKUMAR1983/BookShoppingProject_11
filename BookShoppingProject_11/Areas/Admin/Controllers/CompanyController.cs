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
{ [Area("Admin")]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
                return View(company);
            company = _UnitOfWork.Company.Get(id.GetValueOrDefault());            
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null)
                return NotFound();
            if (!ModelState.IsValid) return View(company);
            if (company.Id == 0)
                _UnitOfWork.Company.Add(company);
            else
                _UnitOfWork.Company.Update(company);

            _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Json(new { data = _UnitOfWork.Company.GetAll()});
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyInDb = _UnitOfWork.Company.Get(id); 
            if (CompanyInDb == null)
                return Json(new { success = true, message = "Error while delete data" });
            _UnitOfWork.Company.Remove(CompanyInDb);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "data deletedsuccessfully" });
        }
        #endregion
    }
}
