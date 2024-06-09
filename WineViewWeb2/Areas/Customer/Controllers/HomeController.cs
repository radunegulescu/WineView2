using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;
using WineView2.Utility;

namespace WineView2Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string? searchedWine)
        {
            IEnumerable<Wine> wineList = _unitOfWork.Wine.GetAll(includeProperties: "Winery,Color,Style,Grapes");
            if (searchedWine != null)
            {
                wineList = wineList.Where(u => u.Name.ToUpper().Contains(searchedWine.ToUpper()) ||
                                               u.Winery.Name.ToUpper().Contains(searchedWine.ToUpper()) ||
                                               u.Style.Name.ToUpper().Contains(searchedWine.ToUpper()) ||
                                               u.Color.Name.ToUpper().Contains(searchedWine.ToUpper()));
            }
            ViewBag.SearchedWine = searchedWine;
            return View(wineList);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPOST(string searchedWine)
        {
            return RedirectToAction("Index", "Home", new { searchedWine = searchedWine });
        }

        public IActionResult Details(int wineId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                WineId = wineId,
                Wine = _unitOfWork.Wine.Get(u => u.Id == wineId, includeProperties: "Color,Winery,Style,Grapes")
            };
            var reviews = _unitOfWork.Review.GetAll(u => u.WineId == wineId, includeProperties: "Body");
            if (reviews.Any())
            {
                ViewBag.Sweetness = (double)reviews.Select(u => u.Sweetness).Sum() / (double)reviews.Count();
                ViewBag.Acidity = (double)reviews.Select(u => u.Acidity).Sum() / (double)reviews.Count();
                ViewBag.Tannin = (double)reviews.Select(u => u.Tannin).Sum() / (double)reviews.Count();
                ViewBag.Body = reviews.Select(u => u.Body).GroupBy(s => s)
                             .OrderByDescending(s => s.Count())
                             .First().Key;
            }
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
                                                                   u.WineId == shoppingCart.WineId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                //add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
