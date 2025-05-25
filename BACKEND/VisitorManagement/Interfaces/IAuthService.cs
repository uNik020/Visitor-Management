using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenDto> Login(AdminLoginDto user);
        public Task<string> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        public Task<bool> VerifyResetCode(VerifyResetCodeRequest verifyResetCodeRequest);
        public Task<string> ResetPassword(ResetPasswordRequest request);
        public Task<Admin> RegisterAdmin(AdminRegisterDto adminDto);

    }
}
