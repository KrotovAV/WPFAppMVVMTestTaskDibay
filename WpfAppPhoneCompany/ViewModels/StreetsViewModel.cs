using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfAppPhoneCompany.Services.Interfaces;
using WpfAppPhoneCompany.Models;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System;

namespace WpfAppPhoneCompany.ViewModels
{
    class StreetsViewModel : ViewModel
    {
        private readonly IRepository<Street> _StreetsRepository;
        private readonly IUserDialog _UserDialog;

        private readonly IRepository<Abonent> _AbonentsRepository;
        private readonly IRepository<Address> _AddressesRepository;

        #region Streets : ObservableCollection<StreetAbonents> - Коллекция улиц
        /// <summary>Коллекция улиц</summary>
        private ObservableCollection<StreetAbonents> _Streets;

        /// <summary>Коллекция улиц</summary>
        public ObservableCollection<StreetAbonents> Streets
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
                            new SortDescription(nameof(StreetAbonents.Street.Name), ListSortDirection.Ascending)
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

        #region Поиск
        /// <summary> Искомое слово абонент</summary>
        private string _AbonentFilter;
        /// <summary> Искомое слово абонент</summary>
        public string AbonentFilter
        {
            get => _AbonentFilter;
            set
            {
                if (Set(ref _AbonentFilter, value))
                    _AbonentsViewSource.View.Refresh(); 
            }
        }
        #endregion
        public ICollectionView AbonentsView => _AbonentsViewSource?.View;

        private CollectionViewSource _AbonentsViewSource;
        
        #region SelectedStreet: SelectedStreet - Выбранная улица
        /// <summary>Выбранная улица</summary>
        private StreetAbonents _SelectedStreet;

        /// <summary>Выбранная улица</summary>
        public StreetAbonents SelectedStreet
        {
            get => _SelectedStreet;
            set
            {
                if (Set(ref _SelectedStreet, value))
                {
                    _AbonentsViewSource = new CollectionViewSource
                    {
                        Source = value.AbonentsOfStreet,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Abonent.Name), ListSortDirection.Ascending)
                        }
                    };

                    _AbonentsViewSource.Filter += OnAbonentsFilter;
                    _AbonentsViewSource.View.Refresh();

                    OnPropertyChanged(nameof(AbonentsView));
                }
            }
        }
        #endregion


        #region SelectedAbonent: SelectedAbonent - Выбранный абонент
        /// <summary>Выбранный абонент</summary>
        private Abonent _SelectedAbonent;

        /// <summary>Выбранный абонент</summary>
        public Abonent SelectedAbonent
        {
            get => _SelectedAbonent;
            set => Set(ref _SelectedAbonent, value);
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
            var OnlyStreets = await _StreetsRepository.Items.ToArrayAsync();
            var OnlyAbonents = await _AbonentsRepository.Items.ToArrayAsync();

            var Streets_Abonents_GroupJoin_query =
                OnlyStreets                      // первый набор
                .GroupJoin(
                    OnlyAbonents,               // второй набор
                    street => street.Id,            // свойство-селектор объекта из первого набора по которому будет идти группировка
                    abonent => abonent.StreetId,    // свойство-селектор объекта из второго набора
                    (street, abonents) => new StreetAbonents { Street = street, AbonentsOfStreet = abonents.ToList() }) // желаемый результат
                ;
            Streets = new ObservableCollection<StreetAbonents>(Streets_Abonents_GroupJoin_query.ToArray());
        }
        #endregion

        #region Command AddNewStreetCommand - Добавление новой книги
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
            var new_street = new StreetAbonents { Street = new Street(), AbonentsOfStreet = new List<Abonent>() };

            if (!_UserDialog.Edit(new_street))
                return;

            _StreetsRepository.Add(new_street.Street);

            _Streets.Add(new_street);
            SelectedStreet = new_street;
        }
        #endregion

        #region Command RemoveStreetCommand : Удаление указанной улицы
        /// <summary>Удаление указанной улицы</summary>
        private ICommand _RemoveStreetCommand;

        /// <summary>Удаление указанной улицы</summary>
        public ICommand RemoveStreetCommand => _RemoveStreetCommand
            ??= new LambdaCommand<StreetAbonents>(OnRemoveStreetCommandExecuted, CanRemoveStreetCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанной улицы</summary>
        private bool CanRemoveStreetCommandExecute(StreetAbonents p) => p != null || SelectedStreet != null;

        /// <summary>Логика выполнения - Удаление указанной улицы</summary>
        private void OnRemoveStreetCommandExecuted(StreetAbonents p)
        {
            var street_to_remove = p ?? SelectedStreet;
            if (!_UserDialog.ConfirmWarning($"Вы действительно хотите удалить улицу {street_to_remove.Street.Name}?", "Удаление улицы"))
                return;

            _StreetsRepository.Remove(street_to_remove.Street.Id);

            if (street_to_remove.AbonentsOfStreet != null)
            {
                foreach (var abonent in street_to_remove.AbonentsOfStreet)
                {
                    abonent.Street = null;
                    _AbonentsRepository.Update(abonent);
                }
            }
            _Streets.Remove(street_to_remove);
            if (ReferenceEquals(SelectedStreet, street_to_remove))
                SelectedStreet = null;
        }
        #endregion

        #region Command EditStreetCommand : Редактирование указанной улицы
        /// <summary>Редактирование указанной улицы</summary>
        private ICommand _EditStreetCommand;

        /// <summary>Редактирование указанной улицы</summary>
        public ICommand EditStreetCommand => _EditStreetCommand
            ??= new LambdaCommand<StreetAbonents>(OnEditStreetCommandExecuted, CanEditStreetCommandExecute);

        /// <summary>Проверка возможности выполнения - Редактирование указанной улицы</summary>
        private bool CanEditStreetCommandExecute(StreetAbonents p) => p != null || SelectedStreet != null;

        /// <summary>Логика выполнения - Редактирование указанной улицы</summary>
        private void OnEditStreetCommandExecuted(StreetAbonents p)
        {
            var street_to_edit = p ?? SelectedStreet;

            if (!_UserDialog.Edit(street_to_edit))
                return;
            if (!_UserDialog.ConfirmWarning($"Сохранить изменения в {street_to_edit.Street.Name}?", "Сохранение изменений"))
                return;

            _StreetsRepository.Update(street_to_edit.Street);
            if (street_to_edit.AbonentsOfStreet != null)
            {
                foreach (var abonent in street_to_edit.AbonentsOfStreet)
                {
                    _AbonentsRepository.Update(abonent);
                }
            }

            StreetsView.Refresh();
            SelectedStreet = street_to_edit;
            OnPropertyChanged(nameof(SelectedStreet));
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

                string CSVPath = folder + $"/report_StreetsModel_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

                using (var sw = new StreamWriter(CSVPath, true, Encoding.UTF8))
                {

                    string headerLine = String.Join(",", "Фамилия", "Имя", "Отчество", "Улица", "Дом", "Квартира", "тел.домашний", "тел.рабочий", "тел.мобильный");

                    sw.Write($"{headerLine}{Environment.NewLine}");

                    List<StreetAbonents> streetsList = StreetsView.OfType<StreetAbonents>().ToList();

                    foreach (var street in streetsList)
                    {
                        if (street.AbonentsOfStreet != null)
                        {
                            foreach (var abonent in street.AbonentsOfStreet)
                            {
                                string csv = string.Join(",",
                                    abonent.SurName,
                                    abonent.Name,
                                    abonent.SecondName,
                                    abonent?.Address?.Street?.Name,
                                    abonent?.Address?.House,
                                    abonent?.Address?.ApartNum,
                                    abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.home)?.Number,
                                    abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.work)?.Number,
                                    abonent?.Phones?.FirstOrDefault(x => x.TypePhone == TypePhone.mobile)?.Number
                                    );
                                sw.Write($"{csv}{Environment.NewLine}");
                            }
                        }
                    }
                }
            }
            else return;
        }
        #endregion
        public StreetsViewModel(
            IRepository<Street> StreetsRepository, 
            IUserDialog UserDialog,
            IRepository<Abonent> abonentsRepository,
            IRepository<Address> addressesRepository)
        {
            _StreetsRepository = StreetsRepository;
            _UserDialog = UserDialog;
            _AbonentsRepository = abonentsRepository;
            _AddressesRepository = addressesRepository;
        }
        private void OnStreetsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is StreetAbonents street) || string.IsNullOrEmpty(StreetFilter)) return;

            if (!street.Street.Name.Contains(StreetFilter))
                E.Accepted = false;
        }
        private void OnAbonentsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Abonent abonent) || string.IsNullOrEmpty(AbonentFilter)) return;

            if (!(string.Concat(abonent.SurName, abonent.Name, abonent.SecondName).Contains(AbonentFilter)))
                E.Accepted = false;
            //if (!abonent.Name.Contains(AbonentFilter))
            //    E.Accepted = false;
            //if (!abonent.SurName.Contains(AbonentFilter))
            //    E.Accepted = false;
            //if (!abonent.SecondName.Contains(AbonentFilter))
            //    E.Accepted = false;
        }
    }
}
