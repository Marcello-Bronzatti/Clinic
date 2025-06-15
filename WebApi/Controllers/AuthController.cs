using Application.DTOs;
using Application.Interfaces;
using Clinic.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);

                if (token == null)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Credenciais inválidas."
                    });
                }

                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = "Acesso não autorizado.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro ao autenticar usuário.",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
        {
            try
            {
                await _authService.RegisterAsync(registerDto.Username, registerDto.Password);
                return Ok(new { message = "Usuário registrado com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Erro ao registrar usuário.",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Erro inesperado ao registrar usuário.",
                    Detail = ex.Message
                });
            }
        }
    }
}
