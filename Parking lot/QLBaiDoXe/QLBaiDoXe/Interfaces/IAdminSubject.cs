using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBaiDoXe.Interfaces;

namespace QLBaiDoXe.Interfaces
{
    internal interface IAdminSubject
    {
        void Logout(Observer observer);

        void Login(Observer observer);

        void Notify(long staffId);
    }
}
