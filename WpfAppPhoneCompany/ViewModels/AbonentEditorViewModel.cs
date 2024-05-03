using DataBaseLayer.Entities;
using MathCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.ViewModels
{
    class AbonentEditorViewModel : ViewModel
    {

        public int AbonentId { get; }
        private string _SurName;
        public string SurName { get => _SurName; set => Set(ref _SurName, value); }

        private string _Name;
        public string Name { get => _Name; set => Set(ref _Name, value); }

        private string _SecondName;
        public string SecondName { get => _SecondName; set => Set(ref _SecondName, value); }

        private int? _AddressId;
        public int? AddressId { get => _AddressId; set => Set(ref _AddressId, value); }



        public AbonentEditorViewModel()
            : this(new Abonent { Id = 1, Name = "Имя", SurName = "Фамилия", SecondName = "Отчество", AddressId = 1})
        {
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public AbonentEditorViewModel(Abonent abonent)
        {
            AbonentId = abonent.Id;
            Name = abonent.Name;
            SurName = abonent.SurName;
            SecondName = abonent.SecondName;
            AddressId = abonent.AddressId;
        }
    }
}
