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
    class AbonentsViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _AbonentsRepository;
        private readonly IUserDialog _UserDialog;
        private readonly IRepository<Phone> _PhonesRepository;
        private readonly IRepository<Street> _StreetsRepository;


        #region Abonents : ObservableCollection<Abonent> - Коллекция абонентов

        /// <summary>Коллекция абонентов</summary>
        private ObservableCollection<Abonent> _Abonents;

        /// <summary>Коллекция абонентов</summary>
        public ObservableCollection<Abonent> Abonents
        {
            get => _Abonents;
            set
            {
                if (Set(ref _Abonents, value))
                {
                    _AbonentsViewSource = new CollectionViewSource
                    {
                        Source = value,
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

        #region Поиск
        /// <summary> Искомое слово </summary>
        private string _AbonentFilter;
        /// <summary> Искомое слово </summary>
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


        private CollectionViewSource _AbonentsViewSource;

        public ICollectionView AbonentsView => _AbonentsViewSource?.View;


        #region SelectedAbonent : SelectedAbonent - Выбранный абонент

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
            Abonents = new ObservableCollection<Abonent>(await _AbonentsRepository.Items.ToArrayAsync());
        }
        #endregion

        #region Command AddNewAbonentCommand - Добавление нового абонента

        /// <summary>Добавление нового абонента</summary>
        private ICommand _AddNewAbonentCommand;

        /// <summary>Добавление нового абонента</summary>
        public ICommand AddNewAbonentCommand => _AddNewAbonentCommand
            ??= new LambdaCommand(OnAddNewAbonentCommandExecuted, CanAddNewAbonentCommandExecute);

        /// <summary>Проверка возможности выполнения - Добавление нового абонента</summary>
        private bool CanAddNewAbonentCommandExecute() => true;

        /// <summary>Логика выполнения - Добавление нового абонента</summary>
        private void OnAddNewAbonentCommandExecuted()
        {
            var new_abonent = new Abonent();

            if (!_UserDialog.Edit(new_abonent))
                return;

            _Abonents.Add(_AbonentsRepository.Add(new_abonent));

            SelectedAbonent = new_abonent;
        }
        #endregion


        #region Command RemoveAbonentCommand : Удаление указанного абонента

        /// <summary>Удаление указанного абонента</summary>
        private ICommand _RemoveAbonentCommand;

        /// <summary>Удаление указанного абонента</summary>
        public ICommand RemoveAbonentCommand => _RemoveAbonentCommand
            ??= new LambdaCommand<Abonent>(OnRemoveAbonentCommandExecuted, CanRemoveAbonentCommandExecute);

        /// <summary>Проверка возможности выполнения - Удаление указанного абонента</summary>
        private bool CanRemoveAbonentCommandExecute(Abonent p) => p != null || SelectedAbonent != null;

        /// <summary>Логика выполнения - Удаление указанного абонента</summary>
        private void OnRemoveAbonentCommandExecuted(Abonent p)
        {
            var abonent_to_remove = p ?? SelectedAbonent;
                        
            if (!_UserDialog.ConfirmWarning($"Вы действительно хотите удалить абонента {abonent_to_remove.Name}?", "Удаление абонента"))
                return;

            _AbonentsRepository.Remove(abonent_to_remove.Id);

            _Abonents.Remove(abonent_to_remove);
            if (ReferenceEquals(SelectedAbonent, abonent_to_remove))
                SelectedAbonent = null;
        }
        #endregion

        #region Command EditAbonentCommand : Редактирование указанного абонента

        /// <summary>Редактирование указанного абонента</summary>
        private ICommand _EditAbonentCommand;

        /// <summary>Редактирование указанного абонента</summary>
        public ICommand EditAbonentCommand => _EditAbonentCommand
            ??= new LambdaCommand<Abonent>(OnEditAbonentCommandExecuted, CanEditAbonentCommandExecute);

        /// <summary>Проверка возможности выполнения - Редактирование указанного абонента</summary>
        private bool CanEditAbonentCommandExecute(Abonent p) => p != null || SelectedAbonent != null;

        /// <summary>Логика выполнения - Редактирование указанного абонента</summary>
        private void OnEditAbonentCommandExecuted(Abonent p)
        {
            var abonent_to_edit = p ?? SelectedAbonent;

            if (!_UserDialog.Edit(abonent_to_edit))
                return;
            if (!_UserDialog.ConfirmWarning($"Сохранить изменения в\n {abonent_to_edit.SurName}\n {abonent_to_edit.Name}\n {abonent_to_edit.SecondName} ?", "Сохранение изменений"))
                return;
            if (abonent_to_edit.Phones != null)
            {
                foreach(var phone in abonent_to_edit.Phones)
                {
                    _PhonesRepository.Update(phone);
                }
            }

            if(abonent_to_edit.Street != null)
            {
                _StreetsRepository.Update(abonent_to_edit.Street);
            }
            
            _AbonentsRepository.Update(abonent_to_edit);
            AbonentsView.Refresh();
            SelectedAbonent = abonent_to_edit;

        }
        #endregion





        public AbonentsViewModel(IRepository<Abonent> AbonentsRepository, 
            IUserDialog UserDialog,
            IRepository<Phone> PhonesRepository,
            IRepository<Street> StreetsRepository)
        {
            _AbonentsRepository = AbonentsRepository;
            _UserDialog = UserDialog;
            _PhonesRepository = PhonesRepository;
            _StreetsRepository = StreetsRepository;
        }

        private void OnAbonentsFilter(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Abonent abonent) || string.IsNullOrEmpty(AbonentFilter)) return;

            if (!abonent.Name.Contains(AbonentFilter))
                E.Accepted = false;
        }


    }

}
