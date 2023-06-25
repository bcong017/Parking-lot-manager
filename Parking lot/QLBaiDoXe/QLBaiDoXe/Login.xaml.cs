using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.Properties;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Staff currentUser;
        public static Account currentAccount;
        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine(DataProvider.Ins.DB.Accounts.FirstOrDefault(x => x.AccountName == "admin").AccountName);
            Debug.WriteLine("last log in date: " + Settings.Default.currentDate.ToString());
            if (DateTime.Now.Month != Settings.Default.currentDate.Month)
            {
                Settings.Default.todayVehicleIn = Settings.Default.todayVehicleOut = 0;
            }
            Settings.Default.currentDate = DateTime.Now;
            Settings.Default.Save();
            Debug.WriteLine("current date: " + Settings.Default.currentDate.ToString());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Staffing.LogIn(txbUsername.Text, txbPassword.Password) != null )
            {
                currentUser = Staffing.LogIn(txbUsername.Text, txbPassword.Password);
                currentAccount = DataProvider.Ins.DB.Accounts.Where(x => x.StaffID == currentUser.StaffID).FirstOrDefault();
                if (currentUser.IsDeleted == true)
                {
                    MessageBox.Show("Tài khoản của bạn đã bị vô hiệu hóa!", "Thông báo!");
                    return;
                }
                MessageBox.Show("Đăng nhập thành công", "Thông báo!");
                if (currentUser.RoleID == 2)
                {
                    admin adminWindow = new admin();
                    adminWindow.Show();
                    this.Close();
                }
                else
                {
                    Homepage1 homepage = new Homepage1();
                    foreach ( var item in DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).ToList())
                    {
                        item.StaffID = MainWindow.currentUser.StaffID;
                    }
                    DataProvider.Ins.DB.SaveChanges();
                    homepage.Show();
                    this.Close();
                }
            }
            else
            {
                
                MessageBox.Show("Sai thông tin đăng nhập", "Thông báo!");
            }
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(null,null);
            }
        }

        private void lbReset_Password_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            MessageBox.Show("Hãy thông báo quản trị viên của bạn để được đặt lại mật khẩu", "Thông báo!");
        }

        private void lbReset_Password_MouseEnter(object sender, MouseEventArgs e)
        {
            lbReset_Password.Cursor = Cursors.Hand;
        }
    }
}
