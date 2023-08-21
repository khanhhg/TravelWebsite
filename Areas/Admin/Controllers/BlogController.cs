using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
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
        public async Task<IActionResult> Create()
        {
            ViewBag.BlogCategory = new SelectList(await _blogRepository.GetAllCategory(), "BlogCategoryId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile fileImage)
        {
            ModelState.ClearValidationState("BlogCategory");
            ModelState.MarkFieldValid("BlogCategory");
            ModelState.ClearValidationState("Image");
            ModelState.MarkFieldValid("Image");
            if (ModelState.IsValid)
            {
                if (fileImage.FileName != null)
                {
                    blog.Image = Common.Common.SaveFile(path, fileImage);                  
                }
                blog.Detail = blog.Detail.Replace("../..", String.Empty);
                blog.IsActive = true;
                blog.CreatedDate = DateTime.Now;
                blog.ModifiedDate = DateTime.Now;
                blog.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _blogRepository.Add(blog);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.BlogCategory = new SelectList(await _blogRepository.GetAllCategory(), "BlogCategoryId", "Name");
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
            else
            {
                ViewBag.BlogCategory = new SelectList(await _blogRepository.GetAllCategory(), "BlogCategoryId", "Name");
                return View(place);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog, IFormFile fileImage)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }
            ModelState.ClearValidationState("BlogCategory");
            ModelState.MarkFieldValid("BlogCategory");
            var blog_Edit = await _blogRepository.Get((int)id);
            if (ModelState.IsValid)
            {
                if (fileImage != null)
                {
                    blog_Edit.Image = Common.Common.SaveFile(path, fileImage);                
                }

                blog_Edit.Description = blog.Description;
                blog_Edit.Title = blog.Title;
                blog_Edit.Detail = blog.Detail.Replace("../..",String.Empty);
                blog_Edit.ModifiedDate = DateTime.Now;
                blog_Edit.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _blogRepository.Update(blog_Edit);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.BlogCategory = new SelectList(await _blogRepository.GetAllCategory(), "BlogCategoryId", "Name");
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
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult TinyMceUpload(IFormFile file)
        {
            string targetFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\blog\\details");
            if (file != null)
            {
                var location = Common.Parameter.BlogDetailPathImage +"/"+ Common.Common.SaveFile(targetFolder, file);             
                return Json(new { location });
            }
            else
            {
                var Error = "File not found !";
                return Json(new { Error });
            }        
        }       
    }
}
