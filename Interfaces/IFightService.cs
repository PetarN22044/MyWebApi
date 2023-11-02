using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Dtos.Fight;

namespace MyWebApi.Interfaces
{
    public interface IFightService
    {
        Task<ServiceResponce<AttackResultDto>> WeaponAttactk(WeaponAttackDto request);
        Task<ServiceResponce<AttackResultDto>> SkillAttack (SkillAttackDto request);
        Task<ServiceResponce<FightResultDto>> Fight (FightRequestDto request);
          Task<ServiceResponce<List<HighscoreDto>>> GetHighscore ();

  
    }
}///...