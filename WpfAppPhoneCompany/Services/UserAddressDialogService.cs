using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.ViewModels;
using WpfAppPhoneCompany.Views.Windows;

namespace WpfAppPhoneCompany.Services
{
    internal class UserAddressDialogService : IUserDialog<Address>
    {
        public bool Edit(Address address)
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

    }
}
