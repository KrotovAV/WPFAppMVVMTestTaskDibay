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
    class AddressRepository : DbRepository<Address>
    {
        public override IQueryable<Address> Items => base.Items.Include(item => item.Street);

        public AddressRepository(ApplicationContext db) : base(db) { }
    }
}
