using AonFreelancing.Context;
using AonFreelancing.DTOs;
using AonFreelancing.DTOs.ClientDTOs;
using AonFreelancing.DTOs.FreelancerDTOs;
using AonFreelancing.Models;
using AonFreelancing.Repositories.IRepos;
using AonFreelancing.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static AonFreelancing.DTOs.ResponseDto;

namespace AonFreelancing.Controllers.Mobile.V1
{
    [Route("api/mobile/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly MyContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IJWTMangerRepo _jWTMangerRepo;

        public AuthController(UserManager<User> userManager, MyContext context, IConfiguration configuration, IUserRepo userRepo, IJWTMangerRepo jWTMangerRepo)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _userRepo = userRepo;
            _jWTMangerRepo = jWTMangerRepo;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.PhoneNumberConfirmed)
                {
                    return Unauthorized(new List<Error>()
                {
                    new Error()
                    {
                        Code = StatusCodes.Status401Unauthorized.ToString(),
                        Message = "UnAuthorized"
                    }
                });
                }
                //jwt
                var jwt = "";
                var token = await _jWTMangerRepo.GenerateToken(user.UserName);
                return Ok(new ApiResponse<string>()
                {
                    IsSuccess = true,
                    Results = token.AccessToken,
                    Errors = []
                });

            }

            return Unauthorized(new List<Error>()
                {
                    new Error()
                    {
                        Code = StatusCodes.Status401Unauthorized.ToString(),
                        Message = "UnAuthorized"
                    }
                });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            User user = new User();
            if (model.UserType == "Freelancer")
            {
                user = new Freelancer()
                {
                    Name = model.Name,
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    Skills = model.Skills,

                };
            }
            else if (model.UserType == "Client")
            {
                user = new Models.Client()
                {
                    Name = model.Name,
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    CompanyName = model.CompanyName,

                };
            }
            else
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Errors = new List<Error>() { new Error { Code="400" , Message = "Invalid UserType"} }
                });
            }

            //add user to db
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>()
                {
                    IsSuccess = false,
                    Results = null,
                    Errors = result.Errors
                            .Select(e => new Error()
                            {
                                Message = e.Description,
                                Code = e.Code,
                            }).ToList()
                });
            }
            if (model.UserType == "Freelancer")
            {
                await _userManager.AddToRoleAsync(user, Constants.USER_TYPE_FREELANCER);
            }
            if (model.UserType == "Client")
            {
                await _userManager.AddToRoleAsync(user, Constants.USER_TYPE_CLIENT);
            }
            // Get created User
            var createdUser = await _userRepo.GetUserDtoByTypeAsync(model.UserType, user);


            /*
            //generate otp verification
            //store in db
            var otp = OTPManager.GenerateOtp();
            // TO-DO(Week 05 Task)
            OTP oTP = new OTP
            {
                Code = otp,
                PhoneNumber = user.PhoneNumber,
            };
            await _context.Otps.AddAsync(oTP);
            await _context.SaveChangesAsync();
            // This should be enhanced using AppSetting 
            var accountSid = _configuration["Twilio:Sid"];
            var authToken = _configuration["Twilio:Token"];
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(_configuration["Twilio:To"])); //To
            messageOptions.From = new PhoneNumber(_configuration["Twilio:From"]);
            messageOptions.ContentSid = _configuration["Twilio:ContentSid"];
            messageOptions.ContentVariables = "{\"1\":\"" + otp + "\"}";

            var message = MessageResource.Create(messageOptions);
            */

            /*

            if (userDto.UserType == UserType.ADMIN)
            {
                await _userManager.AddToRoleAsync(newUser, "ADMIN");
            }
            if(userDto.UserType == UserType.VIEWER)
            {
                await _userManager.AddToRoleAsync(newUser, "VIEWER");
            }
            var jwtSecurityToken = await CreateJwtToken(newUser);
            var Uroles = new List<string>(await _userManager.GetRolesAsync(newUser));
            return new UserRefreshTokens
            {
                IsAuthenticated = true,
            };


            */
            
            
            
            var tokens = await _jWTMangerRepo.GenerateToken(user.UserName);

            UserRefreshTokens userRefreshTokens = new UserRefreshTokens
            {
                IsAuthenticated = true,
            };

            return Ok(new ApiResponse<object>()
            {

                IsSuccess = true,
                Errors = [],
                Results = createdUser,
                AccessToken = tokens.AccessToken,
            });
        }

        [HttpPost("verifiy")]
        public async Task<IActionResult> Verifiy([FromBody] VerifiyOTP model)
        {
            var user = await _userManager.Users.Where(x => x.PhoneNumber == model.Phone).FirstOrDefaultAsync();
            if (user != null && !await _userManager.IsPhoneNumberConfirmedAsync(user))
            {
                // Get sent OTP to the user
                // Get from DB via otps table, usernane of the sender
                // Check expiration and if it is used or not
                var sentOTP = await _context.Otps.FirstOrDefaultAsync(o=>o.Code == model.Otp && o.PhoneNumber == model.Phone);// TO-READ(Week 05 - Task)
                if(sentOTP.IsUsed)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        IsSuccess = false,
                        Results = "Activated",
                        Errors = new List<Error>() {new Error { Code = "", Message = "The code was used"} }
                    });
                }

                if(sentOTP.ExpireAt <= DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        IsSuccess = false,
                        Results = "NotActivated",
                        Errors = new List<Error>() { new Error { Code = "", Message = "The code is expired" } }
                    });
                }
                // verify OTP
                if (model.Otp.Equals(sentOTP))
                {
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    // Delete or disable sent OTP
                    return Ok(new ApiResponse<string>()
                    {
                        IsSuccess = true,
                        Results = "Activated",
                        Errors = []
                    });
                }
            }
            return Unauthorized((new ApiResponse<string>()
            {
                IsSuccess = false,
                Results = null,
                Errors = new List<Error>() {
                    new Error(){
                        Code = StatusCodes.Status401Unauthorized.ToString(),
                        Message = "UnAuthorized"
                    }
                }
            }));
        
        
    }
    }
}
