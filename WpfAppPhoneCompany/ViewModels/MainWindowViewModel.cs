using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF;
using MathCore.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAppPhoneCompany.Services.Interfaces;

namespace WpfAppPhoneCompany.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Street> _Streets;
        private readonly IRepository<Phone> _Phones;
        private readonly IConnectAbonentService _ConnectAbonentService;
        private readonly IUserDialog _UserDialog;

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
            CurrentModel = new StreetsViewModel(_Streets, _UserDialog);
        }

        #endregion

        #region Command ShowAddressesViewCommand - Отобразить представление адресов

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
            CurrentModel = new AddressesViewModel(_Addresses , _UserDialog);
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
            CurrentModel = new PhonesViewModel(_Phones, _UserDialog);
        }

        #endregion


        #region Command ShowAbonentsViewCommand - Отобразить представление номеров телефонов

        /// <summary>Отобразить представление абонентов</summary>
        private ICommand _ShowAbonentsViewCommand;

        /// <summary>Отобразить представление абонентов</summary>
        public ICommand ShowAbonentsViewCommand => _ShowAbonentsViewCommand
            ??= new LambdaCommand(OnShowAbonentsViewCommandExecuted, CanShowAbonentsViewCommandExecute);

        /// <summary>Проверка возможности выполнения - Отобразить представление абонентов</summary>
        private bool CanShowAbonentsViewCommandExecute() => true;

        /// <summary>Логика выполнения - Отобразить представление абонентов</summary>
        private void OnShowAbonentsViewCommandExecuted()
        {
            CurrentModel = new AbonentsViewModel(_Abonents, _UserDialog);
        }

        #endregion


        #region Command ShowStatisticViewCommand - Отобразить представление статистики
        private ICommand _ShowStatisticViewCommand;
        public ICommand ShowStatisticViewCommand => _ShowStatisticViewCommand
            ??= new LambdaCommand(OnShowStatisticViewCommandExecuted, CanShowStatisticViewCommandExecute);
        private bool CanShowStatisticViewCommandExecute() => true;
        private void OnShowStatisticViewCommandExecuted()
        {
            CurrentModel = new StatisticViewModel(_Abonents,_Addresses, _Streets, _Phones);
        }
        #endregion


        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        public MainWindowViewModel(
            IRepository<Abonent> Abonents,
            IRepository<Address> Addresses,
            IRepository<Phone> Phones,
            IRepository<Street> Streets,
            IConnectAbonentService ConnectAbonentService,
            IUserDialog UserDialog)
        {
            _Abonents = Abonents;
            _Addresses = Addresses;
            _Phones = Phones;
            _Streets = Streets;
            _ConnectAbonentService = ConnectAbonentService;
            _UserDialog = UserDialog;

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

        }
    }
}
