﻿using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAppPhoneCompany.Models;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.ViewModels;
using WpfAppPhoneCompany.Views.Windows;
using static MathCore.SpecialFunctions.Distribution;

namespace WpfAppPhoneCompany.Services
{
    public class UserDialogService : IUserDialog
    {
        private readonly IRepository<Street> _StreetsRepo;
        private readonly IRepository<Address> _AddressesRepo;
        private readonly IRepository<Phone> _PhonesRepo;
        private readonly IRepository<Abonent> _AbonentsRepo;

        public UserDialogService(IRepository<Street> StreetsRepo, 
            IRepository<Address> AddressesRepo, 
            IRepository<Phone> PhonesRepo,
            IRepository<Abonent> AbonentsRepo)
        {
            _StreetsRepo = StreetsRepo;
            _AddressesRepo = AddressesRepo;
            _PhonesRepo = PhonesRepo;
            _AbonentsRepo = AbonentsRepo;
        }

        public bool Edit(object item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            switch(item)
            {
                case Phone phone:
                    return EditPhone(phone, _AbonentsRepo);
                case StreetAbonents street:
                    return EditStreet(street);
                case Abonent abonent:
                    return EditAbonent(abonent, _AddressesRepo, _PhonesRepo);
                case AddressAbonent address:
                    return EditAddress(address, _StreetsRepo);
                default: throw new NotSupportedException($"Редактирование типа {item.GetType().Name} не поддерживается");
            }
        }

        public bool ConfirmInformation(string Information, string Caption) => MessageBox
           .Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information)
                == MessageBoxResult.Yes;

        public bool ConfirmWarning(string Warning, string Caption) => MessageBox
           .Show(
                Warning, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)
                == MessageBoxResult.Yes;

        public bool ConfirmError(string Error, string Caption) => MessageBox
           .Show(
                Error, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error)
                == MessageBoxResult.Yes;

        public static bool EditPhone(Phone phone, IRepository<Abonent> AbonentsRepo)
        {
            var phone_editor_model = new PhoneEditorViewModel(phone, AbonentsRepo);

            var phone_editor_window = new PhoneEditorWindow
            {
                DataContext = phone_editor_model
            };

            if (phone_editor_window.ShowDialog() != true) return false;

            phone.Number = phone_editor_model.Number;
            phone.TypePhone = phone_editor_model.TypeOfPhone;
            phone.AbonentId = phone_editor_model.AbonentId;

            return true;
        }

        public static bool EditStreet(StreetAbonents street)
        {
            var street_editor_model = new StreetEditorViewModel(street);

            var street_editor_window = new StreetEditorWindow
            {
                DataContext = street_editor_model
            };

            if (street_editor_window.ShowDialog() != true) return false;

            street.Street = street_editor_model.Street;

            return true;
        }
        public static bool EditAbonent(Abonent abonent, IRepository<Address> AddressesRepo, IRepository<Phone> PhonesRepo)
        {
            var abonent_editor_model = new AbonentEditorViewModel(abonent, AddressesRepo, PhonesRepo);

            var abonent_editor_window = new AbonentEditorWindow
            {
                DataContext = abonent_editor_model
            };

            if (abonent_editor_window.ShowDialog() != true) return false;

            abonent.Id = abonent_editor_model.AbonentId;
            abonent.SurName = abonent_editor_model.SurName;
            abonent.Name = abonent_editor_model.Name;
            abonent.SecondName = abonent_editor_model.SecondName;
            abonent.AddressId = abonent_editor_model.AddressId;
            abonent.StreetId = abonent_editor_model.StreetId;
            abonent.Address = abonent_editor_model.Address;
            abonent.Phones = abonent_editor_model.Phones;
            
            return true;
        }

        public static bool EditAddress(AddressAbonent address, IRepository<Street> StreetsRepo)
        {
            var address_editor_model = new AddressEditorViewModel(address, StreetsRepo);

            var address_editor_window = new AddressEditorWindow
            {
                DataContext = address_editor_model
            };

            if (address_editor_window.ShowDialog() != true) return false;

            address.Address.StreetId = address_editor_model.StreetId;
            address.Address.Street = address_editor_model.Street;
            address.Address.House = address_editor_model.House;
            address.Address.ApartNum = address_editor_model.ApartNum;

            return true;
        }
    }
}
