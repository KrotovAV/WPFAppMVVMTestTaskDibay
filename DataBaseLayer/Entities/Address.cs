using DataBaseLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entities
{
    public class Address : Entity
    {
        //public int Id { get; set; }
        public int? StreetId { get; set; } // //внешний ключ
        public virtual Street? Street { get; set; } //навигационное свойство
        public int House { get; set; }
        public int ApartNum { get; set; }

        public override string ToString() => $"Адрес: ул. {Street}, дом. {House}, кв. {ApartNum}.";
    }
}
