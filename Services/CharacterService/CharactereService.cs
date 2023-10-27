using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
     public CharactereService(IMapper mapper){
            _mapper = mapper;
     }
        public async Task<ServiceResponce<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
          var serviceResponce = new ServiceResponce<List<GetCharacterDto>>();
          var character = _mapper.Map<Character>(newCharacter);
          character.Id = characters.Max(x=>x.Id) +1;
          characters.Add(character);
          serviceResponce.Data = characters.Select(x=> _mapper.Map<GetCharacterDto>(x)).ToList();
          return serviceResponce;
        }

       public async Task<ServiceResponce<GetCharacterDto>> DeleteCharacter(int id)
{
    var serviceResponce = new ServiceResponce<GetCharacterDto>(); // Користи GetCharacterDto, не List<GetCharacterDto>
    try
    {
        var character = characters.First(x => x.Id == id);
        if (character is null)
            throw new Exception($"Character with '{id}' is not found");

        characters.Remove(character);
        serviceResponce.Data = _mapper.Map<GetCharacterDto>(character); // Користи GetCharacterDto, не List<GetCharacterDto>
    }
    catch (Exception ex)
    {
        serviceResponce.Success = false;
        serviceResponce.Message = ex.Message;
    }
    return serviceResponce;
}
//..

        public async Task<ServiceResponce<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponce = new ServiceResponce<List<GetCharacterDto>>();
           serviceResponce.Data = characters.Select(x=>_mapper.Map<GetCharacterDto>(x)).ToList();
           return serviceResponce;
        }

        public async Task<ServiceResponce<GetCharacterDto>> GetCharacterById(int id)
        {
             var serviceResponce = new ServiceResponce<GetCharacterDto>();

            var character = characters.FirstOrDefault(x=>x.Id==id);
            serviceResponce.Data = _mapper.Map<GetCharacterDto>(character);

        return serviceResponce;
         }

        public async Task<ServiceResponce<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
                       var serviceResponce = new ServiceResponce<GetCharacterDto>();

          try
          {

            var character = characters.FirstOrDefault(x=>x.Id == updatedCharacter.Id);
           
           if(character is null)
           throw new Exception($"Character with ID '{updatedCharacter.Id}' is not in the view");
           
           
           character.Name = updatedCharacter.Name;
           character.Strength = updatedCharacter.Strength;
           character.Defense = updatedCharacter.Defense;
           character.HitPoints = updatedCharacter.HitPoints;
           character.Intelligence = updatedCharacter.Intelligence;
           character.Class = updatedCharacter.Class;

           serviceResponce.Data = _mapper.Map<GetCharacterDto>(character);

          }
         catch(Exception ex){

          serviceResponce.Success = false;
          serviceResponce.Message = ex.Message;
         }
          
           return serviceResponce;
        }
    }//..
}