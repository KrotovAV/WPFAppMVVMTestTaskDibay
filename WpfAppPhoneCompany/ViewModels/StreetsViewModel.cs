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

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetsViewModel : ViewModel
    {
        private readonly IRepository<Street> _StreetsRepository;

        #region Streets : ObservableCollection<Street> - Коллекция улиц

        /// <summary>Коллекция книг</summary>
        private ObservableCollection<Street> _Streets;

        /// <summary>Коллекция книг</summary>
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

        public ICollectionView StreetsView => _StreetsViewSource.View;

        //public IEnumerable<Street> Streets => _StreetsRepository.Items.ToArray();


        #region SelectedBook : Street - Выбранная улица

        /// <summary>Выбранная улица</summary>
        private Street _SelectedStreet;

        /// <summary>Выбранная улица</summary>
        public Street SelectedStreeet { get => _SelectedStreet; set => Set(ref _SelectedStreet, value); }

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
        public StreetsViewModel(IRepository<Street> StreetsRepository)
        {
            _StreetsRepository = StreetsRepository;

            //_StreetsViewSource = new CollectionViewSource
            //{
            //    Source = _StreetsRepository.Items.ToArray(),
            //    SortDescriptions = 
            //    {
            //        new SortDescription(nameof(Street.Name), ListSortDirection.Ascending)
            //    }
            //};
            //_StreetsViewSource.Filter += OnStreetsFilter;
        }
        private void OnStreetsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Street street) || string.IsNullOrEmpty(StreetFilter)) return;

            if (!street.Name.Contains(StreetFilter))
                E.Accepted = false;
        }
    }
}
