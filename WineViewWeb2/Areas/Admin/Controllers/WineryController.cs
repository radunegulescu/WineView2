using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models.ViewModels;
using WineView2.Models;
using WineView2.Utility;

namespace WineryView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Producer)]
    public class WineryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public WineryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Winery winery = new Winery();
            if (id != null && id != 0)
            {
                //update
                winery = _unitOfWork.Winery.Get(u => u.Id == id);
            }
            return View(winery);
        }

        [HttpPost]
        public IActionResult Upsert(Winery winery)
        {
            if (ModelState.IsValid)
            {

                if (winery.Id == 0)
                {
                    _unitOfWork.Winery.Add(winery);
                }
                else
                {
                    _unitOfWork.Winery.Update(winery);
                }
                _unitOfWork.Save();
                TempData["success"] = "Winery created successfully";
                return RedirectToAction("Index");
            }
            return View(winery);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Winery> objWineryList = _unitOfWork.Winery.GetAll().ToList();
            return Json(new { data = objWineryList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var wineryToBeDeleted = _unitOfWork.Winery.Get(u => u.Id == id);
            if (wineryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Winery.Remove(wineryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
