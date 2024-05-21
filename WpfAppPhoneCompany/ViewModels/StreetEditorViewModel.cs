using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathCore.ViewModels;
using WpfAppPhoneCompany.Models;
using DataInterfacesLayer.Interfaces;
using DataBaseLayer.Repositories;
using MathCore.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetEditorViewModel : ViewModel
    {

        //public int StreetId { get; }

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
