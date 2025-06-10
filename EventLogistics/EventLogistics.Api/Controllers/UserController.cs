using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            // Establecer valores predeterminados
            user.CreatedBy = user.CreatedBy ?? "System";
            user.UpdatedBy = user.UpdatedBy ?? "System";
            
            var result = await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            user.UpdatedBy = user.UpdatedBy ?? "System";
            var result = await _userRepository.UpdateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}