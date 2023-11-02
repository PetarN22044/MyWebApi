using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Dtos.Character;
using MyWebApi.Interfaces;
using MyWebApi.Models;
using MyWebApi.Services.CharacterService;

namespace MyWebApi.Controllers
{
  [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
         
        private readonly ICharacterService _characterService;
         public CharacterController(ICharacterService characterService){
            _characterService = characterService;
         }

        [HttpGet("GetAll")]
          public async Task<ActionResult<ServiceResponce<List<GetCharacterDto>>>> Get(){

            return Ok( await _characterService.GetAllCharacters());
          }
          
          [HttpGet("{id}")]
          public async Task<ActionResult<ServiceResponce<List<GetCharacterDto>>>> GetSingle(int id){
            return Ok( await _characterService.GetCharacterById(id));
          }
        
        [HttpPost]
          public async Task<ActionResult<ServiceResponce<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter){
            
            return Ok( await _characterService.AddCharacter(newCharacter));
          }

           [HttpPut]
          public async Task<ActionResult<ServiceResponce<List<GetCharacterDto>>>> UpdatedCharacter(UpdateCharacterDto updateCharacter){
             
             var response = await _characterService.UpdateCharacter(updateCharacter);
             if (response.Data is null){
              return NotFound(response);
             }
            return Ok( response );
          }
         
         [HttpDelete("{id}")]
          public async Task<ActionResult<ServiceResponce<GetCharacterDto>>> DeleteById(int id){
             var response = await _characterService.DeleteCharacter(id);
             if(response.Data is null){
              return NotFound(response);
             }
             return Ok(response);
          }
          [HttpPost("Skill")]
          public async Task<ActionResult<ServiceResponce<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill){
            return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
          }
    }//.....
}