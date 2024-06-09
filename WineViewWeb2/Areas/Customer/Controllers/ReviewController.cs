using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;
using WineView2.Models.ViewModels;
using WineView2.Utility;

namespace WineView2Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ReviewController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Review> reviewList;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            reviewList = _unitOfWork.Review.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Wine,Body");

            return View(reviewList);
        }

        public IActionResult WineReviews(int id)
        {
            IEnumerable<Review> reviewList;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.YourReview = _unitOfWork.Review.Get(u => u.ApplicationUserId == claim.Value && u.WineId == id,
                    includeProperties: "Wine,Body");
            ViewBag.WineId = id;
            reviewList = _unitOfWork.Review.GetAll(u => u.WineId == id && u.ApplicationUserId != claim.Value,
                includeProperties: "Wine,Body,ApplicationUser");

            return View(reviewList);
        }

        //GET
        public IActionResult Upsert(int wineId)
        {
            ReviewVM reviewVM = new()
            {
                Review = new(),
                BodyList = _unitOfWork.Body.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                )
            };
            ViewBag.Wine = _unitOfWork.Wine.Get(u => u.Id == wineId);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var reviewFromDb = _unitOfWork.Review.Get(u => u.WineId == wineId &&
            u.ApplicationUserId == claim.Value, includeProperties: "Wine");

            if (reviewFromDb == null)
            {
                reviewVM.Review.ApplicationUserId = claim.Value;
                reviewVM.Review.WineId = wineId;
                //create Review
                return View(reviewVM);
            }
            else
            {
                //update Review
                reviewVM.Review = reviewFromDb;
                return View(reviewVM);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ReviewVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Review.Id == 0)
                {
                    _unitOfWork.Review.Add(obj.Review);
                    TempData["success"] = "Review created successfully";
                }
                else
                {
                    _unitOfWork.Review.Update(obj.Review);
                    TempData["success"] = "Review updated successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Details", "Home", new { wineId = obj.Review.WineId });
            }
            obj.BodyList = _unitOfWork.Body.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            return View(obj);
        }


        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var reviewFromUnitOfWork = _unitOfWork.Review.Get(u => u.Id == id, includeProperties: "Wine,Body");

            if (reviewFromUnitOfWork == null)
            {
                return NotFound();
            }

            return View(reviewFromUnitOfWork);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Review.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            var wineId = obj.WineId;

            _unitOfWork.Review.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Review deleted successfully";
            return RedirectToAction("Details", "Home", new { wineId = wineId });
        }
    }
}
