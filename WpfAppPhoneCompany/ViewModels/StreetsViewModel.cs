using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Reflection.Metadata.BlobBuilder;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using MathCore.WPF;
using WpfAppPhoneCompany.Services;
using WpfAppPhoneCompany.Services.Interfaces;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetsViewModel : ViewModel
    {
        private readonly IRepository<Street> _StreetsRepository;
        private readonly IUserDialog<Street> _UserStreetDialog;

        private readonly IRepository<Abonent> _AbonentsRepository;
        private readonly IRepository<Address> _AddressesRepository;

        #region Streets : ObservableCollection<Street> - Коллекция улиц

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


        
      

        #region Поиск
        /// <summary> Искомое слово </summary>
        private string _StreetFilter;
        /// <summary> Искомое слово </summary>
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

        //public IEnumerable<Street> Streets => _StreetsRepository.Items.ToArray();


        #region SelectedBook : SelectedStreet - Выбранная улица

        /// <summary>Выбранная улица</summary>
        private Street _SelectedStreet;

        /// <summary>Выбранная улица</summary>
        public Street SelectedStreet { 
            get => _SelectedStreet; 
            set => Set(ref _SelectedStreet, value); 
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
            //Streets = (await _StreetsRepository.Items.ToArrayAsync()).ToObservableCollection();
            Streets = new ObservableCollection<Street>(await _StreetsRepository.Items.ToArrayAsync());
        }


        #endregion

        #region Command AddNewBookCommand - Добавление новой книги

        /// <summary>Добавление новой улицы</summary>
        private ICommand _AddNewStreetCommand;

        /// <summary>Добавление новой улицы</summary>
        public ICommand AddNewStreetCommand => _AddNewStreetCommand
            ??= new LambdaCommand(OnAddNewStreetCommandExecuted, CanAddNewStreetCommandExecute);

        /// <summary>Проверка возможности выполнения - Добавление новой улицы</summary>
        private bool CanAddNewStreetCommandExecute() => true;

        /// <summary>Логика выполнения - Добавление новой улицы</summary>
        private void OnAddNewStreetCommandExecuted()
        {
            var new_street = new Street();

            if (!_UserStreetDialog.Edit(new_street))
                return;

            _Streets.Add(_StreetsRepository.Add(new_street));

            SelectedStreet = new_street;
        }

        #endregion


        #region Command RemoveStreetCommand : Удаление указанной книги

        /// <summary>Удаление указанной улицы</summary>
        private ICommand _RemoveStreetCommand;

        /// <summary>Удаление указанной улицы</summary>
        public ICommand RemoveStreetCommand => _RemoveStreetCommand
            ??= new LambdaCommand<Street>(OnRemoveStreetCommandExecuted, CanRemoveStreetCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанной улицы</summary>
        private bool CanRemoveStreetCommandExecute(Street p) => p != null || SelectedStreet != null;

        /// <summary>Проверка возможности выполнения - Удаление указанной улицы</summary>
        private void OnRemoveStreetCommandExecuted(Street p)
        {
            var street_to_remove = p ?? SelectedStreet;

            if (!_UserStreetDialog.ConfirmWarning($"Вы действительно хотите удалить улицу {street_to_remove.Name}?", "Удаление улицы"))
                return;

            _StreetsRepository.Remove(street_to_remove.Id);

            Streets.Remove(street_to_remove);
            if (ReferenceEquals(SelectedStreet, street_to_remove))
                SelectedStreet = null;
        }

        #endregion



        public StreetsViewModel(
            IRepository<Street> StreetsRepository, 
            IUserDialog<Street> UserStreetDialog, 
            IRepository<Abonent> abonentsRepository,
            IRepository<Address> addressesRepository)
        {
            _StreetsRepository = StreetsRepository;
            _UserStreetDialog = UserStreetDialog;
            _AbonentsRepository = abonentsRepository;
            _AddressesRepository = addressesRepository;
        }
        private void OnStreetsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Street street) || string.IsNullOrEmpty(StreetFilter)) return;

            if (!street.Name.Contains(StreetFilter))
                E.Accepted = false;
        }
    }
}
