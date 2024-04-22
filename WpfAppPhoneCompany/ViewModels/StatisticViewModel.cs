using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppPhoneCompany.ViewModels
{
    class StatisticViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Street> _Streets;
        private readonly IRepository<Phone> _Phones;

        private int _AbonentsCount;
        public int AbonentsCount { get => _AbonentsCount; private set => Set(ref _AbonentsCount, value); }


        #region Command ComputeStatisticCommand - Вычислить статистические данные
        private ICommand _ComputeStatisticCommand;

        public ICommand ComputeStatisticCommand => _ComputeStatisticCommand
            ??= new LambdaCommandAsync(OnComputeStatisticCommandExecuted, CanComputeStatisticCommandExecute);
        private bool CanComputeStatisticCommandExecute() => true;


        private async Task OnComputeStatisticCommandExecuted()
        {
            AbonentsCount = await _Abonents.Items.CountAsync();
            //var abonents = _Abonents.Items;
            //abonents.GroupBy(abonent => abonent.Street)
            //    .Select(abon => new { Abonent => abon.Key, Count = abon.Count() })
            //    .OrderByDescending(street => street.Count)
            //    .Take(5)
            //    .ToArrayAsync();
        }






        #endregion
        public StatisticViewModel(
            IRepository<Abonent> Abonents,
            IRepository<Address> Addresses,
            IRepository<Street> Streets,
            IRepository<Phone> Phones
            )
        {
            _Abonents = Abonents;
            _Addresses = Addresses;
            _Phones = Phones;
            _Streets = Streets;
        }
    }
}

