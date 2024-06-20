using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Services.AuthApi.Entites;
using App.Services.AuthApi.ServiceContracts;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.AuthApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(ApplicationUser applicationUser,IList<string> userRoles)
        {

            //steps to generate jwt token
            //initialize or fetch the expiration time from the configuration file
            //setup the paylod which contains the Userdetails and allready registered claims
            //setup the symmetric security key followed by setting up the signing credentials.
            //Generate the token using Token Descriptor

            double expirationTime = Convert.ToDouble(_configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<int>("EXPIRATION_TIME"));

            List<Claim> claims = new (){
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.PersonName),
            };

            foreach(string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Secret")));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Audience = _configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Audience"),
                Issuer = _configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Issuer"),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationTime),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);


        }
    }
}