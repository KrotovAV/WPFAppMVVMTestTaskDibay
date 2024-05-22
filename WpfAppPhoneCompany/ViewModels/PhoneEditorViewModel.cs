using DataBaseLayer.Entities;
using DataBaseLayer.Repositories;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfAppPhoneCompany.Views;

namespace WpfAppPhoneCompany.ViewModels
{
    class PhoneEditorViewModel : ViewModel
    {
        public int StreetId { get; }
        private int _Number;
        public int Number { get => _Number; set => Set(ref _Number, value); }

        private TypePhone _TypeOfPhone;
        public TypePhone TypeOfPhone { get => _TypeOfPhone; set => Set(ref _TypeOfPhone, value); }

        public List<TypePhone> TypePhoneList { get; } = new List<TypePhone> { TypePhone.home, TypePhone.work, TypePhone.mobile };

        private string _StatusOfPhone;
        public string StatusOfPhone { get => _StatusOfPhone; set => Set(ref _StatusOfPhone, value); }

        private int? _AbonentId;
        public int? AbonentId { get => _AbonentId; set => Set(ref _AbonentId, value); }

        private readonly IRepository<Abonent>_AbonentsRepository;



        #region SelectedTypePhone : TypePhone - Выбранный тип номера телефона
        /// <summary>Выбранный тип номера телефона</summary>
        private TypePhone _SelectedTypePhone;

        /// <summary>Выбранный тип номера телефона</summary>
        public TypePhone SelectedTypePhone
        {
            get => _SelectedTypePhone;
            set => Set(ref _SelectedTypePhone, value);
        }
        #endregion


        #region ChooseSelectedTypePhoneCommand Команда присвоения телефону выбранного типа номера
        /// <summary>Команда присвоения телефону выбранного типа номера</summary>
        private ICommand _ChooseSelectedTypePhoneCommand;

        /// <summary>Команда присвоения телефону выбранного типа номера</summary>
        public ICommand ChooseSelectedTypePhoneCommand => _ChooseSelectedTypePhoneCommand
            ??= new LambdaCommandAsync<TypePhone>(OnChooseSelectedTypePhoneCommandExecuted, CanChooseSelectedTypePhoneCommandExecute);

        /// <summary>Проверка возможности выполнения - Команды присвоения телефону выбранного типа номера</summary>
        private bool CanChooseSelectedTypePhoneCommandExecute(TypePhone p) => true;

        /// <summary>Логика выполнения - Команды присвоения телефону выбранного типа номера</summary>
        private async Task OnChooseSelectedTypePhoneCommandExecuted(TypePhone p)
        {
            TypeOfPhone = p;
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
            StatusOfPhone = "Телефон не закреплён за абонентом."; 

            if (AbonentId != null)
            {
                //var abonent = _AbonentsRepository.Items.FirstOrDefault(x => x.Id == AbonentId);

                var abonent = await _AbonentsRepository.GetAsync((int)AbonentId);
                if (abonent != null)
                {
                    StatusOfPhone = "Телефон закреплён за абонентом: " + abonent.ToString();
                }
            }
        }
        #endregion

        //public PhoneEditorViewModel()
        //    : this(new Phone { Id = 1, Number = 123, TypePhone = TypePhone.home, AbonentId = 0 })
        //{
        //    if (!App.IsDesignTime)
        //        throw new InvalidOperationException("Не для рантайма");
        //}

        public PhoneEditorViewModel(Phone phone, IRepository<Abonent> AbonentsRepository)
        {
            _AbonentsRepository = AbonentsRepository;

            StreetId = phone.Id;
            Number = phone.Number;
            TypeOfPhone = phone.TypePhone;
            AbonentId = phone.AbonentId;
        }
    }
}
