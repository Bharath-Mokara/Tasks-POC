using App.Services.AuthApi.Entites.DTO;

namespace App.Services.AuthApi.ServiceContracts
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email,string roleName);
        Task Logout();

    }
}