using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class AdRepository
    {
        private readonly PetHelpContext _context;

        public AdRepository(PetHelpContext context)
        {
            _context = context;
        }

        public async Task<List<AdDto>> GetAds(int petId)
        {
            return await _context.Ads
                .Where(a => a.PetId == petId)
                .Include(a => a.Pet)
                .Include(a => a.Owner)
                .Select(a => new AdDto
                {
                    MeetingAddress = a.MeetingAddress,
                    CareStart = a.CareStart,
                    CareEnd = a.CareEnd,
                    Pay = a.Pay,
                    OwnerName = a.Owner.Name,
                    PetName = a.Pet.Name
                })
                .ToListAsync();
        }

        public async Task<AdDto> GetAd(int id, int petId)
        {
            return await _context.Ads
                .Where(a => a.Id == id && a.PetId == petId)
                .Include(a => a.Pet)
                .Include(a => a.Owner)
                .Select(a => new AdDto
                {
                    MeetingAddress = a.MeetingAddress,
                    CareStart = a.CareStart,
                    CareEnd = a.CareEnd,
                    Pay = a.Pay,
                    OwnerName = a.Owner.Name,
                    PetName = a.Pet.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Ad?> PutAd(int id, Ad ad, int petId)
        {
            ad.Id = id;
            ad.PetId = petId;

            if (!AdExists(id, petId))
            {
                return null;
            }

            var existingAd = _context.Ads.Local.SingleOrDefault(e => e.Id == id);
            if (existingAd != null)
            {
                _context.Entry(existingAd).State = EntityState.Detached;
            }

            _context.Entry(ad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null;
            }

            return ad;
        }

        public async Task<Ad> PostAd(Ad ad, int petId)
        {
            ad.PetId = petId;
            _context.Ads.Add(ad);

            await _context.SaveChangesAsync();

            var pet = await _context.Pets.FindAsync(petId);
            pet.AdId = ad.Id;

            await _context.SaveChangesAsync();

            return ad;
        }

        public async Task<int> DeleteAd(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null)
            {
                return -1;
            }

            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();
            return 1;
        }

        public bool AdExists(int id, int petId)
        {
            return (_context.Ads?.Any(e => e.Id == id && e.PetId == petId)).GetValueOrDefault();
        }
    }
}
