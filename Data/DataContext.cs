using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Models;

namespace MyWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options){
       


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Skill>().HasData(
            new Skill{Id=1,Name="SuperShot",Damage = 30},
            new Skill{Id=2,Name="MagicShot",Damage = 40},
            new Skill{Id=3,Name="FireShot",Damage = 50}
 );
        }
        public DbSet<Character> Characters => Set<Character>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Weapon> Weapons => Set<Weapon>();

        public DbSet<Skill> Skills => Set<Skill>();
    }
}///...