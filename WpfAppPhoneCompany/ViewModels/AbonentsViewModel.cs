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
    class AbonentsViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _AbonentsRepository;

        public IEnumerable<Abonent> Abonentss => _AbonentsRepository.Items.ToArray();
        public AbonentsViewModel(IRepository<Abonent> AbonentsRepository)
        {
                _AbonentsRepository = AbonentsRepository;
        }
    }

}
