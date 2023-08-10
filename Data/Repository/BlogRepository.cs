using Microsoft.EntityFrameworkCore;
using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public class BlogRepository :IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Blog blog)
        {
            _context.Add(blog);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Blog blog)
        {
            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<Blog>> GetAll()
        {
            return await _context.Blog.ToListAsync();
        }
        public async Task<Blog> Get(int blogId)
        {
            return await _context.Blog.FirstOrDefaultAsync(x => x.BlogId == blogId);
        }
        public async Task<Blog> Update(Blog blogChanges)
        {
            var blog = _context.Blog.Attach(blogChanges);
            blog.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return blogChanges;
        }
    }
}
