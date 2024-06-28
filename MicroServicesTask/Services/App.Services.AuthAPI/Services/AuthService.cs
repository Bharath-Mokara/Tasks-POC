using System.Security.Cryptography;
using App.Services.AuthApi.Data;
using App.Services.AuthApi.Entites;
using App.Services.AuthApi.Entites.DTO;
using App.Services.AuthApi.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Services.AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager <ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            ApplicationUser? applicationUser = _applicationDbContext.Users.FirstOrDefault(user => user.Email == email);
            if(applicationUser != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //Create role if it does not exist
                    ApplicationRole applicationRole = new()
                    {
                        Name = roleName
                    };

                    _roleManager.CreateAsync(applicationRole).GetAwaiter().GetResult();
                }

                await _userManager.AddToRoleAsync(applicationUser,roleName);

                return true;
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try{
                ApplicationUser? applicationUser = _applicationDbContext.Users.FirstOrDefault(user => user.UserName == loginRequestDto.UserName);
                var result = await _signInManager.PasswordSignInAsync(applicationUser, loginRequestDto.Password,isPersistent:false, lockoutOnFailure:false);
                if(result.Succeeded)
                {
                   UserDto userDto = new()
                   {
                      ID = applicationUser.Id,
                      Email = applicationUser.Email,
                      Name = applicationUser.PersonName,
                      PhoneNumber = applicationUser.PhoneNumber
                   };

                   //Fetch the role of the user
                   var userRoles = _userManager.GetRolesAsync(applicationUser).GetAwaiter().GetResult();

                   //Generate the JWT Token for Authentication.
                   string token = _jwtService.GenerateToken(applicationUser,userRoles);

                   LoginResponseDto loginResponseDto = new(){
                        User = userDto,
                        Token = token
                   };

                   return loginResponseDto;
                }
                else{
                    return new LoginResponseDto(){
                        User = null,
                        Token = string.Empty
                    };
                }
            }
            catch(Exception ex)
            {
                //TODO: Handle the exception globally
                Console.WriteLine(ex.Message);
            }

            return new LoginResponseDto(){
                User = null,
                Token = string.Empty
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new ()
            {
                UserName = registrationRequestDto.Email,
                PersonName = registrationRequestDto.Name,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if(result.Succeeded){
                    var userToReturn = _applicationDbContext.Users.First(user => user.UserName == registrationRequestDto.Email);

                    //ToDo: Automap the below using automapper
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.PersonName,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return string.Empty;

                }
                else{
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}