using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models.ViewModels;
using WineView2.Models;
using WineView2.Utility;

namespace BodyView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Producer)]
    public class BodyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BodyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Body body = new Body();
            if (id != null && id != 0)
            {
                //update
                body = _unitOfWork.Body.Get(u => u.Id == id);
            }
            return View(body);
        }

        [HttpPost]
        public IActionResult Upsert(Body body)
        {
            if (ModelState.IsValid)
            {

                if (body.Id == 0)
                {
                    _unitOfWork.Body.Add(body);
                }
                else
                {
                    _unitOfWork.Body.Update(body);
                }
                _unitOfWork.Save();
                TempData["success"] = "Body created successfully";
                return RedirectToAction("Index");
            }
            return View(body);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Body> objBodyList = _unitOfWork.Body.GetAll().ToList();
            return Json(new { data = objBodyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var bodyToBeDeleted = _unitOfWork.Body.Get(u => u.Id == id);
            if (bodyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Body.Remove(bodyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
