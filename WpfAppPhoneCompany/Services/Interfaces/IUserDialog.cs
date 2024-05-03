using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhoneCompany.Services.Interfaces
{
    public interface IUserDialog<T> where T : class
    {
        bool Edit(T t);

        bool ConfirmInformation(string Information, string Caption);
        bool ConfirmWarning(string Warning, string Caption);
        bool ConfirmError(string Error, string Caption);
    }

    //public interface IUserDialog
    //{
    //    bool Edit(Street street);

    //    bool ConfirmInformation(string Information, string Caption);
    //    bool ConfirmWarning(string Warning, string Caption);
    //    bool ConfirmError(string Error, string Caption);
    //}
}
