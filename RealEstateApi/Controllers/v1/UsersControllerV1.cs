using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using RealEstateApi.DTOs;
using RealEstateApi.Library;
using RealEstateApi.Services.AuthService;
using RealEstateApi.Services.Common;

namespace RealEstateApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllerV1 : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly DBContext _dBContext;
        public UsersControllerV1(ICommonService commonService, IConfiguration configuration, IAuthService authService)
        {
            _commonService = commonService;
            _configuration = configuration;
            _authService = authService;
            _dBContext = new DBContext();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Register user)
        {
            var userExists = _dBContext.Users.Any(u => u.Email.Equals(user.Email));
            if (userExists)
            {
                return Ok(new CommonResponse()
                {
                    Message = Message.Failed.ToString(),
                    Errors = new List<Errors>()
                    {
                        new Errors()
                        {
                            ErrorCode = ErrorCodes.DuplicateRecord,
                            ErrorMessage=ErrorCodes.DuplicateRecord.ToString()
                        }
                    }
                });
            }
            _dBContext.Users.Add(user.MapObject<User>());
            await _dBContext.SaveChangesAsync();
            return Ok(new CommonResponse()
            {
                Success = true,
                Message = Message.Success.ToString()
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var userExists = _dBContext.Users.FirstOrDefault(u => u.Email.Equals(login.Email)
                                                            && u.Password.Equals(login.Password));
            if (userExists == null)
            {
                return Ok(new CommonResponse()
                {
                    Message = Message.Failed.ToString(),
                    Errors = new List<Errors>()
                    {
                        new Errors()
                        {
                            ErrorCode = ErrorCodes.NotFound,
                            ErrorMessage=ErrorCodes.NotFound.ToString()
                        }
                    }
                });
            }
            var token = await _authService.GenerateToken(login.Email);
            token.Username = userExists.Name;
            token.UserId = userExists.Id;

            return Ok(new CommonResponse()
            {
                Success = true,
                Message = Message.Success.ToString(),
                Data = token
            });
        }
    }
}
