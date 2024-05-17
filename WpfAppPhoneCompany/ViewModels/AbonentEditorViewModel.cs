using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WpfAppPhoneCompany.Models;

namespace WpfAppPhoneCompany.ViewModels
{
    internal class AbonentEditorViewModel : ViewModel
    {

        public int AbonentId { get; }
    
        private string _SurName;
        public string SurName { get => _SurName; set => Set(ref _SurName, value); }

        private string _Name;
        public string Name { get => _Name; set => Set(ref _Name, value); }

        private string _SecondName;
        public string SecondName { get => _SecondName; set => Set(ref _SecondName, value); }

        private int? _AddressId;
        public int? AddressId { get => _AddressId; set => Set(ref _AddressId, value); }

        private List<Phone>? _PhonesOfAbonent;
        public List<Phone>? PhonesOfAbonent { get => _PhonesOfAbonent; set => Set(ref _PhonesOfAbonent, value); }

        private string _FullAddressOfAbonent;
        public string FullAddressOfAbonent { get => _FullAddressOfAbonent; set => Set(ref _FullAddressOfAbonent, value); }


        private readonly IRepository<Address> _AddressesRepository;
        private readonly IRepository<Phone> _PhonesRepository;

        
        #region Addresses : ObservableCollection<Addresses> - Коллекция адресов

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

        #region Phones : ObservableCollection<Phones> - Коллекция телефонов

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

        #region SelectedPhone : Phone - Выбранный телефон

        /// <summary>Выбранный телефон</summary>
        private Phone _SelectedPhone;

        /// <summary>Выбранный телефон</summary>
        public Phone SelectedPhone
        {
            get => _SelectedPhone;
            set => Set(ref _SelectedPhone, value);
        }
        #endregion

        #region Поиск
        /// <summary> Искомое слово телефон</summary>
        private string _AddressFilter;
        /// <summary> Искомое слово телефон</summary>
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

        #region Поиск
        /// <summary> Искомое слово телефон</summary>
        private string _PhoneFilter;
        /// <summary> Искомое слово телефон</summary>
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

        private CollectionViewSource _AddressesViewSource;
        public ICollectionView AddressesView => _AddressesViewSource?.View;

        private CollectionViewSource _PhonesViewSource;
        public ICollectionView PhonesView => _PhonesViewSource?.View;


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
            Phones = new ObservableCollection<Phone>(await _PhonesRepository.Items.ToArrayAsync());

            FullAddressOfAbonent = _Addresses.FirstOrDefault(x => x.Id == AddressId).ToString();
            PhonesOfAbonent = _Phones.Where(x => x.AbonentId == AbonentId).ToList();
        }
        #endregion


        #region RemoveSelectedPhoneCommand команда добавления абоненту выбранного телефона

        /// <summary>Команда удаления абоненту выбранного телефона</summary>
        private ICommand _RemoveSelectedPhoneFromAbonentCommand;

        /// <summary>Команда удаления абоненту выбранного телефона</summary>
        public ICommand RemoveSelectedPhoneFromAbonentCommand => _RemoveSelectedPhoneFromAbonentCommand
            ??= new LambdaCommandAsync<Phone>(OnRemoveSelectedPhoneFromAbonentCommandExecuted, CanRemoveSelectedPhoneFromAbonentCommandExecute);

        /// <summary>Проверка возможности выполнения - Команда удаления абоненту выбранного телефона</summary>
        private bool CanRemoveSelectedPhoneFromAbonentCommandExecute(Phone p) => p != null || SelectedPhone != null || PhonesOfAbonent != null;

        /// <summary>Логика выполнения - Команда удаления абоненту выбранного телефона</summary>
        private async Task OnRemoveSelectedPhoneFromAbonentCommandExecuted(Phone p)
        {
            //var abonent_to_edit = p ?? SelectedAddress;
            if (PhonesOfAbonent != null)
            {
                PhonesOfAbonent.Remove(p);
            }
            //Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }
        #endregion


        #region AddSelectedPhoneCommand команда добавления абоненту выбранного телефона

        /// <summary>Команда добавления абоненту выбранного телефона</summary>
        private ICommand _AddSelectedPhoneToAbonentCommand;

        /// <summary>Команда добавления абоненту выбранного телефона</summary>
        public ICommand AddSelectedPhoneToAbonentCommand => _AddSelectedPhoneToAbonentCommand
            ??= new LambdaCommandAsync<Phone>(OnAddSelectedPhoneToAbonentCommandExecuted, CanAddSelectedPhoneToAbonentCommandExecute);

        /// <summary>Проверка возможности выполнения - Команда добавления абоненту выбранного телефона</summary>
        private bool CanAddSelectedPhoneToAbonentCommandExecute(Phone p) => p != null || SelectedPhone != null;

        /// <summary>Логика выполнения - Команда добавления абоненту выбранного телефона</summary>
        private async Task OnAddSelectedPhoneToAbonentCommandExecuted(Phone p)
        {
            //var address_to_edit = p ?? SelectedAddress;
            if (PhonesOfAbonent is null)
            {
                PhonesOfAbonent = new List<Phone> { };

            }
            PhonesOfAbonent.Add(p);

            //Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }
        #endregion


        #region ChooseSelectedAddressCommand Команда присвоения абоненту выбранного адреса

        /// <summary>Команда присвоения абоненту выбранного адреса</summary>
        private ICommand _ChooseSelectedAddressCommand;

        /// <summary>Команда присвоения абоненту выбранного адреса</summary>
        public ICommand ChooseSelectedAddressCommand => _ChooseSelectedAddressCommand
            ??= new LambdaCommandAsync<Address>(OnChooseSelectedAddressCommandExecuted, CanChooseSelectedAddressCommandExecute);

        /// <summary>Проверка возможности выполнения - Команды присвоения абоненту выбранного адреса</summary>
        private bool CanChooseSelectedAddressCommandExecute(Address p) => p != null || SelectedAddress != null;

        /// <summary>Логика выполнения - Команды присвоения абоненту выбранного адреса</summary>
        private async Task OnChooseSelectedAddressCommandExecuted(Address p)
        {
            //var address_to_edit = p ?? SelectedAddress;
            AddressId = p.Id;

            //Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }
        #endregion


        //public AbonentEditorViewModel()
        //    : this(new Abonent { Id = 1, Name = "Имя", SurName = "Фамилия", SecondName = "Отчество", AddressId = 1})
        //{
        //    if (!App.IsDesignTime)
        //        throw new InvalidOperationException("Не для рантайма");
        //}

        public AbonentEditorViewModel(Abonent abonent,
            IRepository<Address> AddressesRepository,
            IRepository<Phone> PhonesRepository
            )
        {
            _AddressesRepository = AddressesRepository;
            _PhonesRepository = PhonesRepository;
            

            AbonentId = abonent.Id;
            Name = abonent.Name;
            SurName = abonent.SurName;
            SecondName = abonent.SecondName;
            AddressId = abonent.AddressId;

            
        }

        private void OnAddressesFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Address address) || string.IsNullOrEmpty(AddressFilter)) return;

            //if (!address.Street.Name.Contains(AddressFilter))
            //    E.Accepted = false;
            if (!string.Concat(address.Street.Name, address.House, address.ApartNum).Contains(AddressFilter))
                E.Accepted = false;
        }
        private void OnPhonesFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Phone phone) || string.IsNullOrEmpty(PhoneFilter)) return;

            if (!phone.Number.ToString().Contains(PhoneFilter))
                E.Accepted = false;
        }


    }
}
