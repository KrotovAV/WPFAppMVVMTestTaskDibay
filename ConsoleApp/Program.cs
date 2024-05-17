using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
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

            //AbonentRepository abosRepo = new AbonentRepository(db);
            //var abosR = abosRepo.Items.ToList();
            //foreach (var abo in abosR)
            //{
            //    Console.WriteLine(abo.Name);
            //    Console.WriteLine(abo.Address);
            //    if (abo.Phones != null)
            //    {
            //        foreach (var phone in abo.Phones)
            //        {
            //            Console.WriteLine(phone);
            //        }
            //    }
            //}


            PhoneRepository phosRepo = new PhoneRepository(db);
            var phosR = phosRepo.Items.ToList();

            List<Phone> phones = phosR.Where(x => x.AbonentId == 1).ToList();
            foreach (var pho in phones)
            {
                Console.WriteLine(pho);
                
            }

            AddressRepository adrsRepo = new AddressRepository(db);
            var adrsR = adrsRepo.Items.ToList();
            var adr = adrsR.FirstOrDefault(x => x.Id == 8);
            Console.WriteLine(adr);
            //foreach (var adr in adrsR)
            //{
            //    Console.WriteLine(adr.Street + " " + adr.House + " " + adr.ApartNum);
            //    Console.WriteLine(adr.Street.Abonents.Count());
            //    if (adr.Street.Abonents != null)
            //    {
            //        foreach (var abo in adr.Street.Abonents)
            //        {
            //            Console.WriteLine(abo.SurName);
            //            Console.WriteLine(abo.Name);
            //            Console.WriteLine("-----");
            //        }
            //    }
            //}

            //StreetRepository strsRepo = new StreetRepository(db);
            //var strsR = strsRepo.Items.ToList();

            //AbonentRepository abosRepo = new AbonentRepository(db);
            //var abosR = strsRepo.Items.ToList();

            //var OnlyStreets = strsRepo.Items.ToArray();
            //var OnlyAbonents = abosRepo.Items.ToArray();

            //var Abonents_Group_query = OnlyAbonents
            //    .GroupBy(abonent => abonent?.StreetId)
            //    .Select(abonents => new { StreetID = abonents.Key, Abonents = abonents.ToList() });

            //var Abonents_Group_res = Abonents_Group_query.ToList();


            //var Abonents_Streets_Join_query =
            //    Abonents_Group_res
            //    //OnlyAbonents
            //    //.GroupBy(abonent => abonent?.StreetId)
            //    //.Select(abonents => new { StreetID = abonents.Key, Abonents = abonents.ToList() }) 
            //    .Join(                      // первый набор
            //    OnlyStreets,                                                    // второй набор
            //    abonent => abonent.StreetID,                   // свойство-селектор объекта из первого набора
            //    street => street.Id,                           // свойство-селектор объекта из второго набора
            //    (abonent, street) => new { Street = street, AbonentsOfStreet = abonent.Abonents }) // результат
            //    ;

            //var Abonents_Streets_Join_query_res = Abonents_Streets_Join_query.ToList();

            //var Streets_Abonents_Join_query =
            //    OnlyStreets
            //    .Join(
            //        Abonents_Group_res,
            //        street => street.Id,
            //        abonent => abonent.StreetID,
            //         (street, abonent) => new { Street = street, AbonentsOfStreet = abonent.Abonents })
            //    ;
            //var Streets_Abonents_Join_query_res = Streets_Abonents_Join_query.ToList();

            //var Streets_Abonents_GroupJoin_query =
            //    OnlyStreets                      // первый набор
            //    .GroupJoin(
            //        OnlyAbonents,               // второй набор
            //        street => street.Id,            // свойство-селектор объекта из первого набора по которому будет идти группировка
            //        abonent => abonent.StreetId,    // свойство-селектор объекта из второго набора
            //        (street, abonents) => new { Street = street.Id, AbonentsOfStreet = abonents })
            //    ;
            //var Streets_Abonents_GroupJoin_query_res = Streets_Abonents_GroupJoin_query.ToList();



            Console.WriteLine("End");
        }
    }
}