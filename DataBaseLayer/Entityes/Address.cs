﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entityes
{
    public class Address
    {
        public int Id { get; set; }
        public int? StreetId { get; set; } // //внешний ключ
        public virtual Street? Street { get; set; } //навигацинное свойство
        public int House { get; set; }
        public int ApartNum { get; set; }

    }
}
