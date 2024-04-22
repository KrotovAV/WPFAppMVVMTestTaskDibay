using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Services.Interfaces
{
    public interface IConnectAbonentService
    {
        IEnumerable<Abonent> Abonents { get; }
        Task<Abonent> ConnectAbonentAsync(string surName, string name, string secondName, Address address, params Phone[] phones);
    }
}
