using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Models;
using PetHelp.Repositories;

namespace PetHelp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly PetRepository _petRepository;

        public PetsController(PetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        // GET: api/Pets
        [HttpGet]
        public async Task<ActionResult<List<Pet>>> GetPets()
        {
            var pets = await _petRepository.GetPets();

            if (pets == null)
            {
                return NotFound();
            }

            return Ok(pets);
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _petRepository.GetPet(id);

            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }

        // PUT: api/Pets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(int id, Pet pet)
        {
            var petExists = _petRepository.PetExists(id);

            if (!petExists)
            {
                return NotFound("Pet not found");
            }

            try
            {
                await _petRepository.PutPet(id, pet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(pet);
        }

        // POST: api/Pets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(Pet pet)
        {
            try
            {
                await _petRepository.PostPet(pet);
            }
            catch
            {
                return StatusCode(500);
            }

            return CreatedAtAction(nameof(GetPet), new { id = pet.Id }, pet);
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _petRepository.GetPet(id);

            if (pet == null)
            {
                return NotFound();
            }

            try
            {
                await _petRepository.DeletePet(id);
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}
