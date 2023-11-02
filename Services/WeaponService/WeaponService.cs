using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MyWebApi.Data;
using MyWebApi.Dtos;
using MyWebApi.Dtos.Character;
using MyWebApi.Interfaces;

namespace MyWebApi.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context,IHttpContextAccessor httpContextAccessor,IMapper mapper){
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;

        }
        public async Task<ServiceResponce<GetCharacterDto>> AddWeapon(AddWeponDto newWeapon)
        {
            var response = new ServiceResponce<GetCharacterDto>();
            
            try{
         var character = await _context.Characters
         .FirstOrDefaultAsync(x=>x.Id == newWeapon.CharacterId &&
          x.User!.Id== int.Parse(_httpContextAccessor.HttpContext!.User
          .FindFirstValue(ClaimTypes.NameIdentifier)!
          ));
          if(character is null ){
            response.Success = false;
            response.Message ="Character not found";
            return response;
          }

          var weapon = new Weapon{
           Name= newWeapon.Name,
           Damage = newWeapon.Damage,
           Character =character
          };
          _context.Weapons.Add(weapon);
          await _context.SaveChangesAsync();
          response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
               response.Success = false;
               response.Message = ex.Message;
            }
            return response;
        }
    }
}///...