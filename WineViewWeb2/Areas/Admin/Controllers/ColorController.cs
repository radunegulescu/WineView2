using Microsoft.AspNetCore.Mvc;
using WineView2.DataAccess.Data;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;

namespace WineView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ColorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Color> objColorList = _unitOfWork.Color.GetAll().ToList();
            return View(objColorList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Color.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Color created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Color? colorFromDb = _unitOfWork.Color.Get(u => u.Id == id);

            if (colorFromDb == null)
            {
                return NotFound();
            }
            return View(colorFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Color obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Color.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Color updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Color? colorFromDb = _unitOfWork.Color.Get(u => u.Id == id);

            if (colorFromDb == null)
            {
                return NotFound();
            }
            return View(colorFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Color? obj = _unitOfWork.Color.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Color.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Color deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
