using QLBaiDoXe.DBClasses;
using QLBaiDoXe.Interfaces;
using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.Properties;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QLBaiDoXe.Interfaces.Command;
using System.Web.UI.WebControls;
using QLBaiDoXe.Interfaces.Singleton;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Staff currentUser;
        public static Account currentAccount;
        public static MainWindow ins;
        
        public MainWindow()
        {
            ins = this;
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
            IUserCommand login = new LoginCommand(txbUsername.Text, txbPassword.Password);
            CommandOptions menu = new CommandOptions(login, "login");
            menu.Login();
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
