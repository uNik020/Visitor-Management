using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VisitorManagement.DTO;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;
using VisitorManagement.HelperClasses;

namespace VisitorManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly VisitorManagementContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(VisitorManagementContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }


        public async Task<string> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.Email == forgotPasswordRequest.Email);

            if (user == null)
                return "If the email is registered, you'll receive a reset code.";

            var code = new Random().Next(100000, 999999).ToString();

            var resetRequest = new PasswordResetRequest
            {
                Email = forgotPasswordRequest.Email,
                SecretCode = code,
                ExpiresAt = DateTime.Now.AddMinutes(10),
                IsUsed = false
            };
            var rowsToRemove = await _context.PasswordResetRequests.Where(p => p.Email == forgotPasswordRequest.Email).ToListAsync();
            _context.PasswordResetRequests.RemoveRange(rowsToRemove);
            _context.PasswordResetRequests.Add(resetRequest);
            await _context.SaveChangesAsync();

            // TODO: Send the code via email
            string subject = "Your Password Reset Code";
            string body = $@"
                            <html>
                                <body style='font-family: Arial, sans-serif;'>
                                    <p>Hello,</p>
                                    <p>Your password reset code is:</p>
                                    <h2 style='color:#2e6c80;'>{code}</h2>
                                    <p>This code will expire in 10 minutes.</p>
                                    <p>If you didn’t request a reset, please ignore this email.</p>
                                    <br/>
                                    <p>Thanks,<br/>TickSync Team</p>
                                </body>
                            </html>";
            await _emailService.SendEmail(forgotPasswordRequest.Email, subject, body);

            return "If the email is registered, you'll receive a reset code.";
        }
        public async Task<TokenDto> Login(LoginDto loginDto)
        {
            if (loginDto.Email != null && loginDto.Password != null)
            {
                var admin = await _context.Admins.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
                if (admin != null)
                {
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash);

                    if (isPasswordValid)
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("id",admin.AdminId.ToString()),
                        new Claim("email",admin.Email),
                        new Claim("name",admin.Username)

                    };

                       
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: signIn);

                        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                        var tokenDto = new TokenDto()
                        {
                            JwtToken = jwtToken,
                            AdminId = admin.AdminId,
                            Username = admin.Username,
                            Email = admin.Email
                        };

                        return tokenDto;
                    }
                    else
                    {
                        throw new Exception("Password didn't match!");
                    }
                }
                else
                {
                    throw new Exception("User is not valid!");
                }
            }
            else
            {
                throw new Exception("credentials are not valid!");
            }
        }

        public async Task<string> ResetPassword(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                throw new CustomException(400, "Passwords do not match.");

            var resetRecord = await _context.PasswordResetRequests
                .Where(r => r.Email == request.Email && r.SecretCode == request.SecretCode && !r.IsUsed && r.ExpiresAt > DateTime.Now)
                .FirstOrDefaultAsync();

            if (resetRecord == null)
                throw new CustomException(400, "Invalid or expired code.");

            var user = await _context.Admins.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new CustomException(400, "User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            resetRecord.IsUsed = true;

            await _context.SaveChangesAsync();
            return "Password Reset Successful";
        }
        public async Task<bool> VerifyResetCode(VerifyResetCodeRequest verifyResetCodeRequest)
        {
            var record = await _context.PasswordResetRequests
                .Where(r => r.Email == verifyResetCodeRequest.Email && !r.IsUsed && r.ExpiresAt > DateTime.Now)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            if (record == null || record.SecretCode != verifyResetCodeRequest.SecretCode)
                throw new CustomException(400, "Invalid or expired reset code.");

            return true;
        }
    }
}
