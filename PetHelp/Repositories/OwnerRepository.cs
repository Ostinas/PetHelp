using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class OwnerRepository
    {
        private readonly PetHelpContext _context;

        public OwnerRepository(PetHelpContext context)
        {
            _context = context;
        }

        public async Task<List<Owner>> GetOwners()
        {
            return await _context.Owners
                .Include(u => u.Pets)
                .Include(u => u.Ads)
                .ToListAsync();
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _context.Owners
                .Include(u => u.Pets)
                .Include(u => u.Ads)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> PutOwner(int id, Owner owner)
        {
            if (!OwnerExists(id))
            {
                return 0;
            }

            _context.Entry(owner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }

            return 1;
        }

        public async Task<int> PostOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            var id = await _context.SaveChangesAsync();

            return id;
        }

        public async Task<int> DeleteOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                return -1;
            }

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return 1;
        }

        private bool OwnerExists(int id)
        {
            return (_context.Owners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
