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
        //public int AddressId { get; set; } // внешний ключ
        //public int? PhoneHomeId { get; set; } // внешний ключ
        //public int? PhoneWorkId { get; set; } // внешний ключ
        //public int? PhoneMobileId { get; set; } // внешний ключ

        public virtual Address Address { get; set; }//навигацинное свойство
        public virtual Phone PhoneHome { get; set; }//навигацинное свойство
        public virtual Phone PhoneWork { get; set; }//навигацинное свойство
        public virtual Phone PhoneMobile { get; set; }//навигацинное свойство



    }
}
