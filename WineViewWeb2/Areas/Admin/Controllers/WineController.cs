using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;
using WineView2.Models.ViewModels;
using WineView2.Utility;

namespace WineView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class WineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WineController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            WineVM wineVM = new()
            {
                Wine = new(),
                ColorList = _unitOfWork.Color.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                ),
                WineryList = _unitOfWork.Winery.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                ),
                StyleList = _unitOfWork.Style.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                )
            };

            if (id == null || id == 0)
            {
                //create
                return View(wineVM);
            }
            else
            {
                //update
                wineVM.Wine = _unitOfWork.Wine.Get(u => u.Id == id);
                return View(wineVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(WineVM wineVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string winePath = Path.Combine(wwwRootPath, @"images\wine");

                    if (!string.IsNullOrEmpty(wineVM.Wine.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, wineVM.Wine.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(winePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    wineVM.Wine.ImageUrl = @"\images\wine\" + fileName;
                }

                if (wineVM.Wine.Id == 0)
                {
                    if(wineVM.Wine.ImageUrl == null)
                    {
                        wineVM.Wine.ImageUrl = "";
                    }
                    _unitOfWork.Wine.Add(wineVM.Wine);
                }
                else
                {
                    _unitOfWork.Wine.Update(wineVM.Wine);
                }
                _unitOfWork.Save();
                TempData["success"] = "Wine created successfully";
                return RedirectToAction("Index");
            }
            wineVM.ColorList = _unitOfWork.Color.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
            );
            wineVM.WineryList = _unitOfWork.Winery.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
            );
            wineVM.StyleList = _unitOfWork.Style.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
            );
            return View(wineVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Wine> objWineList = _unitOfWork.Wine.GetAll(includeProperties: "Color,Winery,Style").ToList();
            return Json(new { data = objWineList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var wineToBeDeleted = _unitOfWork.Wine.Get(u => u.Id == id);
            if (wineToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           wineToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Wine.Remove(wineToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}


