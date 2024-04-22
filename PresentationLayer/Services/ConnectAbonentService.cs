using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PresentationLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Services
{
    public class ConnectAbonentService : IConnectAbonentService
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Phone> _Phones;
        private readonly IRepository<Street> _Streets;

        public IEnumerable<Abonent> Abonents => _Abonents.Items;

        public ConnectAbonentService(
            IRepository<Abonent> Abonents,
            IRepository<Address> Addresses,
            IRepository<Phone> Phones, 
            IRepository<Street> Streets)
        {
            _Abonents = Abonents;
            _Addresses = Addresses;
            _Phones = Phones;
            _Streets = Streets;
            
        }

        public async Task<Abonent> ConnectAbonentAsync(string SurName, string Name, string SecondName, Address Address, params Phone[] Phones)
        {
            var abo = new Abonent
            {

            };


            return await _Abonents.AddAsync(abo);
        }
    }
}
