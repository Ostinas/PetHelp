using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Dtos;
using PetHelp.Models;
using PetHelp.Repositories;

namespace PetHelp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly OwnerRepository _ownerRepository;

        public OwnersController(OwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        // GET: api/Owners
        [HttpGet]
        [Authorize(Roles = "owner,admin")]
        public async Task<ActionResult<List<OwnerDto>>> GetOwners()
        {
            var owners = await _ownerRepository.GetOwners();

            if (owners == null)
            {
                return NotFound();
            }

            return Ok(owners);
        }

        // GET: api/Owners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetOwner(int id)
        {
            var owner = await _ownerRepository.GetOwner(id);

            if (owner == null)
            {
                return NotFound();
            }

            return Ok(owner);
        }

        // PUT: api/Owners/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "owner,admin")]
        public async Task<IActionResult> PutOwner(int id, Owner owner)
        {
            var ownerExists = _ownerRepository.OwnerExists(id);

            if (!ownerExists)
            {
                return NotFound("Owner not found");
            }

            try
            {
                await _ownerRepository.PutOwner(id, owner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(owner);
        }

        // POST: api/Owners
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "owner,admin")]
        public async Task<ActionResult<Owner>> PostOwner(Owner owner)
        {
            int id;

            try
            {
                id = await _ownerRepository.PostOwner(owner);
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtAction("GetOwner", id, owner);
        }

        // DELETE: api/Owners/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "owner,admin")]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            var owner = await _ownerRepository.GetOwner(id);

            if (owner == null)
            {
                return NotFound();
            }

            try
            {
                await _ownerRepository.DeleteOwner(id);
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}
