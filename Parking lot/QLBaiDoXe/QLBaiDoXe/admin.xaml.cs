using System;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.ViewModel;
namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public static DateTime LoginTime = new DateTime();
        public static DateTime LogoutTime = new DateTime();
        public admin()
        {
            InitializeComponent();
            this.DataContext = new AdminViewModel();
            LoginTime = DateTime.Now;
            txtbStaffName.Text = MainWindow.currentUser.StaffName;
            txtbStaffAccount.Text = MainWindow.currentAccount.AccountName;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Staffing.LogOut(MainWindow.currentAccount.AccountName);
            MainWindow.currentUser = null;
            MainWindow.currentAccount = null;
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                Homepage1 add = new Homepage1();
                add.Show();
                this.Close();
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ThayDoiMatKhau tdmk = new ThayDoiMatKhau();
            tdmk.ShowDialog();
        }
    }
}
