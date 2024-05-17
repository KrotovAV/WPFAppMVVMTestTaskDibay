using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WpfAppPhoneCompany.Models;

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



        private readonly IRepository<Street> _StreetsRepository;



        #region Streets : ObservableCollection<StreetAbonents> - Коллекция улиц

        /// <summary>Коллекция улиц</summary>
        private ObservableCollection<Street> _Streets;

        /// <summary>Коллекция улиц</summary>
        public ObservableCollection<Street> Streets
        {
            get => _Streets;
            set
            {
                if (Set(ref _Streets, value))
                {
                    _StreetsViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Street.Name), ListSortDirection.Ascending)
                        }
                    };

                    _StreetsViewSource.Filter += OnStreetsFilter;
                    _StreetsViewSource.View.Refresh();

                    OnPropertyChanged(nameof(StreetsView));
                }
            }
        }
        #endregion

        #region SelectedStreet : Street - Выбранная улица

        /// <summary>Выбранная улица</summary>
        private Street _SelectedStreet;

        /// <summary>Выбранная улица</summary>
        public Street SelectedStreet
        {
            get => _SelectedStreet;
            set => Set(ref _SelectedStreet, value);
        }
        #endregion

        #region Поиск
        /// <summary> Искомое слово улица</summary>
        private string _StreetFilter;
        /// <summary> Искомое слово улица</summary>
        public string StreetFilter
        {
            get => _StreetFilter;
            set
            {
                if (Set(ref _StreetFilter, value))
                    _StreetsViewSource.View.Refresh();
            }
        }
        #endregion

        private CollectionViewSource _StreetsViewSource;
        public ICollectionView StreetsView => _StreetsViewSource?.View;


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
            Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }
        #endregion

        #region ChooseSelectedStreetCommand комманда присвоения адресу выбранной улицы

        /// <summary>Команда присвоения адресу выбранной улицы</summary>
        private ICommand _ChooseSelectedStreetCommand;

        /// <summary>Команда присвоения адресу выбранной улицы</summary>
        public ICommand ChooseSelectedStreetCommand => _ChooseSelectedStreetCommand
            ??= new LambdaCommandAsync<Street>(OnChooseSelectedStreetCommandExecuted, CanChooseSelectedStreetCommandExecute);

        /// <summary>Проверка возможности выполнения - Команды присвоения адресу выбранной улицы</summary>
        private bool CanChooseSelectedStreetCommandExecute(Street p) => p != null || SelectedStreet != null;

        /// <summary>Логика выполнения - Команды присвоения адресу выбранной улицы</summary>
        private async Task OnChooseSelectedStreetCommandExecuted(Street p)
        {
            //var address_to_edit = p ?? SelectedAddress;
            StreetId = p.Id;
           
            //Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }
        #endregion

        //public AddressEditorViewModel()
        //    : this(new Address { Id = 1, StreetId = 1, House = 1, ApartNum = 1 })
        //{
        //    if (!App.IsDesignTime)
        //        throw new InvalidOperationException("Не для рантайма");
        //}

        public AddressEditorViewModel(Address address,
            IRepository<Street> StreetsRepository)
        {
            _StreetsRepository = StreetsRepository;

            StreetId = address.StreetId ?? 1;// исправить костыль
            House = address.House;
            ApartNum = address.ApartNum;
        }

        private void OnStreetsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Street street) || string.IsNullOrEmpty(StreetFilter)) return;

            if (!street.Name.Contains(StreetFilter))
                E.Accepted = false;
        }
    }
}
