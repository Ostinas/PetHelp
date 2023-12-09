using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetHelp.Dtos;
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
        [Authorize(Roles = "owner,admin")]
        public async Task<ActionResult<List<ApplicantDto>>> GetApplicants(int petId, int adId)
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
        [Authorize(Roles = "owner,admin")]
        public async Task<ActionResult<ApplicantDto>> GetApplicant(int petId, int adId, int id)
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

            var applicantExists = _applicantRepository.ApplicantExists(id);

            if (!applicantExists)
            {
                return NotFound("Applicant not found");
            }

            try
            {
                applicant = await _applicantRepository.PutApplicant(petId, adId, id, applicant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
