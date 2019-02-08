using System;
using Microsoft.EntityFrameworkCore;

namespace LuckySpin.Models
{
    public class LuckySpinDataContext: DbContext
    {
        public LuckySpinDataContext(DbContextOptions<LuckySpinDataContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        //Access to Collections representing DB tables
        public DbSet<Player> Players { get; set; }
        //public DbSet<Spin> Spins { get; set; } 


        //On Visual Studio for Mac ONLY: use the references
        //TODO: For Mac Users ONLY: follow the references to get Docker setup and uncomment line 23 below
        // - https://www.ciclosoftware.com/2018/03/14/sql-server-with-net-core-and-entityframework-on-mac/
        // - https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-2017
         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
               optionsBuilder.UseSqlServer("Server=localhost,1433; Database=LuckySpin;User=SA; Password=Pa$$word!");
        }
    }
}
