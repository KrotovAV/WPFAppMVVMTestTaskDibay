using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.ViewModels;
using WpfAppPhoneCompany.Views.Windows;

namespace WpfAppPhoneCompany.Services
{
    class UserDialogService : IUserDialog
    {
        public bool Edit(object item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            switch(item)
            {
                case Phone phone:
                    return EditPhone(phone);
                case Street street:
                    return EditStreet(street);
                case Abonent abonent:
                    return EditAbonent(abonent);
                case Address address:
                    return EditAddress(address);
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

        public static bool EditPhone(Phone phone)
        {
            var phone_editor_model = new PhoneEditorViewModel(phone);

            var phone_editor_window = new PhoneEditorWindow
            {
                DataContext = phone_editor_model
            };

            if (phone_editor_window.ShowDialog() != true) return false;

            phone.Number = phone_editor_model.Number;
            phone.TypePhone = phone_editor_model.TypePhone;
            phone.AbonentId = phone_editor_model.AbonentId;

            return true;
        }

        public static bool EditStreet(Street street)
        {
            var street_editor_model = new StreetEditorViewModel(street);

            var street_editor_window = new StreetEditorWindow
            {
                DataContext = street_editor_model
            };

            if (street_editor_window.ShowDialog() != true) return false;

            street.Name = street_editor_model.Name;

            return true;
        }
        public static bool EditAbonent(Abonent abonent)
        {
            var abonent_editor_model = new AbonentEditorViewModel(abonent);

            var abonent_editor_window = new AbonentEditorWindow
            {
                DataContext = abonent_editor_model
            };

            if (abonent_editor_window.ShowDialog() != true) return false;

            abonent.SurName = abonent_editor_model.SurName;
            abonent.Name = abonent_editor_model.Name;
            abonent.SecondName = abonent_editor_model.SecondName;
            abonent.AddressId = abonent_editor_model.AddressId;
            //abonent.Name = abonent_editor_model.Name;
            //abonent.Name = abonent_editor_model.Name;
            return true;
        }

        public static bool EditAddress(Address address)
        {
            var address_editor_model = new AddressEditorViewModel(address);

            var address_editor_window = new AddressEditorWindow
            {
                DataContext = address_editor_model
            };

            if (address_editor_window.ShowDialog() != true) return false;

            address.StreetId = address_editor_model.StreetId;
            address.House = address_editor_model.House;
            address.ApartNum = address_editor_model.ApartNum;
            return true;
        }
    }
}
