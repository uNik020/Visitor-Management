using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using VisitorManagement.Services;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AdminLoginDto user)
        {
            try
            {
                var token = await _authService.Login(user);
                return Ok(token);
            }catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AdminRegisterDto>> PostAdmin(AdminRegisterDto adminDto)
        {
            try
            {
                var createdUser = await _authService.RegisterAdmin(adminDto);
                return Ok(createdUser);
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message;

                if (message?.Contains("UQ_Users_Email") == true)
                    return BadRequest("Email already exists.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var res = await _authService.ForgotPassword(request);
                return Ok(res);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }   
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
        {
            try
            {
                var isVerifiedCode = await _authService.VerifyResetCode(request);
                return Ok(new { verified = true });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var res = await _authService.ResetPassword(request);
                return Ok(new { message = res });
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
