using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class PhonesViewModel : ViewModel
    {
        private readonly IRepository<Phone> _PhonesRepository;
        private readonly IUserDialog _UserDialog;

        #region Phones : ObservableCollection<Phone> - Коллекция телефонов

        /// <summary>Коллекция телефонов</summary>
        private ObservableCollection<Phone> _Phones;

        /// <summary>Коллекция телефонов</summary>
        public ObservableCollection<Phone> Phones
        {
            get => _Phones;
            set
            {
                if (Set(ref _Phones, value))
                {
                    _PhonesViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Phone.Number), ListSortDirection.Ascending)
                        }
                    };

                    _PhonesViewSource.Filter += OnPhonesFilter;
                    _PhonesViewSource.View.Refresh();

                    OnPropertyChanged(nameof(PhonesView));
                }
            }
        }
        #endregion

        #region Поиск
        /// <summary> Искомое слово </summary>
        private string _PhoneFilter;
        /// <summary> Искомое слово </summary>
        public string PhoneFilter
        {
            get => _PhoneFilter;
            set
            {
                if (Set(ref _PhoneFilter, value))
                    _PhonesViewSource.View.Refresh();
            }
        }
        #endregion

        private CollectionViewSource _PhonesViewSource;

        public ICollectionView PhonesView => _PhonesViewSource?.View;


        #region SelectedPhone : SelectedPhone - Выбранный телефон

        /// <summary>Выбранный телефон</summary>
        private Phone _SelectedPhone;

        /// <summary>Выбранный телефон</summary>
        public Phone SelectedPhone
        {
            get => _SelectedPhone;
            set => Set(ref _SelectedPhone, value);
        }

        #endregion

        #region Command LoadDataCommand - Команда загрузки данных из репозитория

        /// <summary>Команда загрузки данных из репозитория</summary>
        private ICommand _LoadDataCommand;

        /// <summary>Команда загрузки данных из репозитория</summary>
        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new LambdaCommandAsync(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);

        /// <summary>Проверка возможности выполнения - Команда загрузки данных из репозитория</summary>
        private bool CanLoadDataCommandExecute() => true;

        /// <summary>Логика выполнения - Команда загрузки данных из репозитория</summary>
        private async Task OnLoadDataCommandExecuted()
        {
            Phones = new ObservableCollection<Phone>(await _PhonesRepository.Items.ToArrayAsync());
        }
        #endregion


        #region Command AddNewPhoneCommand - Добавление нового телефона

        /// <summary>Добавление нового телефона</summary>
        private ICommand _AddNewPhoneCommand;

        /// <summary>Добавление нового телефона</summary>
        public ICommand AddNewPhoneCommand => _AddNewPhoneCommand
            ??= new LambdaCommand(OnAddNewPhoneCommandExecuted, CanAddNewPhoneCommandExecute);

        /// <summary>Проверка возможности выполнения - Добавление нового телефона</summary>
        private bool CanAddNewPhoneCommandExecute() => true;

        /// <summary>Логика выполнения - Добавление нового абонента</summary>
        private void OnAddNewPhoneCommandExecuted()
        {
            var new_phone = new Phone();
            
            if (!_UserDialog.Edit(new_phone))
                return;

            _Phones.Add(_PhonesRepository.Add(new_phone));

            SelectedPhone = new_phone;
        }
        #endregion

        #region Command RemovePhoneCommand : Удаление указанного телефона

        /// <summary>Удаление указанного телефона</summary>
        private ICommand _RemovePhoneCommand;

        /// <summary>Удаление указанного телефона</summary>
        public ICommand RemovePhoneCommand => _RemovePhoneCommand
            ??= new LambdaCommand<Phone>(OnRemovePhoneCommandExecuted, CanRemovePhoneCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанного телефона</summary>
        private bool CanRemovePhoneCommandExecute(Phone p) => p != null || SelectedPhone != null;

        /// <summary>Логика выполнения - Удаление указанного абонента</summary>
        private void OnRemovePhoneCommandExecuted(Phone p)
        {
            var phone_to_remove = p ?? SelectedPhone;

            if (!_UserDialog.ConfirmWarning($"Вы действительно хотите удалить телефон {phone_to_remove.Number}?", "Удаление телефона"))//if (!_UserPhoneDialog.ConfirmWarning($"Вы действительно хотите удалить телефон {phone_to_remove.Number}?", "Удаление телефона"))
                return;

            _PhonesRepository.Remove(phone_to_remove.Id);

            _Phones.Remove(phone_to_remove);
            if (ReferenceEquals(SelectedPhone, phone_to_remove))
                SelectedPhone = null;
        }
        #endregion

        #region Command EditPhoneCommand : Редактирование указанного номера

        /// <summary>Редактирование указанного номера</summary>
        private ICommand _EditPhoneCommand;

        /// <summary>Редактирование указанного номера</summary>
        public ICommand EditPhoneCommand => _EditPhoneCommand
            ??= new LambdaCommand<Phone>(OnEditPhoneCommandExecuted, CanEditPhoneCommandExecute);

        /// <summary>Проверка возможности выполнения - Редактирование указанного номера</summary>
        private bool CanEditPhoneCommandExecute(Phone p) => p != null || SelectedPhone != null;

        /// <summary>Логика выполнения - Редактирование указанного номера</summary>
        private void OnEditPhoneCommandExecuted(Phone p)
        {
            var phone_to_edit = p ?? SelectedPhone;

            if (!_UserDialog.Edit(phone_to_edit))
                return;
            if (!_UserDialog.ConfirmWarning($"Сохранить изменения в {phone_to_edit.Number} тип: {phone_to_edit.TypePhone}?", "Сохранение изменений"))
                return;

            _PhonesRepository.Update(phone_to_edit);
            PhonesView.Refresh();
            SelectedPhone = phone_to_edit;
        }
        #endregion





        public PhonesViewModel(IRepository<Phone> PhonesRepository, 
            IUserDialog UserDialog)
        {
            _PhonesRepository = PhonesRepository;
            _UserDialog = UserDialog;
        }

        private void OnPhonesFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Phone phone) || string.IsNullOrEmpty(PhoneFilter)) return;

            if (!phone.Number.ToString().Contains(PhoneFilter))
                E.Accepted = false;
        }
    }

}
