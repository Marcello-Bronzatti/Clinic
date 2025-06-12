using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAsync()
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
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromBody] CreateAppointmentDTO dto)
        {
            var available = await _appointmentService.IsAvailableAsync(dto);
            return Ok(new { available });
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetByProfessional(Guid professionalId)
        {
            var appointments = await _appointmentService.GetAppointmentsByProfessionalAsync(professionalId);
            return Ok(appointments);
        }

        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes([FromQuery] Guid professionalId, [FromQuery] DateTime date)
        {
            var times = await _appointmentService.GetAvailableTimesAsync(professionalId, date);
            return Ok(times);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _appointmentService.CancelAsync(id);
                return NoContent();

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

    }
}
