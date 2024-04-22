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
    class StreetsViewModel : ViewModel
    {
        private readonly IRepository<Street> _StreetsRepository;
        public StreetsViewModel(IRepository<Street> StreetsRepository)
        {
            _StreetsRepository = StreetsRepository;
        }
    }
}
