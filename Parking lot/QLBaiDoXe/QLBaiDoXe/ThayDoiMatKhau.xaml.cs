using QLBaiDoXe.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for ThayDoiMatKhau.xaml
    /// </summary>
    public partial class ThayDoiMatKhau : Window
    {
        public ThayDoiMatKhau()
        {
            InitializeComponent();
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (txbRePwd.Text != txbNewPwd.Text)
            {
                MessageBox.Show("Bạn đã nhập lại sai mật khẩu!", "Thông báo!");
                return;
            }
            Staffing.ChangePassword(MainWindow.currentAccount.AccountName, txbNewPwd.Text, txbCurrentPwd.Text);
            txbCurrentPwd.Clear();
            txbNewPwd.Clear();
            txbRePwd.Clear();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
