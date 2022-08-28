using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Model.ViewModel;
using BookShoppingProject.Utility;
using BookShoppingProject_11;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Controllers
{[Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController>logger,IUnitOfWork unitOfWork)
        {
            _logger = logger; 
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        { 
            var productList = _unitOfWork.Product.GetAll(includeproperties: "Category,CoverType");
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim != null)
            {
                var Count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, Count);
            }
            return View(productList);
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(int id)
        {
            var ProductInDb = _unitOfWork.Product.FirstOrDefault(p => p.Id == id,includeproperties:"Category,CoverType");
            if (ProductInDb == null)
                return NotFound();
            var ShoppingCart = new ShoppingCart()
            {
                Product = ProductInDb,
                ProductId = ProductInDb.Id
            };
            return View(ShoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartobj)
        {
            shoppingCartobj.Id=0;
            if(ModelState.IsValid)
            {
                var ClaimIdentity = (ClaimsIdentity)User.Identity;
                var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCartobj.ApplicationUserId = Claim.Value;
                var ShoppingCartFromDb = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ApplicationUserId == Claim.Value && u.ProductId == shoppingCartobj.ProductId);
                if(ShoppingCartFromDb==null)
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCartobj);
                }
                else
                {
                    ShoppingCartFromDb.Count+= shoppingCartobj.Count;
                }
                _unitOfWork.Save();

                //session
                var Count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, Count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var ProductInDb = _unitOfWork.Product.FirstOrDefault(p => p.Id == shoppingCartobj.ProductId, includeproperties: "Category,CoverType");
                if (ProductInDb == null)
                    return NotFound();
                var ShoppingCart = new ShoppingCart()
                {
                    Product = ProductInDb,
                    ProductId = ProductInDb.Id
                };
                return View(ShoppingCart);
            }
            
           
        }
    }
}
