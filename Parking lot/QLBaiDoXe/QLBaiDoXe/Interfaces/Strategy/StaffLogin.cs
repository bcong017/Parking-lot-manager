using QLBaiDoXe.Interfaces.Singleton;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Strategy
{
    internal class StaffLogin : ILoginStrategy
    {
        public void LoginType()
        {
            Homepage1 homepage = new Homepage1();
            foreach (var item in DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).ToList())
            {
                item.StaffID = UserProvider.Ins.currentUser.RoleID;
            }
            DataProvider.Ins.DB.SaveChanges();
            homepage.Show();
            MainWindow.ins.Close();
        }
    }
}
