using App.Services.AuthApi.Entites.DTO;
using App.Services.AuthApi.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private BaseResponseDto _response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new BaseResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;

                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            return Ok(_response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);

            if(loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";

                return BadRequest(_response);
            }

            _response.Message = "User logged in successfully";
            _response.IsSuccess = true;
            _response.Result = loginResponse;

            

            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            bool assignRoleSuccessfull = await _authService.AssignRole(registrationRequestDto.Email,registrationRequestDto.Role);

            if(!assignRoleSuccessfull)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered while assigning role.";

                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.Message = "Role assigned successfully.";

            return Ok(_response);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(){
            
            await _authService.Logout();
            _response.IsSuccess = true;
            _response.Message = "User looged out successfully";

            return Ok(_response);
        }

        [Authorize]
        [HttpPost("release-access")]
        public async Task<IActionResult> ReleaseAccess(ReleaseData releaseData)
        {
            Thread.Sleep(5000);
            Console.WriteLine("Lock Released Successfully");
            // Console.WriteLine(Request.Headers.Authorization);
            //Perform the DB Operation here to release lock
            
            return Ok(new { Success = true });
        }
    }
}