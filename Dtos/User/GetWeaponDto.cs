using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Dtos.User
{
    public class GetWeaponDto
    {
        public string Name {get;set;} = string.Empty;
        public int Damage {get;set;}
    }
}