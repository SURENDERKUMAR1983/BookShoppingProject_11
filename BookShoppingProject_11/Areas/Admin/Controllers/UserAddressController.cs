using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Areas.Customer.Controllers
{[Area("Admin")]
    public class UserAddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        { 
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            UserAddress userAddress = new UserAddress();
            if (id == null)
            {
                userAddress.ApplicationUserId = Claim.Value;
                return View(userAddress);
            }
           var UserAddressInDb =_unitOfWork.UserAddress.Get(id.GetValueOrDefault());
            if (UserAddressInDb == null)
                return NotFound();
            return View(UserAddressInDb);           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(UserAddress userAddress)
        {
            if (userAddress == null)
                return NotFound();
            if (!ModelState.IsValid) return View(userAddress);
            if (userAddress.Id == 0)
                _unitOfWork.UserAddress.Add(userAddress);
            else
                _unitOfWork.UserAddress.Update(userAddress);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));                
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var UserAddressList = _unitOfWork.UserAddress.GetAll(); 
            return Json(new { data = UserAddressList });
        }
        [HttpDelete] 
        public IActionResult Delete(int id)
        {
            var CompanyInDb = _unitOfWork.UserAddress.Get(id);
            if (CompanyInDb == null)
                return Json(new { success = true, message = "Error while delete data" });
            _unitOfWork.UserAddress.Remove(CompanyInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "data deletedsuccessfully" });
        }
    }
}
