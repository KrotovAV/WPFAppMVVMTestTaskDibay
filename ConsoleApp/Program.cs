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

            var abos = db.Abonents.ToList();
            if (abos != null)
            {
                Console.WriteLine($"есть абоненты");
                foreach (var abo in abos)
                {
                    Console.WriteLine(abo.StreetId);
                    Console.WriteLine($"***");
                    var str = db.Streets.FirstOrDefault(x => x.Id == abo.StreetId);
                    if (str != null)
                    {
                        str.Abonents.Add(abo);
                        db.SaveChanges();
                    }
                }
            }
            else Console.WriteLine($"нет абонентов");


            //-------------------------


            var strs2 = db.Streets.ToList();
            if (strs2 != null)
            {
                foreach (var s in strs2)
                {
                    Console.WriteLine("*****************");
                    Console.WriteLine(s.Name);
                    Console.WriteLine("Список абонентов");

                    if (s.Abonents != null)
                    {
                        foreach (var abs in s.Abonents)
                        {
                            Console.WriteLine("абонент:" + abs.Name);

                        }
                        Console.WriteLine("---------");
                    }
                    else Console.WriteLine($"У улицы {s.Name}: нет абонентов");
                }
            }

            Console.WriteLine("------------------------");
            Console.WriteLine("/////////////////////////");
            Console.WriteLine("------------------------");

            var result = db.Streets.FirstOrDefault();
            if (result != null)
            {
                Console.WriteLine(result.Name);

                if (result.Abonents != null)
                {
                    foreach (var abs in result.Abonents)
                    {
                        Console.WriteLine("аб:" + abs.Name);

                    }
                    Console.WriteLine("---------");
                }
            }
            else Console.WriteLine(" db.Streets.FirstOrDefault() - пустой");

            Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");


            using var db2 = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(connection).Options);
            Console.WriteLine("************************");
            Console.WriteLine("------------------------");

            var result22 = db2.Streets.FirstOrDefault(x => x.Name == "Улица 4");
            if (result22 != null)
            {
                Console.WriteLine(result22.Name);

                if (result22.Abonents != null)
                {
                    foreach (var abs in result22.Abonents)
                    {
                        Console.WriteLine("аб:" + abs.Name);

                    }
                    Console.WriteLine("---------");
                }
            }


            Console.WriteLine("End");
        }
    }
}