#nullable disable
using Microsoft.AspNetCore.Identity;

namespace App.Services.AuthApi.Entites
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PersonName { get; set; }
    }
}