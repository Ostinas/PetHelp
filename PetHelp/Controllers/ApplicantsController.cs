﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetHelp.Data;
using PetHelp.Models;
using PetHelp.Repositories;

namespace PetHelp.Controllers
{
    [Route("api/pets/{petId}/ads/{adId}/[controller]")]
    [ApiController]
    public class ApplicantsController : ControllerBase
    {
        private readonly ApplicantRepository _applicantRepository;
        private readonly PetRepository _petRepository;
        private readonly AdRepository _adRepository;

        public ApplicantsController(
            ApplicantRepository applicantRepository,
            PetRepository petRepository,
            AdRepository adRepository)
        {
            _applicantRepository = applicantRepository;
            _petRepository = petRepository;
            _adRepository = adRepository;
        }

        // GET: api/pets/{petId}/ads/{adId}/Applicants
        [HttpGet]
        public async Task<ActionResult<List<Applicant>>> GetApplicants(int petId, int adId)
        {
            var pet = await _petRepository.GetPet(petId);
            var ad = await _adRepository.GetAd(adId, petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }
            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            var applicants = await _applicantRepository.GetApplicants(petId, adId);
            if (applicants.IsNullOrEmpty())
            {
                return NotFound("No applicants found for the given pet and ad");
            }
            
            return Ok(applicants);
        }

        // GET: api/pets/{petId}/ads/{adId}/Applicants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> GetApplicant(int petId, int adId, int id)
        {
            var pet = await _petRepository.GetPet(petId);
            var ad = await _adRepository.GetAd(adId, petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }
            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            var applicant = await _applicantRepository.GetApplicant(id, petId, adId);

            if (applicant == null)
            {
                return NotFound("No applicant found for the given pet and ad");
            }

            return Ok(applicant);
        }

        // PUT: api/pets/{petId}/ads/{adId}/Applicants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicant(int petId, int adId, int id, Applicant applicant)
        {
            var pet = await _petRepository.GetPet(petId);
            var ad = await _adRepository.GetAd(adId, petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }
            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            if (id != applicant.Id)
            {
                return BadRequest();
            }

            try
            {
                applicant = await _applicantRepository.PutApplicant(petId, adId, id, applicant);
            }
            catch
            {
                return BadRequest();
            }

            if (applicant == null)
            {
                return NotFound("Applicant not found");
            }

            return Ok(applicant);
        }

        // POST: api/pets/{petId}/ads/{adId}/Applicants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Applicant>> PostApplicant(int petId, int adId, Applicant applicant)
        {
            var pet = await _petRepository.GetPet(petId);
            var ad = await _adRepository.GetAd(adId, petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }
            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            try
            {
                applicant = await _applicantRepository.PostApplicant(applicant, petId, adId);
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtAction("GetApplicant", applicant.Id, applicant);
        }

        // DELETE: api/pets/{petId}/ads/{adId}/Applicants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicant(int petId, int adId, int id)
        {
            var applicant = await _applicantRepository.GetApplicant(id, petId, adId);

            if (applicant == null)
            {
                return NotFound("Applicant for the given ad not found");
            }

            try
            {
                await _applicantRepository.DeleteApplicant(id, petId, adId);
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}