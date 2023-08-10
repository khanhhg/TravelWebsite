using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public interface IMenuPageRepository
    {
        Task<IList<MenuPage>> GetAll();
        Task<MenuPage> Get(int menuPageId);
        Task Add(MenuPage menuPage);
        Task<MenuPage> Update(MenuPage menuPage);
        Task Delete(MenuPage menuPage);
    }
}
