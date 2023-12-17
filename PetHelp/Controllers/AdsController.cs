using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Dtos;
using PetHelp.Models;
using PetHelp.Repositories;

namespace PetHelp.Controllers
{
    [Route("api/pets/{petId}/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly AdRepository _adRepository;
        private readonly PetRepository _petRepository;

        public AdsController(AdRepository adRepository, PetRepository petRepository)
        {
            _adRepository = adRepository;
            _petRepository = petRepository;
        }

        // GET: api/pets/{petId}/Ads
        [HttpGet]
        public async Task<ActionResult<List<AdDto>>> GetAds(int petId)
        {
            var pet = await _petRepository.GetPet(petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var ads = await _adRepository.GetAds(petId);

            if (ads == null)
            {
                return NotFound();
            }

            return Ok(ads);
        }

        // GET: api/pets/{petId}/Ads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdDto>> GetAd(int id, int petId)
        {
            var pet = await _petRepository.GetPet(petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var ad = await _adRepository.GetAd(id, petId);

            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            return Ok(ad);
        }

        // PUT: api/pets/{petId}/Ads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "owner,admin")]
        public async Task<IActionResult> PutAd(int id, Ad ad, int petId)
        {
            var pet = await _petRepository.GetPet(petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var adExists = _adRepository.AdExists(id, petId);

            if (!adExists)
            {
                return NotFound("Ad not found");
            }

            try
            {
                ad = await _adRepository.PutAd(id, ad, petId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (ad == null)
            {
                return NotFound();
            }

            return Ok(ad);
        }

        // POST: api/pets/{petId}/Ads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "owner,admin")]
        public async Task<ActionResult<Ad>> PostAd([FromBody] Ad ad, int petId)
        {
            var pet = await _petRepository.GetPet(petId);

            if (pet == null)
            {
                NotFound("Pet not found");
            }

            try
            {
                ad = await _adRepository.PostAd(ad, petId);
            }
            catch(Exception ex)
            {
                var check = ex;
            }

            if (ad == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetAd", new { id = ad.Id, petId }, ad);
        }

        // DELETE: api/pets/{petId}/Ads/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "owner,admin")]
        public async Task<IActionResult> DeleteAd(int id, int petId)
        {
            var pet = await _petRepository.GetPet(petId);

            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var ad = await _adRepository.GetAd(id, petId);

            if (ad == null)
            {
                return NotFound("Ad not found");
            }

            try
            {
                await _adRepository.DeleteAd(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}
