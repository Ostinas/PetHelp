using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class PetRepository(PetHelpContext context)
    {
        private readonly PetHelpContext _context = context;

        public async Task<List<PetDto>> GetPets()
        {
            return await _context.Pets
                .Include(u => u.Owner)
                .Include(u => u.Ad)
                .Select(pet => new PetDto
                {
                    Name = pet.Name,
                    Description = pet.Description,
                    Sex = pet.Sex,
                    Age = pet.Age,
                    OwnerName = pet.Owner.Name,
                    CareStart = pet.Ad.CareStart,
                    CareEnd = pet.Ad.CareEnd
                }).ToListAsync();
        }

        public async Task<PetDto> GetPet(int id)
        {
            return await _context.Pets
                .Where(p => p.Id == id)
                .Include(u => u.Owner)
                .Include(u => u.Ad)
                .Select(pet => new PetDto
                {
                    Name = pet.Name,
                    Description = pet.Description,
                    Sex = pet.Sex,
                    Age = pet.Age,
                    OwnerName = pet.Owner.Name,
                    CareStart = pet.Ad.CareStart,
                    CareEnd = pet.Ad.CareEnd
                }).FirstOrDefaultAsync();
        }

        public async Task<int> PutPet(int id, Pet pet)
        {
            pet.Id = id;

            _context.Entry(pet).State = EntityState.Modified;

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

        public async Task<int> PostPet(Pet pet)
        {
            _context.Pets.Add(pet);
            var id = await _context.SaveChangesAsync();

            return id;
        }

        public async Task<int> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return -1;
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return 1;
        }

        public bool PetExists(int id)
        {
            return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
