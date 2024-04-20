using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Repositories
{
    
    class PhoneRepository : DbRepository<Phone>
    {
        public override IQueryable<Phone> Items => base.Items.Include(item => item.Abonent);

        public PhoneRepository(ApplicationContext db) : base(db) { }
    }
}
