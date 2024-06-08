using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models.ViewModels;
using WineView2.Models;
using WineView2.Utility;

namespace StyleView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Producer)]
    public class StyleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StyleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Style style = new Style();
            if (id != null && id != 0)
            {
                //update
                style = _unitOfWork.Style.Get(u => u.Id == id);
            }
            return View(style);
        }

        [HttpPost]
        public IActionResult Upsert(Style style)
        {
            if (ModelState.IsValid)
            {

                if (style.Id == 0)
                {
                    _unitOfWork.Style.Add(style);
                }
                else
                {
                    _unitOfWork.Style.Update(style);
                }
                _unitOfWork.Save();
                TempData["success"] = "Style created successfully";
                return RedirectToAction("Index");
            }
            return View(style);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Style> objStyleList = _unitOfWork.Style.GetAll().ToList();
            return Json(new { data = objStyleList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var styleToBeDeleted = _unitOfWork.Style.Get(u => u.Id == id);
            if (styleToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Style.Remove(styleToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
