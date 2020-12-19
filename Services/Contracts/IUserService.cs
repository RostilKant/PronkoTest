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
    }
}