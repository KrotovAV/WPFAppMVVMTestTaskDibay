using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Services
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient <IConnectAbonentService, ConnectAbonentService>()
        //.AddTransient<ISalesService, SalesService>()
        //.AddTransient<IUserDialog, UserDialogService>()
        ;
    }
}
