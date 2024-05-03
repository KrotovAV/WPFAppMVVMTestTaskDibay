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
    internal class UserPhoneDialogService : IUserDialog<Phone>
    {
        public bool Edit(Phone phone)
        {
            var street_editor_model = new PhoneEditorViewModel(phone);

            var street_editor_window = new PhoneEditorWindow
            {
                DataContext = street_editor_model
            };

            if (street_editor_window.ShowDialog() != true) return false;

            phone.Number = street_editor_model.Number;
            phone.TypePhone = street_editor_model.TypePhone;

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
