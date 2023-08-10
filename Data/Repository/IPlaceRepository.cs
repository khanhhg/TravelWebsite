using Travels.Models.EF;

namespace Travels.Data.Repository
{
    public interface IPlaceRepository
    {
        Task<IList<Place>> GetAll();
        Task<Place> Get(int placeId);
        Task Add(Place place);
        Task<Place> Update(Place place);
        Task Delete(Place place);
    }
}
