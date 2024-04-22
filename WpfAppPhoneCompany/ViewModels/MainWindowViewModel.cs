using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using PresentationLayer.Services;
using PresentationLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace WpfAppPhoneCompany.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Street> _Streets;
        private readonly IRepository<Phone> _Phones;
        private readonly IConnectAbonentService _ConnectAbonentService;


        #region Title : string - Заголовок

        /// <summary>Заголовок</summary>
        private string _Title = "Главное окно программы - PhoneCompany";

        /// <summary>Заголовок</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }
        #endregion
       
        
        
        #region CurrentModel : BaseViewModel - Текущая дочерняя модель-представления

        /// <summary>Текущая дочерняя модель-представления</summary>
        private ViewModel _CurrentModel;

        /// <summary>Текущая дочерняя модель-представления</summary>
        public ViewModel CurrentModel { get => _CurrentModel; private set => Set(ref _CurrentModel, value); }

        #endregion


        #region Command ShowStreesViewCommand - Отобразить представление улиц

        /// <summary>Отобразить представление улиц</summary>
        private ICommand _ShowStreetsViewCommand;

        /// <summary>Отобразить представление улиц</summary>
        public ICommand ShowStreetsViewCommand => _ShowStreetsViewCommand
            ??= new LambdaCommand(OnShowStreetsViewCommandExecuted, CanShowStreetsViewCommandExecute);

        /// <summary>Проверка возможности выполнения - Отобразить представление улиц</summary>
        private bool CanShowStreetsViewCommandExecute() => true;

        /// <summary>Логика выполнения - Отобразить представление улиц</summary>
        private void OnShowStreetsViewCommandExecuted()
        {
            CurrentModel = new StreetsViewModel(_Streets /*, _UserDialog*/);
        }

        #endregion

        #region Command ShowBooksViewCommand - Отобразить представление адресов

        /// <summary>Отобразить представление адресов</summary>
        private ICommand _ShowAddressesViewCommand;

        /// <summary>Отобразить представление адресов</summary>
        public ICommand ShowAddressesViewCommand => _ShowAddressesViewCommand
            ??= new LambdaCommand(OnShowAddressesViewCommandExecuted, CanShowAddressesViewCommandExecute);

        /// <summary>Проверка возможности выполнения - Отобразить представление адресов</summary>
        private bool CanShowAddressesViewCommandExecute() => true;

        /// <summary>Логика выполнения - Отобразить представление адресов</summary>
        private void OnShowAddressesViewCommandExecuted()
        {
            CurrentModel = new AddressesViewModel(_Addresses /*, _UserDialog*/);
        }

        #endregion



        #region Command ShowPhonesViewCommand - Отобразить представление номеров телефонов

        /// <summary>Отобразить представление номеров телефонов</summary>
        private ICommand _ShowPhonesViewCommand;

        /// <summary>Отобразить представление номеров телефонов</summary>
        public ICommand ShowPhonesViewCommand => _ShowPhonesViewCommand
            ??= new LambdaCommand(OnShowPhonesViewCommandExecuted, CanShowPhonesViewCommandExecute);

        /// <summary>Проверка возможности выполнения - Отобразить представление Номеров телефонов</summary>
        private bool CanShowPhonesViewCommandExecute() => true;

        /// <summary>Логика выполнения - Отобразить представление номеров телефонов</summary>
        private void OnShowPhonesViewCommandExecuted()
        {
            CurrentModel = new PhonesViewModel(_Phones/*, _UserDialog*/);
        }

        #endregion

        #region Command ShowStatisticViewCommand - Отобразить представление статистики

        /// <summary>Отобразить представление статистики</summary>
        private ICommand _ShowStatisticViewCommand;

        /// <summary>Отобразить представление статистики</summary>
        public ICommand ShowStatisticViewCommand => _ShowStatisticViewCommand
            ??= new LambdaCommand(OnShowStatisticViewCommandExecuted, CanShowStatisticViewCommandExecute);

        /// <summary>Проверка возможности выполнения - Отобразить представление статистики</summary>
        private bool CanShowStatisticViewCommandExecute() => true;

        /// <summary>Логика выполнения - Отобразить представление статистики</summary>
        private void OnShowStatisticViewCommandExecuted()
        {
            CurrentModel = new StatisticViewModel(_Abonents,_Addresses,_Phones, _Streets);
        }

        #endregion






        public MainWindowViewModel(
            IRepository<Abonent> Abonents,
            IRepository<Address> Addresses,
            IRepository<Phone> Phones,
            IRepository<Street> Streets,
            IConnectAbonentService ConnectAbonentService)
        {
            _Abonents = Abonents;
            _Addresses = Addresses;
            _Phones = Phones;
            _Streets = Streets;
            _ConnectAbonentService = ConnectAbonentService;


            //Test();
           
        }

        //private async void Test()
        //{
        //    var abonents_count = _ConnectAbonentService.Abonents.Count();
        //    var abonent = await _AbonentRepository.GetAsync(2);

        //    var address = await _AddressRepository.GetAsync(3);
        //    var phone = await _PhoneRepository.GetAsync(4);
        //    var newAbonent = await _ConnectAbonentService.ConnectAbonentAsync("Semin", "Semion", "Semionovich", address, phone);
        //    var abonents_count2 = _ConnectAbonentService.Abonents.Count();

        //}

    }
}
