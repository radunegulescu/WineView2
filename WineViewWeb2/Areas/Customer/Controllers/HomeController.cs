using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;

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

        public IActionResult Index()
        {
            IEnumerable<Wine> wineList = _unitOfWork.Wine.GetAll(includeProperties: "Color");
            return View(wineList);
        }
        public IActionResult Details(int wineId)
        {
            Wine wine = _unitOfWork.Wine.Get(u => u.Id == wineId, includeProperties: "Color");
            return View(wine);
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
