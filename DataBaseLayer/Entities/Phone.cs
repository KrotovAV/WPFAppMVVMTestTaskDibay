using DataBaseLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entities
{
    public class Phone : Entity
    {
        //public int Id { get; set; }
        public int Number { get; set; }
        public TypePhone TypePhone { get; set; }

        public int? AbonentId { get; set; } //внешний ключ
        public virtual Abonent? Abonent { get; set; }//навигацинное свойство

        public override string ToString() => $"Тел. {Number} {TypePhone}";
    }
    

}
