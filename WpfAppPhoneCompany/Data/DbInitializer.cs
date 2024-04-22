using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using Microsoft.EntityFrameworkCore;
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
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация БД...");

            //_Logger.LogInformation("Удаление существующей БД...");
            //await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);
            //_Logger.LogInformation("Удаление существующей БД выполнено за {0} мс", timer.ElapsedMilliseconds);

            //_db.Database.EnsureCreated();

            _Logger.LogInformation("Миграция БД...");
            await _db.Database.MigrateAsync().ConfigureAwait(false);
            _Logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);

            if (await _db.Streets.AnyAsync()) return;
            await InitializeStreets();
            await InitializeAddresses();
            await InitializePhones();
            await InitializeAbonents().ConfigureAwait(false); 
            
   

            _Logger.LogInformation("Инициализация БД выполнена за {0} с", timer.Elapsed.TotalSeconds);
        }
        private const int __StreetsCount = 10;

        private Street[] _Streets;
        private async Task InitializeStreets()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (1) улиц...");

            _Streets = new Street[__StreetsCount];
            for (var i = 0; i < __StreetsCount; i++)
                _Streets[i] = new Street { Name = $"Улица {i + 1}" };

            await _db.Streets.AddRangeAsync(_Streets);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (1) улиц выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int __AddressesCount = 10;
        private Address[] _Addresses;
        private async Task InitializeAddresses()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (2) адресов ...");

            var rnd = new Random();
            _Addresses = Enumerable.Range(1, __AddressesCount)
               .Select(i => new Address
               {
                   Street = rnd.NextItem(_Streets),
                   House = i * i - 1,
                   ApartNum = (i * 10) - i * 5
               })
               .ToArray();

            await _db.Addresses.AddRangeAsync(_Addresses);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (2) адресов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int __PhonesCount = 50;

        private Phone[] _Phones;
        private async Task InitializePhones()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (3) телефонов...");

            _Phones = new Phone[__PhonesCount];
            var rnd = new Random();

            for (var i = 0; i < __PhonesCount; i++)
                _Phones[i] = new Phone
                {
                    Number = 123456700 + i,
                    TypePhone = (TypePhone)rnd.Next(0, 3)
                };

            await _db.Phones.AddRangeAsync(_Phones);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (3) телефонов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int __AbonentsCount = 10;
        private Abonent[] _Abonents;
        private async Task InitializeAbonents()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация (4) абонентов ...");

            var rnd = new Random();
            var listPhones = new List<Phone>() { };
            for (var i = 0; i < 3; i++)
            {
                var phone = rnd.NextItem(_Phones);
                if (!listPhones.Any(x => x.TypePhone == phone.TypePhone)) listPhones.Add(phone);
            }
            _Abonents = Enumerable.Range(1, __AbonentsCount)
               .Select(i => new Abonent
               {
                   Name = $"Имя {i}",
                   SecondName = $"Отчество {i}",
                   SurName = $"Фамилия {i}",
                   Address = rnd.NextItem(_Addresses),
                   Phones = listPhones
               })
               .ToArray();

            await _db.Abonents.AddRangeAsync(_Abonents);
            await _db.SaveChangesAsync();

            _Logger.LogInformation("Инициализация (4) абонентов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

    }
}
