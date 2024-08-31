using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment; 
        private readonly string _storageConnectionString;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _storageConnectionString = configuration.GetConnectionString("AzureStorage");
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

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
        public IActionResult Classify()
        {
            IEnumerable<Wine> wineList = _unitOfWork.Wine.GetAll(u => u.IsInClasifier, includeProperties: "Winery,Color,Style,Grapes");
            return View(wineList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
        public async Task<IActionResult> Classify(IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                // Retrieve storage account from connection string
                BlobServiceClient blobServiceClient = new BlobServiceClient(_storageConnectionString);

                // Create a unique name for the blob
                string blobName = Path.GetFileName(file.FileName);

                // Retrieve a reference to a container
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("images");

                // Create the container if it does not exist
                await containerClient.CreateIfNotExistsAsync();

                // Retrieve reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload file to the blob
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                //string url = $"http://wineview.hxarb8eyagcxd6dm.eastus.azurecontainer.io:5000/predict?blob_name={file.FileName}";
                string url = $"http://localhost:5000/predict?blob_name={file.FileName}";

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                await blobClient.DeleteIfExistsAsync();

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    int wineClasifierId = Convert.ToInt32(result);
                    if (wineClasifierId != -1)
                    {
                        var idFromDb = _unitOfWork.Wine.Get(u => u.IsInClasifier && u.ClasifierId == wineClasifierId).Id;
                        return RedirectToAction("Details", "Home", new { wineId = idFromDb });
                    }
                    else
                    {
                        TempData["error"] = "No Wine Found!";
                    }
                }
                else
                {
                    TempData["error"] = "Error calling the Python service.";
                }
            }
            else
            {
                TempData["error"] = "Please select a file";
            }
            return RedirectToAction("Classify");
        }

        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
                public async Task<IActionResult> Classify(IFormFile? file)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        // Retrieve storage account from connection string
                        BlobServiceClient blobServiceClient = new BlobServiceClient(_storageConnectionString);

                        // Create a unique name for the blob
                        string blobName = Path.GetFileName(file.FileName);

                        // Retrieve a reference to a container
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("images");

                        // Create the container if it does not exist
                        await containerClient.CreateIfNotExistsAsync();

                        // Retrieve reference to a blob
                        BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        // Upload file to the blob
                        using (var stream = file.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }*/

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_User)]
        public IActionResult Classify(IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\temp");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }   
                var url = @"\images\temp\" + fileName + extension;
                var imgPath = Path.Combine(wwwRootPath, url.TrimStart('\\'));

                var psi = new ProcessStartInfo();
                psi.FileName = @"C:\Program Files\Python311\python.exe";
                var script = @"D:\Radu\IA\pythonProject1\Scripturi\test.py";


                psi.Arguments = script + " " + imgPath;

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                var errors = "";
                var results = "";

                using (var process = Process.Start(psi))
                {
                    errors = process.StandardError.ReadToEnd();
                    results = process.StandardOutput.ReadToEnd();
                }
                int wineClasifierId = Int16.Parse(results);
                System.IO.File.Delete(imgPath);


                if (wineClasifierId != -1)
                {
                    var idFromDb = _unitOfWork.Wine.Get(u => u.IsInClasifier && u.ClasifierId == wineClasifierId).Id;
                    return RedirectToAction("Details", "Home", new { wineId = idFromDb });
                }
                else
                {
                    TempData["error"] = "No Wine Found!";
                    return RedirectToAction("Classify");
                }
            }
            return RedirectToAction(nameof(Index));
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
