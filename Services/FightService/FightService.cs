using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyWebApi.Data;
using MyWebApi.Dtos.Fight;
using MyWebApi.Interfaces;

namespace MyWebApi.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public IMapper _mapper;
        public FightService(DataContext context , IMapper mapper)
        {
         _mapper = mapper;
          _context = context;
            
        }

        public async Task<ServiceResponce<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponce<FightResultDto>{
                Data = new FightResultDto()
            };
            try
            {
                   var characters = await _context.Characters
                   .Include(x=>x.Weapon)
                   .Include(x=>x.Skills)
                   .Where(x=>request.CharacterIds.Contains(x.Id))
                   .ToListAsync();
                   bool defeated = false;
                  while(!defeated){
                    foreach(var attacker in characters){
                        var opponents = characters.Where(x=>x.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;


                        bool  useWeapon = new Random().Next(2 )==0;
                        if(useWeapon && attacker.Weapon is not null)
                        {
                    attackUsed = attacker.Weapon.Name;
                    damage = DoWeaponAttack(attacker,opponent);

                        }
                        else if (!useWeapon && attacker.Skills is not null){
                     var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                     attackUsed = skill.Name;
                     damage = DoSkillAttack(attacker,opponent,skill);
                        }else{
                            response.Data.Log
                            .Add($"{attacker.Name} wasnt able to attack");
                            continue;
                        }

                        response.Data.Log
                        .Add($"{attacker.Name} attack {opponent.Name} using {attackUsed} with {(damage >=0 ? damage :0)} damage");
                  if(opponent.HitPoints <=0){
                    defeated = false;
                    attacker.Victories++;
                    opponent.Defeats++;
                    response.Data.Log.Add($"{opponent.Name} has been defeated");
                                        response.Data.Log.Add($"{attacker.Name} has wint with {attacker.HitPoints} HP");
                                        break;

                  }
                }
  }
  characters.ForEach(x=>{
    x.Fights++;
    x.HitPoints = 100;
  });
await _context.SaveChangesAsync();
            }
            catch (Exception ex){
           response.Success = false;
           response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponce<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
           
var response = new ServiceResponce<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(x => x.Skills)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                var opponent = await _context.Characters
                 .FirstOrDefaultAsync(x => x.Id == request.OpponentId);
                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Its not good here");
                }
                var skill = attacker.Skills.FirstOrDefault(x => x.Id == request.SkillId);

                if (skill is null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesnt know that skill";
                    return response;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated now";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name!,
                    Opponent = opponent.Name!,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex){
          response.Success = false;
          response.Message = ex.Message;
            }
            return response;
          
          
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defeats);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponce<AttackResultDto>> WeaponAttactk(WeaponAttackDto request)
        {
            var response = new ServiceResponce<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                var opponent = await _context.Characters
                 .FirstOrDefaultAsync(x => x.Id == request.OpponentId);
                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Its not good here");
                }
                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated now";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name!,
                    Opponent = opponent.Name!,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex){
          response.Success = false;
          response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
             if(attacker.Weapon is null)
                throw new Exception("Attacker has no weapon");
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defeats);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponce<List<HighscoreDto>>> GetHighscore()
        {
            var characters = await _context.Characters
            .Where(x=>x.Fights > 0)
            .OrderByDescending(x=>x.Victories)
            .ThenBy(x=>x.Defeats)
            .ToListAsync();

            var response = new ServiceResponce<List<HighscoreDto>>(){
                Data = characters.Select(x=>_mapper.Map<HighscoreDto>(x)).ToList()
            };
          return response;
         }//....
    }
}