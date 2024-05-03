using DataBaseLayer.Entities;
using MathCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.ViewModels
{
    internal class AddressEditorViewModel : ViewModel
    {

        public int AddressId { get; }
        private int _StreetId;
        public int StreetId { get => _StreetId; set => Set(ref _StreetId, value); }

        private int _House;
        public int House { get => _House; set => Set(ref _House, value); }

        private int _ApartNum;
        public int ApartNum { get => _ApartNum; set => Set(ref _ApartNum, value); }



        public AddressEditorViewModel()
            : this(new Address { Id = 1, StreetId = 1, House = 1, ApartNum = 1 })
        {
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public AddressEditorViewModel(Address address)
        {
            StreetId = address.StreetId ?? 1;// исправить костыль
            House = address.House;
            ApartNum = address.ApartNum;
        }
    }
}
