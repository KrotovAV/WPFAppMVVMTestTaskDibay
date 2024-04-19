﻿using DataBaseLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.Data
{
    static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
           .AddDbContext<ApplicationContext>(opt =>
           {
               var type = Configuration["Type"];
               switch (type)
               {
                   case null: throw new InvalidOperationException("Не определён тип БД");
                   default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");

                   case "MSSQL":
                       opt.UseSqlServer(Configuration.GetConnectionString("Connection"));
                       break;
                   case "InMemory":
                       opt.UseInMemoryDatabase("PhoneCompanyDB.db");
                       break;
               }
           })
           .AddTransient<DbInitializer>()
           //.AddRepositoriesInDB()
        ;
    }
}