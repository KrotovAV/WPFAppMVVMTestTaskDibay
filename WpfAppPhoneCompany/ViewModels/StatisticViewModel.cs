using DataBaseLayer.Entities;
using DataInterfacesLayer.Interfaces;
using MathCore.ViewModels;
using MathCore.WPF.Commands;
using MathCore.WPF.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppPhoneCompany.Models;

namespace WpfAppPhoneCompany.ViewModels
{
    class StatisticViewModel : ViewModel
    {
        private readonly IRepository<Abonent> _Abonents;
        private readonly IRepository<Address> _Addresses;
        private readonly IRepository<Street> _Streets;
        private readonly IRepository<Phone> _Phones;

        //private int _AbonentsCount;
        //public int AbonentsCount { get => _AbonentsCount; private set => Set(ref _AbonentsCount, value); }

        public ObservableCollection<BestStreetsInfo> StatStreetAbonents { get; } = new ObservableCollection<BestStreetsInfo>();
        

        #region Command ComputeStatisticCommand - Вычислить статистические данные
        private ICommand _ComputeStatisticCommand;

        public ICommand ComputeStatisticCommand => _ComputeStatisticCommand
            ??= new LambdaCommandAsync(OnComputeStatisticCommandExecuted, CanComputeStatisticCommandExecute);
        private bool CanComputeStatisticCommandExecute() => true;


        private async Task OnComputeStatisticCommandExecuted()
        {
            await ComputeStatisticAsync();
        }

        private async Task ComputeStatisticAsync()
        {
            var bestStreet_query = _Abonents.Items
               .GroupBy(u => u.Street)
               .Select(group => new BestStreetsInfo{ Street = group.Key, AbonentsCount = group.Count() })
               .OrderByDescending(u => u.AbonentsCount)
               .Take(5)
            ;

            StatStreetAbonents.Clear();
            foreach(var bestStreet in await bestStreet_query.ToArrayAsync())
            {
                StatStreetAbonents.Add(bestStreet);
            }
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

