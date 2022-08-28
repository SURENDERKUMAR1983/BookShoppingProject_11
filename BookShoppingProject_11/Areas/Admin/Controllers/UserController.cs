using BookShoppingProject.DataAccess.Data;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var UserList = _context.ApplicationUsers.Include(c => c.Company).ToList();   //ASP NetRoles
            var Roles = _context.Roles.ToList();   //ASP Net Roles 
            var UserRole = _context.UserRoles.ToList();  ///ASP Net User Role
            foreach (var user in UserList)
            {
                var RoleId = UserRole.FirstOrDefault(u=>u.UserId == user.Id).RoleId;
                user.Role = Roles.FirstOrDefault(r => r.Id == RoleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            if(!User.IsInRole(SD.Role_Admin))
            {
                var AdminInUser = UserList.FirstOrDefault(u => u.Role == SD.Role_Admin);
                UserList.Remove(AdminInUser);
            }
            return Json(new {data=UserList});
            
        }

        [HttpPost]
        public IActionResult LockUnLock([FromBody] string id)
        {
            bool isLocked = false;
            var UserInDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (UserInDb == null)
                return Json(new { success = false, message = "Error while Locking and UnLocking data" });
            if (UserInDb != null && UserInDb.LockoutEnd > DateTime.Now)
            {
                UserInDb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                UserInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "User Successfully Locked" : "User Successfully Unlocked" });
        }
        #endregion
    }
}          
             
          
  
 
    

