using QLBaiDoXe.DBClasses;
using QLBaiDoXe.Interfaces.Singleton;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLBaiDoXe.Interfaces.Command
{
    internal class LogoutCommand : IUserCommand
    {
        public LogoutCommand()
        {

        }

        public void Execute()
        {
            Staffing.LogOut(MainWindow.currentAccount.AccountName);
            UserProvider.Ins.currentUser = null;
            UserProvider.Ins.currentAccount = null;
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
        }
    }
}
