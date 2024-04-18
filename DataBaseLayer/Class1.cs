
namespace DataBaseLayer
{
    /*
    В базе данных определить следующие основные таблицы: 
    1) Таблица Abonent для хранения информации об абонентах (фио - обязательно); 
    2) Таблица Address для хранения адресов абонентов; 
    3) Таблица PhoneNumber для хранения номеров (учесть, что существует 3 типа номера - домашний, рабочий, мобильный); 
    4) Таблица Streets для хранения обслуживаемых компанией улиц.
    */
    //public class Abonent
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string SecondName { get; set; }
    //    public string SurName { get; set; }
    //    public int AddressId { get; set; } // внешний ключ
    //    public int? PhoneHomeId { get; set; } // внешний ключ
    //    public int? PhoneWorkId { get; set; } // внешний ключ
    //    public int? PhoneMobileId { get; set; } // внешний ключ

    //    public virtual Address Address { get; set; }//навигацинное свойство
    //    public virtual Phone PhoneHome {  get; set; }//навигацинное свойство
    //    public virtual Phone PhoneWork { get; set; }//навигацинное свойство
    //    public virtual Phone PhoneMobile { get; set; }//навигацинное свойство



    //}
    //public class Address
    //{
    //    public int Id { get; set; }
    //    public int StreetId { get; set; } // //внешний ключ
    //    public virtual Street Street { get; set; } //навигацинное свойство
    //    public int House { get; set; }
    //    public int ApartNum { get; set; }
    //    public string City { get; set; }
        
        
    //}

    //public class Phone
    //{
    //    public int Id { get; set; }
    //    public int Number { get; set; }

    //    //public int AbonentId { get; set; } // внешний ключ
    //    //public virtual Abonent Abonent { get; set; }//навигацинное свойство
    //}

    //public class Street
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //    public virtual List<Abonent> Abonents { get; set; }
    //}
}