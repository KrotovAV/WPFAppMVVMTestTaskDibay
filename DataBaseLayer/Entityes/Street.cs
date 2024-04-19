using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer.Entityes
{
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Abonent> Abonents { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
