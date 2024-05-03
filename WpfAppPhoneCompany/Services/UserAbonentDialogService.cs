using DataBaseLayer.Entities;
using MathCore.WPF;
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
    internal class UserAbonentDialogService : IUserDialog<Abonent>
    {
        public bool Edit(Abonent abonent)
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
