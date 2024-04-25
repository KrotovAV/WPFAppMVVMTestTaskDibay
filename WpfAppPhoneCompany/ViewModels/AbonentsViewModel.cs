using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPhoneCompany.Services.Interfaces;

namespace WpfAppPhoneCompany.ViewModels
{
    class AbonentsViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _AbonentsRepository;
        private readonly IUserDialog _UserDialog;
        public IEnumerable<Abonent> Abonentss => _AbonentsRepository.Items.ToArray();
        public AbonentsViewModel(IRepository<Abonent> AbonentsRepository, IUserDialog UserDialog)
        {
            _AbonentsRepository = AbonentsRepository;
            _UserDialog = UserDialog;
        }
    }

}
