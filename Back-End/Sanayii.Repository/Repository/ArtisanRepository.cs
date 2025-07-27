
using Sanayii.Core.Entities;
using Sanayii.Repository;
using Sanayii.Repository.Data;

namespace Sanayii.Core.Repository
{
    public class ArtisanRepository : GenericRepository<Artisan>
    {
        public ArtisanRepository(SanayiiContext db) : base(db) { }

        public List<Artisan> GetTopRatedArtisans()
        {
            return db.Artisans.OrderByDescending(a => a.Rating).ToList();
        }
        public List<Artisan> GetAllArtisan()
        {
            return db.Artisans.Where(Artisan => Artisan.IsDeleted == false).ToList();
        }

        public Artisan GetArtisanById(string id)
        {
            return db.Artisans.FirstOrDefault(Artisan => Artisan.Id == id && Artisan.IsDeleted == false);
        }

    }
}