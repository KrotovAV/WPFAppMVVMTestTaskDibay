using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using MathCore.WPF.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WpfAppPhoneCompany.Data
{
    class DbInitializer
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(ApplicationContext db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }
        public async Task InitializeAsync()
        {
            if (await _db.Streets.AnyAsync()) return;

            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация БД...");

            //_Logger.LogInformation("Удаление существующей БД...");
            //await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);
            //_Logger.LogInformation("Удаление существующей БД выполнено за {0} мс", timer.ElapsedMilliseconds);

            //_db.Database.EnsureCreated();

            //_Logger.LogInformation("Миграция БД...");
            //await _db.Database.MigrateAsync().ConfigureAwait(false);
            //_Logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);

            await InitializeStreets();
            await InitializeAddresses();
            await InitializePhones();
            await InitializeAbonents().ConfigureAwait(false);

            _Logger.LogInformation("Инициализация БД выполнена за {0} с", timer.Elapsed.TotalSeconds);
        }
        private const int _StreetsCount = 10;

        private Street[] _Streets;
        private async Task InitializeStreets()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (1) улиц...");

            _Streets = new Street[_StreetsCount];
            for (var i = 0; i < _StreetsCount; i++)
                _Streets[i] = new Street { Name = $"IM_Улица {i + 1}" };

            await _db.Streets.AddRangeAsync(_Streets);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (1) улиц выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int _AddressesCount = 20;
        private Address[] _Addresses;
        private async Task InitializeAddresses()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (2) адресов ...");

            var rnd = new Random();

            _Addresses = new Address[_AddressesCount];
            for (var i = 0; i < _AddressesCount; i++)
            {
                _Addresses[i] = new Address
                {
                    Street = rnd.NextItem(_Streets),
                    House = (i*2)+1 - i,
                    ApartNum = ((i+1) * 10) - (i+1) * 3
                };
            }
            await _db.Addresses.AddRangeAsync(_Addresses);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (2) адресов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int _PhonesCount = 50;

        private Phone[] _Phones;
        private async Task InitializePhones()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (3) телефонов...");

            _Phones = new Phone[_PhonesCount];
            var rnd = new Random();

            for (var i = 0; i < _PhonesCount; i++)
                _Phones[i] = new Phone
                {
                    Number = 123456700 + i,
                    TypePhone = (TypePhone)rnd.Next(0, 3)
                };

            await _db.Phones.AddRangeAsync(_Phones);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (3) телефонов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int _AbonentsCount = 20;
        private Abonent[] _Abonents;
        private async Task InitializeAbonents()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (4) абонентов ...");

            var rnd = new Random();

            List<Phone>[] listOfListPhones = new List<Phone>[_AbonentsCount];
            for (var i = 0; i < _AbonentsCount; i++)
            {
                var phonesOfAbonent = new List<Phone>();
                for (var j = 0; j < 3; j++)
                {
                    var phone = rnd.NextItem(_Phones);
                    if (!phonesOfAbonent.Any(x => x.TypePhone == phone.TypePhone)) 
                        phonesOfAbonent.Add(phone);
                }

                listOfListPhones[i] = phonesOfAbonent;
            }

            _Abonents = Enumerable.Range(1, _AbonentsCount)
               .Select(i => new Abonent
               {
                   Name = $"IM_Имя {i}",
                   SecondName = $"IM_Отчество {i}",
                   SurName = $"IM_Фамилия {i}",
                   Address = rnd.NextItem(_Addresses),
                   Phones = rnd.NextItem(listOfListPhones)
               })
               .ToArray();

            foreach (var abo in _Abonents)
            {
                abo.StreetId = abo.Address?.Street?.Id;
            }

            await _db.Abonents.AddRangeAsync(_Abonents);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (4) абонентов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }
    }
}
