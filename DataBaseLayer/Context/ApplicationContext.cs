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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(config.GetConnectionString("Connection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonent>().HasOne(u => u.Street).WithMany(c => c.Abonents).HasForeignKey(u => u.StreetId);

            modelBuilder.Entity<Address>().HasOne(u => u.Street).WithMany(c => c.Addresses).HasForeignKey(u => u.StreetId);

            modelBuilder.Entity<Phone>().HasOne(u => u.Abonent).WithMany(c => c.Phones).HasForeignKey(u => u.AbonentId).OnDelete(DeleteBehavior.SetNull); ; ;

            //modelBuilder.Entity<Street>().HasData(
            //    new Street() { Id = 1, Name = "Садовая" },
            //    new Street() { Id = 2, Name = "Центральная" },
            //    new Street() { Id = 3, Name = "Шумная" },
            //    new Street() { Id = 4, Name = "Большая" },
            //    new Street() { Id = 5, Name = "Тихая" },
            //    new Street() { Id = 6, Name = "Загородная" }
            //);

            //modelBuilder.Entity<Address>().HasData(
            //    new Address() { Id = 1, StreetId = 1, House = 10, ApartNum = 2 },
            //    new Address() { Id = 2, StreetId = 1, House = 20, ApartNum = 321 },
            //    new Address() { Id = 3, StreetId = 1, House = 30, ApartNum = 12 },
            //    new Address() { Id = 4, StreetId = 1, House = 40, ApartNum = 125 },
            //    new Address() { Id = 5, StreetId = 2, House = 10, ApartNum = 1 },
            //    new Address() { Id = 6, StreetId = 2, House = 20, ApartNum = 11 },
            //    new Address() { Id = 7, StreetId = 2, House = 30, ApartNum = 3 },
            //    new Address() { Id = 8, StreetId = 3, House = 40, ApartNum = 35 },
            //    new Address() { Id = 9, StreetId = 3, House = 50, ApartNum = 33 },
            //    new Address() { Id = 10, StreetId = 4, House = 60, ApartNum = 45 },
            //    new Address() { Id = 11, StreetId = 5, House = 70, ApartNum = 47 },
            //    new Address() { Id = 12, StreetId = 6, House = 80, ApartNum = 55 }
            //);


           // Phone P1 = new Phone() { Id = 1, Number = 123450001, TypePhone = TypePhone.home };
           // Phone P2 = new Phone() { Id = 2, Number = 123450002, TypePhone = TypePhone.work };
           // Phone P3 = new Phone() { Id = 3, Number = 123450003, TypePhone = TypePhone.mobile };
           // Phone P4 = new Phone() { Id = 4, Number = 123450004, TypePhone = TypePhone.home };
           // Phone P5 = new Phone() { Id = 5, Number = 123450005, TypePhone = TypePhone.work };
           // Phone P6 = new Phone() { Id = 6, Number = 123450006, TypePhone = TypePhone.mobile };
           // Phone P7 = new Phone() { Id = 7, Number = 123450007, TypePhone = TypePhone.home };
           // Phone P8 = new Phone() { Id = 8, Number = 123450008, TypePhone = TypePhone.work };
           // Phone P9 = new Phone() { Id = 9, Number = 123450009, TypePhone = TypePhone.mobile };
           // Phone P10 = new Phone() { Id = 10, Number = 123450010, TypePhone = TypePhone.home };
           // Phone P11 = new Phone() { Id = 11, Number = 123450011, TypePhone = TypePhone.work };
           // Phone P12 = new Phone() { Id = 12, Number = 123450012, TypePhone = TypePhone.mobile };
           // Phone P13 = new Phone() { Id = 13, Number = 123450013, TypePhone = TypePhone.home };
           // Phone P14 = new Phone() { Id = 14, Number = 123450014, TypePhone = TypePhone.work };
           // Phone P15 = new Phone() { Id = 15, Number = 123450015, TypePhone = TypePhone.mobile };
           // Phone P16 = new Phone() { Id = 16, Number = 123450016, TypePhone = TypePhone.home };
           // Phone P17 = new Phone() { Id = 17, Number = 123450017, TypePhone = TypePhone.work };
           // Phone P18 = new Phone() { Id = 18, Number = 123450018, TypePhone = TypePhone.mobile };
           // Phone P19 = new Phone() { Id = 19, Number = 123450019, TypePhone = TypePhone.home };
           // Phone P20 = new Phone() { Id = 20, Number = 123450020, TypePhone = TypePhone.work };
           // Phone P21 = new Phone() { Id = 21, Number = 123450021, TypePhone = TypePhone.mobile };
           // Phone P22 = new Phone() { Id = 22, Number = 123450022, TypePhone = TypePhone.home };
           // Phone P23 = new Phone() { Id = 23, Number = 123450023, TypePhone = TypePhone.work };
           // Phone P24 = new Phone() { Id = 24, Number = 123450024, TypePhone = TypePhone.mobile };
           // Phone P25 = new Phone() { Id = 25, Number = 123450025, TypePhone = TypePhone.home };
           // Phone P26 = new Phone() { Id = 26, Number = 123450026, TypePhone = TypePhone.work };
           //modelBuilder.Entity<Phone>().HasData(P1, P2, P3, P4, P5, P6, P7, P8, P9, P10,
           //    P11, P12, P13, P14, P15, P16, P17, P18, P19, P20,
           //    P21, P22, P23, P24, P25, P26);

           // modelBuilder.Entity<Abonent>().HasData(

           //     new Abonent() { Id = 1, Name = "Сергей", SecondName = "Сергеевич", SurName = "Сергеев", AddressId = 1, Phones = new List<Phone>() { P1, P2, P3 } },
           //     new Abonent() { Id = 2, Name = "Николай", SecondName = "Николаевич", SurName = "Николаев", AddressId = 2, Phones = new List<Phone>() { P4, P5, P6 } },
           //     new Abonent() { Id = 3, Name = "Дмитрий", SecondName = "Дмитриевич", SurName = "Дмитриев", AddressId = 3 , Phones = new List<Phone>() { P7, P8, P9 } },
           //     new Abonent() { Id = 4, Name = "Александр", SecondName = "Александрович", SurName = "Александров", AddressId = 4, Phones = new List<Phone>() { P10, P11, P12 } }, 
           //     new Abonent() { Id = 5, Name = "Михаил", SecondName = "Михаилович", SurName = "Михайлов", AddressId = 5 , Phones = new List<Phone>() { P25, P26 } },
           //     new Abonent() { Id = 6, Name = "Арем", SecondName = "Аремович", SurName = "Артемов", AddressId = 6 , Phones = new List<Phone>() { P13, P14, P15 } },
           //     new Abonent() { Id = 7, Name = "Андрей", SecondName = "Андреевич", SurName = "Андреев", AddressId = 7, Phones = new List<Phone>() { P16, P17, P18 } },
           //     new Abonent() { Id = 8, Name = "Юрий", SecondName = "Юрьевич", SurName = "Юрьев", AddressId = 8 , Phones = new List<Phone>() { P19, P20, P21 } },
           //     new Abonent() { Id = 9, Name = "Антон", SecondName = "Антонович", SurName = "Антонов", AddressId = 9 , Phones = new List<Phone>() { P22, P23, P24 } }
           // );


            
        }
    }
}
