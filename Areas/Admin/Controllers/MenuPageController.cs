﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travels.Data.Repository;
using Travels.Models.EF;
using X.PagedList;

namespace Travels.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuPageController : Controller
    {
        //private readonly ApplicationDbContext _context;
        IMenuPageRepository _menuPageRepository;
        public MenuPageController(IMenuPageRepository menuPageRepository)
        {
            _menuPageRepository = menuPageRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(string Searchtext, int? page = 1)
        {
            IEnumerable<MenuPage> items = await _menuPageRepository.GetAll();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Name.Contains(Searchtext)).OrderByDescending(x => x.Position);
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuPage menuPage)
        {
            if (ModelState.IsValid)
            {
                menuPage.CreatedDate = DateTime.Now;
                menuPage.ModifiedDate = DateTime.Now;
                menuPage.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                menuPage.IsActive = true;
                await _menuPageRepository.Add(menuPage);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(menuPage);
            }
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _menuPageRepository.Get((int)id) == null)
            {
                return NotFound();
            }

            var menuPage = await _menuPageRepository.Get((int)id);
            if (menuPage == null)
            {
                return NotFound();
            }
            return View(menuPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuPage menuPage)
        {
            if (id != menuPage.MenuPageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                menuPage.Modifiedby = User.FindFirstValue(ClaimTypes.NameIdentifier);
                menuPage.ModifiedDate = DateTime.Now;
                await _menuPageRepository.Update(menuPage);
                return RedirectToAction(nameof(Index));
            }
            return View(menuPage);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _menuPageRepository.Get(id);
            if (item != null)
            {
                //var DeleteItem = db.Categories.Attach(item);
                await _menuPageRepository.Delete(item);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
