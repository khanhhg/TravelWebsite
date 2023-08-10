using Microsoft.EntityFrameworkCore;
using Travels.Models.EF;

namespace Travels.Data.Repository 
{
    public class PlaceRepository :IPlaceRepository
    {
        private readonly ApplicationDbContext _context;
        public PlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Place place)
        {
            _context.Add(place);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Place place)
        {
            _context.Place.Remove(place);
            await _context.SaveChangesAsync();
        }
        public async Task<IList<Place>> GetAll()
        {
            return await _context.Place.ToListAsync();
        }
        public async Task<Place> Get(int placeId)
        {
            return await _context.Place.FirstOrDefaultAsync(x => x.PlaceId == placeId);
        }
        public async Task<Place> Update(Place placeChanges)
        {
            var place = _context.Place.Attach(placeChanges);
            place.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return placeChanges;
        }
    }
}
