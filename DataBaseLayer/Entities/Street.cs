﻿using DataBaseLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entities
{
    public class Street : Entity
    {
        //public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Abonent> Abonents { get; set; } = new HashSet<Abonent>();
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        public override string ToString() => $"{Name}";
    }
}
