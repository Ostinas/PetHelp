using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class AuthRepository
    {
        private readonly PetHelpContext _context;

        public AuthRepository(PetHelpContext context)
        {
            _context = context;
        }

        public async Task AddUser(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<OwnerDto> GetOwner(string email)
        {
            return await _context.Owners
                .Where(o => o.Email == email)
                .Select(o => new OwnerDto
                {
                    Id = o.Id,
                    Email = o.Email,
                    Password = o.Password
                }).FirstOrDefaultAsync();
        }

        public async Task<ApplicantDto> GetApplicant(string email)
        {
            return await _context.Applicants
                .Where(o => o.Email == email)
                .Select(o => new ApplicantDto
                {
                    Id = o.Id,
                    Email = o.Email,
                    Password = o.Password
                }).FirstOrDefaultAsync();
        }
    }
}
