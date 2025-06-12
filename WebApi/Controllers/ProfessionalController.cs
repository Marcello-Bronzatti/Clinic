using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionalController : ControllerBase
    {
        private readonly ProfessionalService _professionalService;

        public ProfessionalController(ProfessionalService service)
        {
            _professionalService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _professionalService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var professional = await _professionalService.GetByIdAsync(id);
            if (professional is null)
                return NotFound();

            return Ok(professional);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Professional professional)
        {
            await _professionalService.AddAsync(professional);
            return CreatedAtAction(nameof(GetById), new { id = professional.Id }, professional);
        }
    }
}
