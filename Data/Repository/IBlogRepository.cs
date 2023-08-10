using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public interface IBlogRepository
    {
        Task<IList<Blog>> GetAll();
        Task<Blog> Get(int blogId);
        Task Add(Blog blog);
        Task<Blog> Update(Blog blog);
        Task Delete(Blog blog);
    }
}
