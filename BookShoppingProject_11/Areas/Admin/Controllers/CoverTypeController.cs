using BookShoppingProject.DataAccess.Repository.IRepository;
using BookShoppingProject.Model;
using BookShoppingProject.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShoppingProject_11.Areas.Admin.Controllers
{[Area("Admin")]
   // [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Individual + "," + SD.Role_Comapny + "," + SD.Role_Employee)]
    [AllowAnonymous]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }        
        public IActionResult Upsert (int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
                return View(coverType);
            else
            {
                //store procedure
                var param = new DynamicParameters();
                param.Add("@Id", id.GetValueOrDefault());
                coverType = _unitOfWork.SP_Cal.OneRecord<CoverType>(SD.Proc_GetCoverType, param);
                //coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
                if (coverType == null)
                    return NotFound();
                return View(coverType);
            }
            

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (!ModelState.IsValid) return View(coverType);
            //store procedure
            var Param = new DynamicParameters();
            Param.Add("@Name", coverType.Name);
            if (coverType.Id == 0)
                _unitOfWork.SP_Cal.Execute(SD.Proc_CoverType_Create,Param);
            else
            {
                Param.Add("@Id", coverType.Id);
                _unitOfWork.SP_Cal.Execute(SD.Proc_CoverType_Update,Param);
            }
            //end
            if (coverType.Id == 0)
                _unitOfWork.CoverType.Add(coverType);
            else
                _unitOfWork.CoverType.Update(coverType);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
             var CoverTypeList = _unitOfWork.CoverType.GetAll();
            //var CoverTypeList = _unitOfWork.SP_Cal.List<CoverType>(SD.Proc_GetCoverType);
            return Json(new { data = CoverTypeList });
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var CoverTypeInDb = _unitOfWork.CoverType.Get(id);
            if (CoverTypeInDb == null)
                return Json(new { success = false, message = "Error while delete data" });
            // _unitOfWork.CoverType.Remove(CoverTypeInDb);
            var param = new DynamicParameters();
            param.Add("@Id", id);
            _unitOfWork.SP_Cal.Execute(SD.Proc_CoverType_Delete, param);
            return Json(new { success = true, message = "Data successfully deleted" });
        }
        #endregion
    }
}
