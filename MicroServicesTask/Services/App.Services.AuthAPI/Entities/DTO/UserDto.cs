#nullable disable
namespace App.Services.AuthApi.Entites.DTO{
    public class UserDto
    {
        public Guid ID {get; set;}
        public string Email {get; set;}
        public string Name {get; set;}
        public string PhoneNumber {get; set;}
    }
}