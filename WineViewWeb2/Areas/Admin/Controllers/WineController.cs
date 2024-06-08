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
                ),
                GrapeList = _unitOfWork.Grape.GetAll().Select(
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
                wineVM.Wine = _unitOfWork.Wine.Get(u => u.Id == id, includeProperties: "Grapes");
                wineVM.GrapeList = _unitOfWork.Grape.GetAll().Select(
                                        u => new SelectListItem
                                        {
                                            Text = u.Name,
                                            Value = u.Id.ToString(),
                                            Selected = wineVM.Wine.Grapes.Select(g => g.Id).Contains(u.Id)
                                        });
                return View(wineVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(WineVM wineVM, IFormFile? file, string[]? grapes)
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

                if (grapes != null)
                {
                    var items = _unitOfWork.Grape.GetAll().Select(
                                u => new SelectListItem
                                {
                                    Text = u.Name,
                                    Value = u.Id.ToString()
                                }
                                );
                    wineVM.Wine.Grapes = new();
                    foreach (SelectListItem item in items)
                    {
                        if (grapes.Contains(item.Value))
                        {
                            Grape grapeFromDb = _unitOfWork.Grape.Get(u => u.Id.ToString() == item.Value, tracked:true);
                            wineVM.Wine.Grapes.Add(grapeFromDb);
                        }
                    }
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
                    Wine wineFromDb = _unitOfWork.Wine.Get(u => u.Id == wineVM.Wine.Id, includeProperties: "Grapes", tracked:true);
                    wineFromDb.Grapes.RemoveAll(u => true);
                    _unitOfWork.Wine.Update(wineVM.Wine);
                    TempData["success"] = "Wine updated successfully";
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
            wineVM.GrapeList = _unitOfWork.Grape.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
                Selected = grapes.Contains(u.Id.ToString())
            });
            return View(wineVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Wine> objWineList = _unitOfWork.Wine.GetAll(includeProperties: "Color,Winery,Style,Grapes").ToList();
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


