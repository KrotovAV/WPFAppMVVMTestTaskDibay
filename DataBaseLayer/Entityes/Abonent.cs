using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entityes
{
    public class Abonent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }

        public int? AddressId { get; set; } // внешний ключ
        public virtual Address? Address { get; set; }//навигацинное свойство
        public virtual int? StreetId { get; set; } // внешний ключ
        public virtual Street? Street { get; set; }//навигацинное свойство
        public virtual ICollection<Phone>? Phones { get; set; }

    }
}
