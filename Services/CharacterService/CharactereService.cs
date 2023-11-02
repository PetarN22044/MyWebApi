using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MyWebApi.Data;
using MyWebApi.Dtos.Character;
using MyWebApi.Interfaces;
using MyWebApi.Models;

namespace MyWebApi.Services.CharacterService
{
    public class CharactereService : ICharacterService
    {
           private static List<Character> characters = new List<Character>{
        new Character(),
        new Character{
          Id = 1,  Name = "Superman"
        }
     };
     private readonly IMapper _mapper;
     private readonly DataContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;
     public CharactereService(IMapper mapper,DataContext context,IHttpContextAccessor httpContextAccessor ){
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
     }
     private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
     .FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceResponce<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
          var serviceResponce = new ServiceResponce<List<GetCharacterDto>>();
          var character = _mapper.Map<Character>(newCharacter);

         character.User = await _context.Users.FirstOrDefaultAsync(u=>u.Id==GetUserId());
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

          serviceResponce.Data = 
             await _context.Characters
          .Where(c=>c.User!.Id== GetUserId())
          .Select(x=> _mapper.Map<GetCharacterDto>(x)).ToListAsync ();
          return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponce = new ServiceResponce<List<GetCharacterDto>>();
            try{

                var character =await _context.Characters.FirstOrDefaultAsync(x=>x.Id == id && x.User!.Id ==GetUserId());
                if(character is null)
                throw new Exception($"Character with Id '{id}' not found");
                _context.Characters.Remove(character);
                serviceResponce.Data = await _context.Characters
                .Where(x=>x.User!.Id == GetUserId())
                .Select(x=>_mapper.Map<GetCharacterDto>(x)).ToListAsync();

            }
            catch(Exception ex){
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }



      

        public async Task<ServiceResponce<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponce = new ServiceResponce<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
            .Include(x=>x.Weapon)
            .Include(x=>x.Skills)
            .Where(x=>x.User!.Id==GetUserId()).ToListAsync();
           serviceResponce.Data = dbCharacters.Select(x=>_mapper.Map<GetCharacterDto>(x)).ToList();
           return serviceResponce;
        }

        public async Task<ServiceResponce<GetCharacterDto>> GetCharacterById(int id)
        {
             var serviceResponce = new ServiceResponce<GetCharacterDto>();

            var dbCharacter = await  _context.Characters
            .Include(x=>x.Weapon)
            .Include(x=>x.Skills)
            .FirstOrDefaultAsync(x=>x.Id==id && x.User!.Id == GetUserId());
            serviceResponce.Data = _mapper.Map<GetCharacterDto>(dbCharacter);

        return serviceResponce;
         }

        public async Task<ServiceResponce<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
          var serviceResponce = new ServiceResponce<GetCharacterDto>();

          try
          {

            var character =
            await _context.Characters
            .Include(x=>x.User)
            .FirstOrDefaultAsync(x=>x.Id == updatedCharacter.Id);
           
           if(character is null || character.User!.Id != GetUserId())
           throw new Exception($"Character with ID '{updatedCharacter.Id}' is not in the view");
           
           
           character.Name = updatedCharacter.Name;
           character.Strength = updatedCharacter.Strength;
           character.Defense = updatedCharacter.Defense;
           character.HitPoints = updatedCharacter.HitPoints;
           character.Intelligence = updatedCharacter.Intelligence;
           character.Class = updatedCharacter.Class;

await _context.SaveChangesAsync();
           serviceResponce.Data = _mapper.Map<GetCharacterDto>(character);

          }
         catch(Exception ex){

          serviceResponce.Success = false;
          serviceResponce.Message = ex.Message;
         }
          
           return serviceResponce;
        }

        public async Task<ServiceResponce<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
             var response = new ServiceResponce<GetCharacterDto>();
             try{
               var character = await _context.Characters
               .Include(x=>x.Weapon)
               .Include(x=>x.Skills)
               .FirstOrDefaultAsync(x=>x.Id==newCharacterSkill.CharacterId &&
               x.User!.Id ==GetUserId()
               );
               if(character is null){
                response.Success = false;
                response.Message = "Character is not found";
                return response;
               }
               var skill = await _context.Skills
               .FirstOrDefaultAsync(x=>x.Id== newCharacterSkill.SkillId);
                 if(skill is null){
                response.Success = false;
                response.Message = "Skill is not found";
                return response;
               }
               character.Skills!.Add(skill);
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
}