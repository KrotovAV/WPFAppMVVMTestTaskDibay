using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.Models
{
    public class StreetAbonents
    {
        public Street Street {  get; set; }
        public List<Abonent>? AbonentsOfStreet {  get; set; }
    }
}
