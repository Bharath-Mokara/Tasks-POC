#nullable disable
namespace App.Services.AuthApi.Entites.DTO{
    public class LoginResponseDto
    {
       public UserDto User { get; set; }
       public string Token { get; set; }
    }
}