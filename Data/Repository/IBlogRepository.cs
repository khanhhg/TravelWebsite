using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public interface IBlogRepository
    {
        Task<IList<Blog>> GetAll();
        Task<IList<Blog>> GetBlogHomePage();
        Task<Blog> Get(int blogId);
        Task Add(Blog blog);
        Task<Blog> Update(Blog blog);
        Task Delete(Blog blog);
        Task<IList<BlogCategory>> GetAllCategory();
    }
}
