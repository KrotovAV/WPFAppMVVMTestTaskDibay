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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using WpfAppPhoneCompany.Models;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class AddressesViewModel : ViewModel
    {
        private readonly IRepository<Address> _AddressesRepository;
        private readonly IUserDialog _UserDialog;

        private readonly IRepository<Abonent> _AbonentsRepository;
        private readonly IRepository<Street> _StreetsRepository;

        #region Abonents : ObservableCollection<AddressAbonent> - Коллекция адресов
        /// <summary>Коллекция адресов</summary>
        private ObservableCollection<AddressAbonent> _Addresses;

        /// <summary>Коллекция адресов</summary>
        public ObservableCollection<AddressAbonent> Addresses
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
        private AddressAbonent _SelectedAddress;

        /// <summary>Выбранный адрес</summary>
        public AddressAbonent SelectedAddress
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
            var OnlyAddresses = await _AddressesRepository.Items.ToArrayAsync();
            var OnlyAbonents = await _AbonentsRepository.Items.ToArrayAsync();

            var Streets_Abonents_Join_query =
                OnlyAddresses                      // первый набор
                .Join(
                    OnlyAbonents,               // второй набор
                    address => address.Id,            // свойство-селектор объекта из первого набора
                    abonent => abonent.AddressId,    // свойство-селектор объекта из второго набора
                    (address, abonent) => new AddressAbonent { Address = address, Abonent = abonent }) // желаемый результат
                ;

            Addresses = new ObservableCollection<AddressAbonent>(Streets_Abonents_Join_query.ToArray());
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
            var new_address = new AddressAbonent { Address = new Address() };

            if (!_UserDialog.Edit(new_address))
                return;

            //_Addresses.Add(_AddressesRepository.Add(new_address));
            _AddressesRepository.Add(new_address.Address);
            _Addresses.Add(new_address);

            SelectedAddress = new_address;
        }
        #endregion

        #region Command RemoveAddressCommand : Удаление указанного абонента
        /// <summary>Удаление указанного абонента</summary>
        private ICommand _RemoveAddressCommand;

        /// <summary>Удаление указанного абонента</summary>
        public ICommand RemoveAddressAbonentCommand => _RemoveAddressCommand
            ??= new LambdaCommand<AddressAbonent>(OnRemoveAddressCommandExecuted, CanRemoveAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанного абонента</summary>
        private bool CanRemoveAddressCommandExecute(AddressAbonent p) => p != null || SelectedAddress != null;

        /// <summary>Логика выполнения - Удаление указанного абонента</summary>
        private void OnRemoveAddressCommandExecuted(AddressAbonent p)
        {
            var address_to_remove = p ?? SelectedAddress;
            if (!_UserDialog.ConfirmWarning($"Вы действительно хотите удалить адрес {address_to_remove.Address.Street.Name} д.{address_to_remove.Address.House} кв.{address_to_remove.Address.ApartNum}?", "Удаление адреса"))
                return;

            _AddressesRepository.Remove(address_to_remove.Address.Id);
            _StreetsRepository.Update(address_to_remove.Address.Street);

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
            ??= new LambdaCommand<AddressAbonent>(OnEditAddressCommandExecuted, CanEditAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Редактирование указанного адреса</summary>
        private bool CanEditAddressCommandExecute(AddressAbonent p) => p != null || SelectedAddress != null;

        /// <summary>Логика выполнения - Редактирование указанного адреса</summary>
        private void OnEditAddressCommandExecuted(AddressAbonent p)
        {
            var address_to_edit = p ?? SelectedAddress;

            if (!_UserDialog.Edit(address_to_edit))
                return;
            if (!_UserDialog.ConfirmWarning($"Сохранить изменения в {address_to_edit.Address.Street} д.{address_to_edit.Address.House} кв.{address_to_edit.Address.ApartNum}?", "Сохранение изменений"))
                return;

            _AddressesRepository.Update(address_to_edit.Address);
            if(address_to_edit.Address.Street != null) _StreetsRepository.Update(address_to_edit.Address.Street);
            AddressesView.Refresh();
            SelectedAddress = address_to_edit;

            _AddressesViewSource.View.Refresh();
        }
        #endregion

        #region Command ExportDataToCSVFileCommand : Сохранение данных в CSV файл
        /// <summary>Сохранение данных в CSV файл</summary>
        private ICommand _ExportDataToCSVFileCommand;

        /// <summary>Сохранение данных в CSV файл</summary>
        public ICommand ExportDataToCSVFileCommand => _ExportDataToCSVFileCommand
            ??= new LambdaCommand(OnExportDataToCSVFileCommandExecuted, CanExportDataToCSVFileCommandExecute);

        /// <summary>Проверка возможности выполнения - Сохранение данных в CSV файл</summary>
        private bool CanExportDataToCSVFileCommandExecute() => true;

        /// <summary>Логика выполнения - Сохранение данных в CSV файл</summary>
        private async void OnExportDataToCSVFileCommandExecuted()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.InitialDirectory = "D:";
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string folder = dialog.SelectedPath;
                if (!_UserDialog.ConfirmWarning($"Сохранить в папку {folder}?", "Сохранение"))
                    return;

                string CSVPath = folder + $"/report_AddressesModel_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

                using (var sw = new StreamWriter(CSVPath, true, Encoding.UTF8))
                {

                    string headerLine = String.Join(",", "Фамилия", "Имя", "Отчество", "Улица", "Дом", "Квартира", "тел.домашний", "тел.рабочий", "тел.мобильный");

                    sw.Write($"{headerLine}{Environment.NewLine}");

                    List<AddressAbonent> AddressesList = AddressesView.OfType<AddressAbonent>().ToList();

                    foreach (var abonent in AddressesList)
                    {
                        if (abonent.Abonent == null) continue;
                        string csv = string.Join(",",
                            abonent.Abonent.SurName,
                            abonent.Abonent.Name,
                            abonent.Abonent.SecondName,
                            abonent?.Abonent?.Address?.Street?.Name,
                            abonent?.Abonent?.Address?.House,
                            abonent?.Abonent?.Address?.ApartNum,
                            abonent?.Abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.home)?.Number,
                            abonent?.Abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.work)?.Number,
                            abonent?.Abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.mobile)?.Number
                            );
                        sw.Write($"{csv}{Environment.NewLine}");
                    }
                }
            }
            else return;
        }
        #endregion

        public AddressesViewModel(
            IRepository<Address> AddressesRepository, 
            IUserDialog UserDialog,
            IRepository<Abonent> AbonentsRepository,
            IRepository<Street> StreetsRepository)
        {
            _AddressesRepository = AddressesRepository;
            _UserDialog = UserDialog;
            _AbonentsRepository = AbonentsRepository;
            _StreetsRepository = StreetsRepository;
        }

        private void OnAddressesFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is AddressAbonent address) || string.IsNullOrEmpty(AddressFilter)) return;

            if (!string.Concat(address.Address.Street.Name, address.Address.House, address.Address.ApartNum).Contains(AddressFilter))
                E.Accepted = false;
            //if (!address.Address.Street.Name.Contains(AddressFilter))
            //    E.Accepted = false;
            //if (!address.Address.House.ToString().Contains(AddressFilter))
            //    E.Accepted = false;
            //if (!address.Address.ApartNum.ToString().Contains(AddressFilter))
            //    E.Accepted = false;
        }
    }
}
