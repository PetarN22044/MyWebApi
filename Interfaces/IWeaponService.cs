using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Dtos;
using MyWebApi.Dtos.Character;

namespace MyWebApi.Interfaces
{
    public interface IWeaponService
    {
        Task<ServiceResponce<GetCharacterDto>> AddWeapon (AddWeponDto newWeapon);
    }
}///...