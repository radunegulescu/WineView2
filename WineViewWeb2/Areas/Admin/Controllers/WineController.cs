using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;
using WineView2.Models.ViewModels;
using WineView2.Utility;

namespace WineView2Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Producer)]
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
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                wineVM.Wine.Creator = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
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

                StringBuilder grapeNames = new StringBuilder();
                for (int i = 0; i < wineVM.Wine.Grapes.Count; i++)
                {
                    grapeNames.Append(wineVM.Wine.Grapes[i].Name);
                    if (i < wineVM.Wine.Grapes.Count - 1)
                    {
                        grapeNames.Append(", ");
                    }
                }
                var colorName = _unitOfWork.Color.Get(u => u.Id == wineVM.Wine.ColorId).Name;
                var styleName = _unitOfWork.Style.Get(u => u.Id == wineVM.Wine.StyleId).Name;

                wineVM.Wine.FullName = wineVM.Wine.Name + " " + grapeNames.ToString() + ", " + colorName + ", " + styleName;

                if (wineVM.Wine.Id == 0)
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    wineVM.Wine.ApplicationUserId = userId;

                    if (wineVM.Wine.ImageUrl == null)
                    {
                        wineVM.Wine.ImageUrl = "";
                    }

                    _unitOfWork.Wine.Add(wineVM.Wine);
                    TempData["success"] = "Wine created successfully";
                }
                else
                {
                    Wine wineFromDb = _unitOfWork.Wine.Get(u => u.Id == wineVM.Wine.Id, includeProperties: "Grapes", tracked:true);

                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (wineFromDb.ApplicationUserId != userId && !(User.IsInRole(SD.Role_Admin)))
                    {
                        return Json(new { success = false, message = "No access" });
                    }

                    wineFromDb.Grapes.RemoveAll(u => true);

                    _unitOfWork.Wine.Update(wineVM.Wine);
                    TempData["success"] = "Wine updated successfully";
                }
                _unitOfWork.Save();
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

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!(User.IsInRole(SD.Role_Admin)))
            {
                objWineList = objWineList.Where(u => u.ApplicationUserId == userId).ToList();
            }

            return Json(new { data = objWineList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            
            var wineToBeDeleted = _unitOfWork.Wine.Get(u => u.Id == id);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(wineToBeDeleted.ApplicationUserId != userId && !(User.IsInRole(SD.Role_Admin)))
            {
                return Json(new { success = false, message = "No access" });
            }

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


