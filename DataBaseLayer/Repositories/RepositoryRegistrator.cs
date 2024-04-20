using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Repositories
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoriesInDB(this IServiceCollection services) => services
           .AddTransient<IRepository<Abonent>, AbonentRepository>()
           .AddTransient<IRepository<Address>, AddressRepository>()
           .AddTransient<IRepository<Phone>, PhoneRepository>()
           .AddTransient<IRepository<Street>, DbRepository<Street>>()
        ;
    }
}
