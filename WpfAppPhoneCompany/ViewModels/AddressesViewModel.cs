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
    class AddressesViewModel : ViewModel
    {
        private readonly IRepository<Address> _AddressesRepository;
        public IEnumerable<Address> Addresses => _AddressesRepository.Items.ToArray();
        public AddressesViewModel(IRepository<Address> AddressesRepository) 
        {
            _AddressesRepository = AddressesRepository;
        }
    }
}
