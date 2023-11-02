using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Dtos.Fight;
using MyWebApi.Interfaces;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController :ControllerBase
    {
        public IFightService _fightService;
        public FightController(IFightService fightService){
            _fightService = fightService;

        }
    [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponce<AttackResultDto>>> WeaponAttack(WeaponAttackDto request){
            return Ok(await _fightService.WeaponAttactk(request) );
        }

          [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponce<AttackResultDto>>> SkillAttack(SkillAttackDto request){
            return Ok(await _fightService.SkillAttack(request) );
        }
   [HttpPost]
        public async Task<ActionResult<ServiceResponce<FightResultDto>>>Fight(FightRequestDto request){
            return Ok(await _fightService.Fight(request) );
        }
  [HttpGet]
        public async Task<ActionResult<ServiceResponce<List<HighscoreDto>>>>GetHighscore(){
            return Ok(await _fightService.GetHighscore() );
        }
    }
}