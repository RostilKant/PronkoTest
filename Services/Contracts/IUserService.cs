using System.Threading.Tasks;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<bool> Register(UserRegistrationDto userRegistration, ModelStateDictionary modelState);

        Task<bool> Validate(UserAuthenticationDto userAuthentication, ModelStateDictionary modelState);
        Task<string> CreateToken();

        Task<UserInfoDto> GetInformation(string userId);
        
        Task EditInformation(string userId, UserUpdateDto userUpdate);
        
        Task ChangePassword(string userId, ChangePassDto changePass, ModelStateDictionary modelState);


    }
}