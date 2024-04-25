using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathCore.ViewModels;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetEditorViewModel : ViewModel
    {
        
        public int StreetId { get; }
        private string _Name;
        public string Name { get => _Name; set => Set(ref _Name, value); }


        public StreetEditorViewModel()
            : this(new Street { Id = 1, Name = "Одуванчиковая!" })
        {
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public StreetEditorViewModel(Street street)
        {
            StreetId = street.Id;
            Name = street.Name;
        }
    }
}
