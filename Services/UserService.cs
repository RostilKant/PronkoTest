using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        private User _user;

        public UserService(IMapper mapper, ILogger<UserService> logger, UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<bool> Register(UserRegistrationDto userRegistration, ModelStateDictionary modelState)
        {
            _user = _mapper.Map<User>(userRegistration);

            var result = await _userManager.CreateAsync(_user, userRegistration.Password);

            if (!result.Succeeded)
            {
                _logger.Log(LogLevel.Error, "Something went wrong with user registration!");
                foreach (var error in result.Errors)
                {
                    modelState.TryAddModelError(error.Code, error.Description);
                }

                return false;
            }
            
            await _userManager.AddToRolesAsync(_user, userRegistration.Roles);
            return true;
        }

        public async Task<bool> Validate(UserAuthenticationDto userAuthentication, ModelStateDictionary modelState)
        {
            _user = await _userManager.FindByEmailAsync(userAuthentication.Email);
            if (_user != null && await _userManager.CheckPasswordAsync(_user, userAuthentication.Password))
            {
                return true;
            }

            modelState.TryAddModelError("wrongCredentials", "Wrong email or password");
            _logger.Log(LogLevel.Error, "Auth failed! Wrong email or password");
            return false;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();

            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<UserInfoDto> GetInformation(string userId)
        {
            _user = await _userManager.FindByIdAsync(userId);

            if (_user == null) 
                _logger.Log(LogLevel.Error, "Something went wrong! There is no such user!");
            
            return _mapper.Map<UserInfoDto>(_user);
        }

        public async Task EditInformation(string userId, UserUpdateDto userUpdate)
        {
            _user = await _userManager.FindByIdAsync(userId);
            
            if (_user == null)
                _logger.Log(LogLevel.Error, "Something went wrong! There is no such user!");
            
            _mapper.Map(userUpdate, _user);
            await _userManager.UpdateAsync(_user);
        }

        public async Task ChangePassword(string userId, ChangePassDto changePass, ModelStateDictionary modelState)
        {
            _user = await _userManager.FindByIdAsync(userId);
            
            if (_user == null) 
                _logger.Log(LogLevel.Error, "Something went wrong! There is no such user!");
            
            if (!await _userManager.CheckPasswordAsync(_user, changePass.OldPassword))
            {
                modelState.TryAddModelError("wrong-pass", "Invalid current password!");
                return;
            }

            if (changePass.OldPassword == changePass.NewPassword)
            {
                modelState.TryAddModelError("same-pass", "New password can't be such as current password!");
                return;
            }
            
            await _userManager.ChangePasswordAsync(_user, changePass.OldPassword, changePass.NewPassword);
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var secret =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Id)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());

            return claims;
        }

        private static JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtIssuer = Environment.GetEnvironmentVariable("JWTIssuer");
            var jwtAudience = Environment.GetEnvironmentVariable("JWTAudience");

            return new JwtSecurityToken
            (
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
        }
    }
}