using System;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace PronkoTest
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>()
                .ForMember(user => user.UserName,
                    opt => 
                        opt.MapFrom(x => 
                            string.Join(' ', x.FirstName, x.LastName)));

            CreateMap<User, UserInfoDto>();

            CreateMap<UserUpdateDto, User>();

            CreateMap<Note, NoteDto>();

            CreateMap<NoteManipulationDto, Note>();
        }
    }
}