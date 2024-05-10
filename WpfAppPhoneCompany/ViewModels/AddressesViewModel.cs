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
    class AddressesViewModel : ViewModel
    {
        private readonly IRepository<Address> _AddressesRepository;
        private readonly IUserDialog _UserDialog;

        //public IEnumerable<Address> Addresses => _AddressesRepository.Items.ToArray();

        #region Abonents : ObservableCollection<Address> - Коллекция адресов

        /// <summary>Коллекция адресов</summary>
        private ObservableCollection<Address> _Addresses;

        /// <summary>Коллекция адресов</summary>
        public ObservableCollection<Address> Addresses
        {
            get => _Addresses;
            set
            {
                if (Set(ref _Addresses, value))
                {
                    _AddressesViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Address.Street.Name), ListSortDirection.Ascending)
                        }
                    };

                    _AddressesViewSource.Filter += OnAddressesFilter;
                    _AddressesViewSource.View.Refresh();

                    OnPropertyChanged(nameof(AddressesView));
                }
            }
        }
        #endregion

        #region Поиск
        /// <summary> Искомое слово </summary>
        private string _AddressFilter;
        /// <summary> Искомое слово </summary>
        public string AddressFilter
        {
            get => _AddressFilter;
            set
            {
                if (Set(ref _AddressFilter, value))
                    _AddressesViewSource.View.Refresh();
            }
        }
        #endregion

        private CollectionViewSource _AddressesViewSource;

        public ICollectionView AddressesView => _AddressesViewSource?.View;


        #region SelectedAddress : Address - Выбранный адрес

        /// <summary>Выбранный адрес</summary>
        private Address _SelectedAddress;

        /// <summary>Выбранный адрес</summary>
        public Address SelectedAddress
        {
            get => _SelectedAddress;
            set => Set(ref _SelectedAddress, value);
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
            Addresses = new ObservableCollection<Address>(await _AddressesRepository.Items.ToArrayAsync());
        }
        #endregion

        #region Command AddNewAddressCommand - Добавление нового абонента

        /// <summary>Добавление нового адреса</summary>
        private ICommand _AddNewAddressCommand;

        /// <summary>Добавление нового адреса</summary>
        public ICommand AddNewAddressCommand => _AddNewAddressCommand
            ??= new LambdaCommand(OnAddNewAddressCommandExecuted, CanAddNewAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Добавление нового адреса</summary>
        private bool CanAddNewAddressCommandExecute() => true;

        /// <summary>Логика выполнения - Добавление нового адреса</summary>
        private void OnAddNewAddressCommandExecuted()
        {
            var new_address = new Address();

            if (!_UserDialog.Edit(new_address))
                return;

            _Addresses.Add(_AddressesRepository.Add(new_address));

            SelectedAddress = new_address;
        }
        #endregion

        #region Command RemoveAddressCommand : Удаление указанного абонента

        /// <summary>Удаление указанного абонента</summary>
        private ICommand _RemoveAddressCommand;

        /// <summary>Удаление указанного абонента</summary>
        public ICommand RemoveAddressAbonentCommand => _RemoveAddressCommand
            ??= new LambdaCommand<Address>(OnRemoveAddressCommandExecuted, CanRemoveAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанного абонента</summary>
        private bool CanRemoveAddressCommandExecute(Address p) => p != null || SelectedAddress != null;

        /// <summary>Логика выполнения - Удаление указанного абонента</summary>
        private void OnRemoveAddressCommandExecuted(Address p)
        {
            var address_to_remove = p ?? SelectedAddress;
            if (!_UserDialog.ConfirmWarning($"Вы действительно хотите удалить адрес {address_to_remove.Street.Name}?", "Удаление адреса"))
                return;

            _AddressesRepository.Remove(address_to_remove.Id);

            _Addresses.Remove(address_to_remove);
            if (ReferenceEquals(SelectedAddress, address_to_remove))
                SelectedAddress = null;
        }
        #endregion

        #region Command EditAddressCommand : Редактирование указанного адреса

        /// <summary>Редактирование указанного адреса</summary>
        private ICommand _EditAddressCommand;

        /// <summary>Редактирование указанного адреса</summary>
        public ICommand EditAddressCommand => _EditAddressCommand
            ??= new LambdaCommand<Address>(OnEditAddressCommandExecuted, CanEditAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Редактирование указанного адреса</summary>
        private bool CanEditAddressCommandExecute(Address p) => p != null || SelectedAddress != null;

        /// <summary>Логика выполнения - Редактирование указанного адреса</summary>
        private void OnEditAddressCommandExecuted(Address p)
        {
            var address_to_edit = p ?? SelectedAddress;

            if (!_UserDialog.Edit(address_to_edit))
                return;
            if (!_UserDialog.ConfirmWarning($"Сохранить изменения в {address_to_edit.Street} Дом:{address_to_edit.House} Кв.:{address_to_edit.ApartNum}?", "Сохранение изменений"))
                return;

            _AddressesRepository.Update(address_to_edit);
            AddressesView.Refresh();
            SelectedAddress = address_to_edit;

            _AddressesViewSource.View.Refresh();
            //OnPropertyChanged(nameof(AddressesView));
            
        }
        #endregion




        public AddressesViewModel(IRepository<Address> AddressesRepository,
            IUserDialog UserDialog)
        {
            _AddressesRepository = AddressesRepository;
            _UserDialog = UserDialog;
        }

        private void OnAddressesFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Address address) || string.IsNullOrEmpty(AddressFilter)) return;

            if (!address.Street.Name.Contains(AddressFilter))
                E.Accepted = false;
        }
    }
}
