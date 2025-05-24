using VisitorManagement.DTO;

namespace VisitorManagement.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenDto> Login(LoginDto user);
        public Task<string> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        public Task<bool> VerifyResetCode(VerifyResetCodeRequest verifyResetCodeRequest);
        public Task<string> ResetPassword(ResetPasswordRequest request);
    }
}
