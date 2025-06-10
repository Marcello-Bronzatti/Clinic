using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromBody] CreateAppointmentDTO dto)
        {
            var available = await _appointmentService.IsAvailableAsync(dto);
            return Ok(new { available });
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetByProfessional(Guid professionalId, [FromQuery] DateTime date)
        {
            var appointments = await _appointmentService.GetAppointmentsByProfessionalAsync(professionalId, date);
            return Ok(appointments);
        }
    }
}
