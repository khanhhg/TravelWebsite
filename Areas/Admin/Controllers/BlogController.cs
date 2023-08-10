using Microsoft.AspNetCore.Mvc;
using Travels.Data.Repository;
using Travels.Models.EF;
using X.PagedList;

namespace Travels.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\blog");
        IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<Blog> items = await _blogRepository.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext)).OrderByDescending(x => x.BlogId);
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
        public async Task<IActionResult> Create(Blog blog, IFormFile fileImage)
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
                        blog.Image = filename;
                    }
                }
                blog.IsActive = true;
                await _blogRepository.Add(blog);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(blog);
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _blogRepository.Get((int)id) == null)
            {
                return NotFound();
            }

            var place = await _blogRepository.Get((int)id);
            if (place == null)
            {
                return NotFound();
            }
            return View(place);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog, IFormFile fileImage)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("Products");
            ModelState.MarkFieldValid("Products");
            ModelState.ClearValidationState("fileImage");
            ModelState.MarkFieldValid("fileImage");
            var blog_Edit = await _blogRepository.Get((int)id);
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
                        blog_Edit.Image = filename;
                    }
                }
                blog_Edit.Description = blog.Description;
                blog_Edit.Title = blog.Title;
                blog_Edit.Detail = blog.Detail;

                await _blogRepository.Update(blog_Edit);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(blog);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _blogRepository.Get(id);
            if (item != null)
            {
                FileInfo file = new FileInfo(path + "\\" + item.Image);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                await _blogRepository.Delete(item);
                return Json(new { success = false });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
