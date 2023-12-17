using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class ApplicantRepository(PetHelpContext context)
    {
        private readonly PetHelpContext _context = context;

        public async Task<List<ApplicantDto>> GetApplicants(int petId, int adId)
        {
            return await _context.Applicants
                .Where(a => a.Ads.Any(a => a.Id == adId && a.PetId == petId))
                .Include(p => p.Ads)
                    .ThenInclude(p => p.Pet)
                .Select(a => new ApplicantDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Email = a.Email,
                    Password = a.Password,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    City = a.City,
                    PetId = a.Ads.First().PetId,
                    AdId = (int)a.Ads.First().Id,
                    PetName = a.Ads.First().Pet.Name
                })
                .ToListAsync();
        }

        public async Task<ApplicantDto> GetApplicant(int id, int petId, int adId)
        {
            return await _context.Applicants
                .Where(a => a.Id == id)
                .Select(a => new ApplicantDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Email = a.Email,
                    Password = a.Password,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    City = a.City
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Applicant> PutApplicant(int petId, int adId, int id, Applicant applicant)
        {
            applicant.Id = id;
            var existingApplicant = _context.Applicants.Local.SingleOrDefault(e => e.Id == id);
            if (existingApplicant != null)
            {
                _context.Entry(existingApplicant).State = EntityState.Detached;
            }

            _context.Entry(applicant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null;
            }

            return applicant;
        }

        public async Task<Applicant> PostApplicant(Applicant applicant, int petId, int adId)
        {
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            var ad = await _context.Ads.FindAsync(adId);
            ad.Applicants.Add(applicant);

            await _context.SaveChangesAsync();

            return applicant;
        }

        public async Task<int> DeleteApplicant(int id, int petId, int adId)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return -1;
            }

            _context.Applicants.Remove(applicant);
            await _context.SaveChangesAsync();
            return 1;
        }

        public bool ApplicantExists(int id)
        {
            return (_context.Applicants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
