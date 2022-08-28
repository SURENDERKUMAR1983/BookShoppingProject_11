using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Model.ViewModel;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Areas.Customer.Controllers
{[Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim == null)
            {
                ShoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(ShoppingCartVM);
            }
            //***
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value, includeproperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == Claim.Value, includeproperties: "Company");
            foreach (var List in ShoppingCartVM.ListCart)
            {
                List.Price = SD.GetPriceBasedOnQuantity(List.Count, List.Product.Price, List.Product.Price50, List.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (List.Count * List.Price);
                if (List.Product.Description.Length > 100)
                {
                    List.Product.Description = List.Product.Description.Substring(0, 99) + "..";
                }
            }
            return View(ShoppingCartVM);
        }
        public IActionResult plus(int cartId)
        {
            var Cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartId, includeproperties: "Product");
            if (Cart.Count > 0) 
            Cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult minus(int cartId)
        {
            var Cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartId, includeproperties: "Product");
            
            Cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult remove(int cartId)
        {
            var Cart = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.Id == cartId, includeproperties: "Product");
            _unitOfWork.ShoppingCart.Remove(cartId);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var ClaimIdentity = (ClaimsIdentity)(User.Identity);
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value, includeproperties: "Product"),
                UserAddressList=_unitOfWork.UserAddress.GetAll(x=>x.ApplicationUserId==Claim.Value)
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == Claim.Value, includeproperties: "Company");
            foreach (var List in ShoppingCartVM.ListCart)
            {
                List.Price = SD.GetPriceBasedOnQuantity(List.Count, List.Product.Price, List.Product.Price50, List.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (List.Price * List.Count);
                List.Product.Description = SD.ConvertToRawHtml(List.Product.Description);
            }
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;            
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            return View(ShoppingCartVM);
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(IFormCollection form)
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value, includeproperties: "Product"),
              UserAddressList = _unitOfWork.UserAddress.GetAll(x => x.ApplicationUserId == Claim.Value)
            };

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll
            (sc => sc.ApplicationUserId == Claim.Value, includeproperties: "Product");
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == Claim.Value, includeproperties:"Company");            
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = Claim.Value;
            ShoppingCartVM.OrderHeader.UserAddressId = Convert.ToInt32(form["OrderHeader.UserAddressId"]);            
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var List in ShoppingCartVM.ListCart)
            {
                List.Price = SD.GetPriceBasedOnQuantity(List.Count, List.Product.Price, List.Product.Price50, List.Product.Price100);
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = List.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = List.Price,
                    Count = List.Count
                    
                };
                ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Price * orderDetails.Count;
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.Ss_Session, 0);
            string stripeToken = form["stripeToken"];
            #region Stripe Payment
            if (stripeToken == null)
            {
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
            }
            else
            //Payment process
            {
                var Options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "inr",
                    Description = "OrderId:" + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                //Payment,
                var service = new ChargeService();
                Charge charge = service.Create(Options);
                if (charge.BalanceTransactionId == null)
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }
            }
            #endregion
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int id)
        {
            return View();
        }
    }
}
