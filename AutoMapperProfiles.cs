using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyWebApi.Dtos.Character;
using MyWebApi.Dtos.Fight;
using MyWebApi.Dtos.SkillDto;
using MyWebApi.Dtos.User;
using MyWebApi.Models;

namespace MyWebApi
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto,Character>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill,GetSkillDto>();
            CreateMap<Character,HighscoreDto>();
        }
    }
}