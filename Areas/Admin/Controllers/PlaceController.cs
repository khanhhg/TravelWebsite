using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travels.Data.Repository;
using Travels.Models.EF;
using X.PagedList;

namespace Travels.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaceController : Controller
    {
        //private readonly ApplicationDbContext _context;
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\place");
        IPlaceRepository _placeRepository;
        public PlaceController(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<Place> items = await _placeRepository.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.PlaceName.Contains(Searchtext)).OrderByDescending(x => x.PlaceId);
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Place place, IFormFile fileImage)
        {
            if (ModelState.IsValid)
            {
                if (fileImage.FileName != null)
                {

                    FileInfo fileInfo = new FileInfo(fileImage.FileName);

                    if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = Common.Common.RandomString(12) + fileInfo.Extension;
                        string fileNameWithPath = Path.Combine(path, filename);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            fileImage.CopyTo(stream);
                        }
                        place.Image = filename;
                    }
                }
                await _placeRepository.Add(place);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(place);
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _placeRepository.Get((int)id) == null)
            {
                return NotFound();
            }

            var place = await _placeRepository.Get((int)id);
            if (place == null)
            {
                return NotFound();
            }
            return View(place);
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Place place, IFormFile fileImage)
        {
            if (id != place.PlaceId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("Products");
            ModelState.MarkFieldValid("Products");
            ModelState.ClearValidationState("fileImage");
            ModelState.MarkFieldValid("fileImage");
            var place_Edit = await _placeRepository.Get((int)id);
            if (ModelState.IsValid)
            {
                if (fileImage != null)
                {
                    FileInfo fileInfo = new FileInfo(fileImage.FileName);

                    if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".jpeg")
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filename = Common.Common.RandomString(12) + fileInfo.Extension;
                        string fileNameWithPath = Path.Combine(path, filename);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            fileImage.CopyTo(stream);
                        }
                        place_Edit.Image = filename;
                    }
                }
                place_Edit.Description = place.Description;
                place_Edit.PlaceName = place.PlaceName;
             
                await _placeRepository.Update(place_Edit);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(place);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _placeRepository.Get(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path + "\\" + item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                 await _placeRepository.Delete(item);              
                 return Json(new { success = false });              
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
