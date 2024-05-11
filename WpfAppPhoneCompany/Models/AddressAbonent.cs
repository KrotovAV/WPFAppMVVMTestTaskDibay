using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.Models
{
    public class AddressAbonent
    {
        public Address Address { set; get; }
        public Abonent? Abonent { set; get; }

    }
}
