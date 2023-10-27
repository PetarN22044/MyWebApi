using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Enums;

namespace MyWebApi.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }

        public RpgClass Class {get;set;} = RpgClass.Knight;

    }//..
}