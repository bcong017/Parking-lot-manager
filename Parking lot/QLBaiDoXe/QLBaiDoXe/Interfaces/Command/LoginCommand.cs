using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.Interfaces.Command;
using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.Interfaces.Singleton;
using System.Windows.Controls;

namespace QLBaiDoXe.Interfaces.Command
{
    internal class LoginCommand : IUserCommand
    {
        public string username;
        public string password;
        public LoginCommand (string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public void Execute()
        {
            if (Staffing.LogIn(this.username, this.password) != null)
            {
                UserProvider.Ins.currentUser = Staffing.LogIn(this.username, this.password);
                UserProvider.Ins.currentAccount = DataProvider.Ins.DB.Accounts.Where(x => x.StaffID == UserProvider.Ins.currentUser.StaffID).FirstOrDefault();
                MainWindow.currentUser = Staffing.LogIn(this.username, this.password);
                MainWindow.currentAccount = DataProvider.Ins.DB.Accounts.Where(x => x.StaffID == UserProvider.Ins.currentUser.StaffID).FirstOrDefault();

                if (UserProvider.Ins.currentUser.IsDeleted == true)
                {
                    MessageBox.Show("Tài khoản của bạn đã bị vô hiệu hóa!", "Thông báo!");
                    return;
                }
                MessageBox.Show("Đăng nhập thành công", "Thông báo!");
                if (UserProvider.Ins.currentUser.RoleID == 2)
                {
                    admin adminWindow = new admin();
                    adminWindow.Show();
                    MainWindow.ins.Close();
                }
                else
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
            else
            {
                MessageBox.Show("Sai thông tin đăng nhập", "Thông báo!");
            }

        }
    }
}
