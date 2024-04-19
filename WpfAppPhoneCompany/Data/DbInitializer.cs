using DataBaseLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //if (await _db.Books.AnyAsync()) return;

            //await InitializeCategories();
            //await InitializeBooks();
            //await InitializeSellers();
            //await InitializeBuyers();
            //await InitializeDeals();

            _Logger.LogInformation("Инициализация БД выполнена за {0} с", timer.Elapsed.TotalSeconds);
        }

    }
}
