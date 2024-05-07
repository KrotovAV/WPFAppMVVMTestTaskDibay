using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class PhoneEditorViewModel : ViewModel
    {
        public int StreetId { get; }
        private int _Number;
        public int Number { get => _Number; set => Set(ref _Number, value); }

        private TypePhone _TypePhone;
        public TypePhone TypePhone { get => _TypePhone; set => Set(ref _TypePhone, value); }

        private int? _AbonentId;
        public int? AbonentId { get => _AbonentId; set => Set(ref _AbonentId, value); }


        

    public PhoneEditorViewModel()
            : this(new Phone { Id = 1, Number = 123, TypePhone = TypePhone.home, AbonentId = 0 })
        {
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public PhoneEditorViewModel(Phone phone)
        {
            StreetId = phone.Id;
            Number = phone.Number;
            TypePhone = phone.TypePhone;
            AbonentId = phone.AbonentId;
        }
    }
}
