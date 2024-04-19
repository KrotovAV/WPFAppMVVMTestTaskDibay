using DataBaseLayer.Entityes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Abonent> Abonents { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Street> Streets { get; set; }

        //    dotnet ef migrations add InitialMigration 
        //    dotnet ef database update
        public ApplicationContext()
        {
        }
        public ApplicationContext(DbContextOptions dbc) : base(dbc)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var config = new ConfigurationBuilder()
        //                .AddJsonFile("appsettings.json")
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .Build();

        //    optionsBuilder
        //        .UseLazyLoadingProxies()
        //        .UseSqlServer(config.GetConnectionString("Connection"));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonent>().HasOne(u => u.Street).WithMany(c => c.Abonents).HasForeignKey(u => u.StreetId);

            modelBuilder.Entity<Address>().HasOne(u => u.Street).WithMany(c => c.Addresses).HasForeignKey(u => u.StreetId);

            modelBuilder.Entity<Phone>().HasOne(u => u.Abonent).WithMany(c => c.Phones).HasForeignKey(u => u.AbonentId).OnDelete(DeleteBehavior.SetNull); ; ;
            
        }
    }
}
