using Microsoft.Extensions.DependencyInjection;
using WpfAppPhoneCompany.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.WPF;
using DataBaseLayer.Entities;

namespace WpfAppPhoneCompany.Services
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient <IConnectAbonentService, ConnectAbonentService>()
            .AddTransient<IUserDialog<Street>, UserStreetDialogService>()
            .AddTransient<IUserDialog<Abonent>, UserAbonentDialogService>()
            .AddTransient<IUserDialog<Address>, UserAddressDialogService>()
            .AddTransient<IUserDialog<Phone>, UserPhoneDialogService>()
            ;
    }
}
