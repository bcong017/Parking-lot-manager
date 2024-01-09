using QLBaiDoXe.Interfaces.Singleton;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Strategy
{
    internal class AdminLogin : ILoginStrategy
    {
        public void LoginType()
        {
            admin adminWindow = new admin();
            adminWindow.Show();
            MainWindow.ins.Close();
        }
    }
}
