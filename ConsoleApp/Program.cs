using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Net.WebSockets;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            const string connection = "Server=DESKTOP-IFVARIJ\\SQLEXPRESS;Database=PhoneCompanyDB;User Id=Admin;Password=MsSQLavk;TrustServerCertificate=True";
            using var db = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(connection).Options);

            //var abos = db.Abonents.Take(5).ToList();
            //if (abos != null)
            //{
            //    Console.WriteLine($"есть абоненты: {abos.Count()} шт");
               
            //    foreach (var abo in abos)
            //    {
            //        Console.WriteLine(abo.AddressId);
            //        var addr = db.Addresses.FirstOrDefault(x => x.Id == abo.AddressId);
            //        //Console.WriteLine(addr.Street);
            //        Console.WriteLine(addr);
            //    }
            //}
            //else Console.WriteLine($"нет абонентов");

            AbonentRepository abosRepo = new AbonentRepository(db);
            var abosR = abosRepo.Items.Take(5).ToList();
            foreach (var abo in abosR)
            {
                Console.WriteLine(abo.Name);
                Console.WriteLine(abo.Address);
                if(abo.Phones != null)
                {
                    foreach (var phone in abo.Phones)
                    {
                        Console.WriteLine(phone);
                    }
                }
            }


            AddressRepository adrsRepo = new AddressRepository(db);
            var adrsR = adrsRepo.Items.ToList();
            //foreach (var abo in abosR)
            //{
            //    Console.WriteLine(abo.Street);
            //}

            StreetRepository strsRepo = new StreetRepository(db);
            var strsR = strsRepo.Items.ToList();





            Console.WriteLine("End");
        }
    }
}