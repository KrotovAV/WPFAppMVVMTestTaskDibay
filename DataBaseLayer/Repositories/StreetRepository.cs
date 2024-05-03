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
    public class StreetRepository : DbRepository<Street>
    {
        public override IQueryable<Street> Items => base.Items.Include("Abonents");

        public StreetRepository(ApplicationContext db) : base(db) { }
    }
}
