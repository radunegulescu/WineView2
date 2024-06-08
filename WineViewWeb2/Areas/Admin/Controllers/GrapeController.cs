using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models.ViewModels;
using WineView2.Models;
using WineView2.Utility;

namespace GrapeView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Producer)]
    public class GrapeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GrapeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Grape grape = new Grape();
            if (id != null && id != 0)
            {
                //update
                grape = _unitOfWork.Grape.Get(u => u.Id == id);
            }
            return View(grape);
        }

        [HttpPost]
        public IActionResult Upsert(Grape grape)
        {
            if (ModelState.IsValid)
            {

                if (grape.Id == 0)
                {
                    _unitOfWork.Grape.Add(grape);
                }
                else
                {
                    _unitOfWork.Grape.Update(grape);
                }
                _unitOfWork.Save();
                TempData["success"] = "Grape created successfully";
                return RedirectToAction("Index");
            }
            return View(grape);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Grape> objGrapeList = _unitOfWork.Grape.GetAll().ToList();
            return Json(new { data = objGrapeList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var grapeToBeDeleted = _unitOfWork.Grape.Get(u => u.Id == id);
            if (grapeToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Grape.Remove(grapeToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
