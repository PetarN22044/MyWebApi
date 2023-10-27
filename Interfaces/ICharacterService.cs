using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Dtos.Character;
using MyWebApi.Models;

namespace MyWebApi.Interfaces
{
    public interface ICharacterService
    {
     Task<ServiceResponce<List<GetCharacterDto>>>GetAllCharacters();
     Task<ServiceResponce<GetCharacterDto>> GetCharacterById(int id);

    Task<ServiceResponce<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
    Task<ServiceResponce<GetCharacterDto>> UpdateCharacter (UpdateCharacterDto updatedCharacter);
     
     Task<ServiceResponce<GetCharacterDto>> DeleteCharacter(int id);
    }//.
}