using Application.DTOs;
using Application.Interfaces;
using Clinic.API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAsync()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAsync();

                var result = appointments.Select(a => new
                {
                    Id = a.Id,
                    a.ScheduledAt,
                    a.PatientName,
                    a.ProfessionalName,
                    a.ProfessionalId
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao carregar consultas.",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] CreateAppointmentDTO dto)
        {
            try
            {
                await _appointmentService.ScheduleAsync(dto);
                return Ok(new { message = "Consulta agendada com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Erro ao agendar consulta.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro inesperado ao agendar consulta.",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromBody] CreateAppointmentDTO dto)
        {
            try
            {
                var available = await _appointmentService.IsAvailableAsync(dto);
                return Ok(new { available });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inválidos para verificação.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao verificar disponibilidade.",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetByProfessional(Guid professionalId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByProfessionalAsync(professionalId);
                return Ok(appointments);
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
                    Message = "Erro ao carregar consultas do profissional.",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes([FromQuery] Guid professionalId, [FromQuery] DateTime date)
        {
            try
            {
                var times = await _appointmentService.GetAvailableTimesAsync(professionalId, date);
                return Ok(times);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Parâmetros inválidos.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao buscar horários disponíveis.",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _appointmentService.CancelAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Consulta não encontrada.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao cancelar consulta.",
                    Detail = ex.Message
                });
            }
        }
    }
}
