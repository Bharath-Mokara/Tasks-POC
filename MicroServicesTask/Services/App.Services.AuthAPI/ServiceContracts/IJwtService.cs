using App.Services.AuthApi.Entites;

namespace App.Services.AuthApi.ServiceContracts
{
    public interface IJwtService
    {
        public string GenerateToken(ApplicationUser applicationUser,IList<string> userRoles);
    }
}