﻿using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class ApplicantRepository
    {
        private readonly PetHelpContext _context;

        public ApplicantRepository(PetHelpContext context)
        {
            _context = context;
        }

        public async Task<List<Applicant>> GetApplicants(int petId, int adId)
        {
            return await _context.Applicants
                .Where(a => a.Ads.Any(a => a.Id == adId && a.PetId == petId))
                .ToListAsync();
        }

        public async Task<Applicant> GetApplicant(int id, int petId, int adId)
        {
            return await _context.Applicants
                .Where(a => a.Ads.Any(a => a.Id == adId && a.PetId == petId))
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Applicant> PutApplicant(int petId, int adId, int id, Applicant applicant)
        {
            if (!ApplicantExists(id))
            {
                return null;
            }

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

        private bool ApplicantExists(int id)
        {
            return (_context.Applicants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}