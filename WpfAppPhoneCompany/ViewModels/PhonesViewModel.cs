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
    class PhonesViewModel : ViewModel
    {
        private readonly IRepository<Phone> _PhonesRepository;
        public PhonesViewModel(IRepository<Phone> PhonesRepository)
        {
            _PhonesRepository = PhonesRepository;
        }
    }
}
