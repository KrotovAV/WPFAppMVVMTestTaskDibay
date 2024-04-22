using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfAppPhoneCompany.ViewModels
{
    class StatisticViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Street> _Streets;
        private readonly IRepository<Phone> _Phones;
        public StatisticViewModel(
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
    }
}

/*
public MainWindowViewModel(
            IRepository<Abonent> Abonents,
            IRepository<Address> Addresses,
            IRepository<Phone> Phones,
            IRepository<Street> Streets,
            IConnectAbonentService ConnectAbonentService)
        {
            _Abonents = Abonents;
            _Addresses = Addresses;
            _Phones = Phones;
            _Streets = Streets;
            _ConnectAbonentService = ConnectAbonentService;


            //Test();
           
        }
*/