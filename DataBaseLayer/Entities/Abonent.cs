using DataBaseLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entities
{
    public class Abonent : Entity
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }

        public int? AddressId { get; set; } // внешний ключ
        public virtual Address? Address { get; set; }//навигационное свойство
        public int? StreetId { get; set; } // внешний ключ
        public virtual Street? Street { get; set; }//навигационное свойство
        public virtual ICollection<Phone>? Phones { get; set; } = new HashSet<Phone>();

        public override string ToString() => $"Абонент {SurName} {Name} {SecondName}";

    }
}
