using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionalController : ControllerBase
    {
        private readonly ProfessionalService _service;

        public ProfessionalController(ProfessionalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var professional = await _service.GetByIdAsync(id);
            if (professional is null)
                return NotFound();

            return Ok(professional);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Professional professional)
        {
            await _service.AddAsync(professional);
            return CreatedAtAction(nameof(GetById), new { id = professional.Id }, professional);
        }
    }
}
