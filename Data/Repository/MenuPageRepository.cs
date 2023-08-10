using Microsoft.EntityFrameworkCore;
using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public class MenuPageRepository:IMenuPageRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuPageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(MenuPage menuPage)
        {
            _context.Add(menuPage);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(MenuPage menuPage)
        {
            _context.MenuPage.Remove(menuPage);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<MenuPage>> GetAll()
        {
            return await _context.MenuPage.ToListAsync();
        }

        public async Task<MenuPage> Get(int menuPageId)
        {
            return await _context.MenuPage.FirstOrDefaultAsync(x => x.MenuPageId == menuPageId);
        }

        public async Task<MenuPage> Update(MenuPage menuPageChanges)
        {
            var menuPage = _context.MenuPage.Attach(menuPageChanges);
            menuPage.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return menuPageChanges;
        }
    }
}
