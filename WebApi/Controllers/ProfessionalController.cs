using Application.Interfaces;
using Clinic.API.Responses;
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
        private readonly IProfessionalService _professionalService;

        public ProfessionalController(IProfessionalService service)
        {
            _professionalService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var professionals = await _professionalService.GetAllAsync();
                return Ok(professionals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao carregar profissionais.",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var professional = await _professionalService.GetByIdAsync(id);

                if (professional is null)
                    return NotFound(new ErrorResponse
                    {
                        Message = "Profissional não encontrado."
                    });

                return Ok(professional);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao buscar profissional.",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Professional professional)
        {
            try
            {
                await _professionalService.AddAsync(professional);
                return CreatedAtAction(nameof(GetById), new { id = professional.Id }, professional);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Erro ao cadastrar profissional.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro inesperado ao cadastrar profissional.",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _professionalService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Profissional não encontrado.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao excluir profissional.",
                    Detail = ex.Message
                });
            }
        }
    }
}
