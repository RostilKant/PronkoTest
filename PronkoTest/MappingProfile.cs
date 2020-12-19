using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace PronkoTest
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}