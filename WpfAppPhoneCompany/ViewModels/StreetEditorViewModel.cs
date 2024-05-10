using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathCore.ViewModels;
using WpfAppPhoneCompany.Models;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetEditorViewModel : ViewModel
    {

        //public int StreetId { get; }
        //private string _Name;
        //public string Name { get => _Name; set => Set(ref _Name, value); }
        private Street _Street;
        public Street Street { get =>_Street; set => Set(ref _Street, value); }

        private List<Abonent> _AbonentsOfStreet;
        public List<Abonent> AbonentsOfStreet { get => _AbonentsOfStreet; set => Set(ref _AbonentsOfStreet, value); }

        public StreetEditorViewModel()
            : this(new StreetAbonents { Street = new Street() { Id = 1, Name = "Одуванчиковая!" } })
        {
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public StreetEditorViewModel(StreetAbonents street)
        {
            Street = street.Street;
            AbonentsOfStreet = street.AbonentsOfStreet ?? new List<Abonent>();
        }
    }
}
