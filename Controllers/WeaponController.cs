using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Dtos;
using MyWebApi.Dtos.Character;
using MyWebApi.Interfaces;

namespace MyWebApi.Controllers
{
    [Authorize]
    [ApiController]
[Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        public IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService){
            _weaponService= weaponService;

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponce<GetCharacterDto>>> AddWeapon (AddWeponDto newWeapon){

            return await _weaponService.AddWeapon(newWeapon);
        }

    }
}///...