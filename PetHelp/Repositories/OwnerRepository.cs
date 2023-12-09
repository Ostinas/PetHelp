using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
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

        public async Task<List<OwnerDto>> GetOwners()
        {
            return await _context.Owners
                .Select(o => new OwnerDto
                {
                    Name = o.Name,
                    Email = o.Email,
                    Password = o.Password,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address,
                    City = o.City
                })
                .ToListAsync();
        }

        public async Task<OwnerDto> GetOwner(int id)
        {
            return await _context.Owners
                .Where(o => o.Id == id)
                .Select(o => new OwnerDto
                {
                    Name = o.Name,
                    Email = o.Email,
                    Password = o.Password,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address,
                    City = o.City
                }).FirstOrDefaultAsync();
        }

        public async Task<int> PutOwner(int id, Owner owner)
        {
            owner.Id = id;
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

        public bool OwnerExists(int id)
        {
            return (_context.Owners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
