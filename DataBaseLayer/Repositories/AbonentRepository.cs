﻿using DataBaseLayer.Context;
using DataBaseLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Repositories
{
    public class AbonentRepository : DbRepository<Abonent>
    {
        public override IQueryable<Abonent> Items => base.Items
                                                        .Include(item => item.Address)
                                                        .Include(item => item.Street)
                                                        .Include("Phones");
        public AbonentRepository(ApplicationContext db) : base(db) { }
    }
}
