using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathCore.ViewModels;
using WpfAppPhoneCompany.Models;
using DataInterfacesLayer.Interfaces;
using DataBaseLayer.Repositories;
using MathCore.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetEditorViewModel : ViewModel
    {

        //public int StreetId { get; }
        //private string _Name;
        //public string Name { get => _Name; set => Set(ref _Name, value); }

        private Street _Street;
        public Street Street { get =>_Street; set => Set(ref _Street, value); }

        private List<Abonent> _AbonentsOfStreet;
        public List<Abonent> AbonentsOfStreet { get => _AbonentsOfStreet; set => Set(ref _AbonentsOfStreet, value); }
        //private StreetAbonents streetAbonents;

        


        //private readonly IRepository<Abonent> _AbonentsRepository;

        //#region Abonents : ObservableCollection<Abonent> - Коллекция абонентов

        ///// <summary>Коллекция абонентов</summary>
        //private ObservableCollection<Abonent> _Abonents;

        ///// <summary>Коллекция абонентов</summary>
        //public ObservableCollection<Abonent> Abonents
        //{
        //    get => _Abonents;
        //    set
        //    {
        //        if (Set(ref _Abonents, value))
        //        {
        //            _AbonentsViewSource = new CollectionViewSource
        //            {
        //                Source = value,
        //                SortDescriptions =
        //                {
        //                    new SortDescription(nameof(Abonent.Name), ListSortDirection.Ascending)
        //                }
        //            };

        //            _AbonentsViewSource.Filter += OnAbonentsFilter;
        //            _AbonentsViewSource.View.Refresh();

        //            OnPropertyChanged(nameof(AbonentsView));
        //        }
        //    }
        //}
        //#endregion


        //#region Поиск
        ///// <summary> Искомое слово </summary>
        //private string _AbonentFilter;
        ///// <summary> Искомое слово </summary>
        //public string AbonentFilter
        //{
        //    get => _AbonentFilter;
        //    set
        //    {
        //        if (Set(ref _AbonentFilter, value))
        //            _AbonentsViewSource.View.Refresh();
        //    }
        //}
        //#endregion

        //private CollectionViewSource _AbonentsViewSource;

        //public ICollectionView AbonentsView => _AbonentsViewSource?.View;

        //#region SelectedAbonent : SelectedAbonent - Выбранный абонент

        ///// <summary>Выбранный абонент</summary>
        //private Abonent _SelectedAbonent;

        ///// <summary>Выбранный абонент</summary>
        //public Abonent SelectedAbonent
        //{
        //    get => _SelectedAbonent;
        //    set => Set(ref _SelectedAbonent, value);
        //}
        //#endregion


        public StreetEditorViewModel()
            : this(new StreetAbonents { Street = new Street() { Id = 1, Name = "Одуванчиковая!" } })
        {
            
            if (!App.IsDesignTime)
                throw new InvalidOperationException("Не для рантайма");
        }

        public StreetEditorViewModel(StreetAbonents street
            //,IRepository<Abonent> AbonentsRepository
            )
        {
            //_AbonentsRepository = AbonentsRepository;

            Street = street.Street;
            AbonentsOfStreet = street.AbonentsOfStreet ?? new List<Abonent>();
        }

        //public StreetEditorViewModel(StreetAbonents streetAbonents)
        //{
        //    this.streetAbonents = streetAbonents;
        //}

        //private void OnAbonentsFilter(object Sender, FilterEventArgs E)
        //{
        //    if (!(E.Item is Abonent abonent) || string.IsNullOrEmpty(AbonentFilter)) return;

        //    if (!abonent.Name.Contains(AbonentFilter))
        //        E.Accepted = false;
        //}
    }
}
