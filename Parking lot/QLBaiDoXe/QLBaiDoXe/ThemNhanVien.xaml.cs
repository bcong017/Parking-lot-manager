using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QLBaiDoXe.DBClasses;
using System.Text.RegularExpressions;
using QLBaiDoXe.Classes;
using System.Windows.Media.Media3D;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for ThemNhanVien.xaml
    /// </summary>
    public partial class ThemNhanVien : Window
    {
        public ThemNhanVien()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txbPhoneNumb.Text))
            {
                if (!(Classes.Validation.isPhoneNumber.IsMatch(txbPhoneNumb.Text) && !(txbPhoneNumb.Text.IndexOf("0", 0, 1) == -1)))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!");
                    return;
                }
            }
            if (!Classes.Validation.isCivilId.IsMatch(txbCivilID.Text))
            {
                MessageBox.Show("CMND/CCCD không hợp lệ!");
                return;
            }
            if (cbxRole.Text == "Quản trị viên")
            {
                if (string.IsNullOrEmpty(dpBirthday.Text))
                    Staffing.AddAdminInfo(txbName.Text, txbCivilID.Text, txbPhoneNumb.Text, txbAddress.Text, null, txbAccName.Text, txbCivilID.Text);
                else
                    Staffing.AddAdminInfo(txbName.Text, txbCivilID.Text, txbPhoneNumb.Text, txbAddress.Text, DateTime.Parse(dpBirthday.Text), txbAccName.Text, txbCivilID.Text);
            }
            else
            {
                if (string.IsNullOrEmpty(dpBirthday.Text))
                    Staffing.AddStaffInfo(txbName.Text, txbCivilID.Text, txbPhoneNumb.Text, txbAddress.Text, null, txbAccName.Text, txbCivilID.Text);
                else
                    Staffing.AddStaffInfo(txbName.Text, txbCivilID.Text, txbPhoneNumb.Text, txbAddress.Text, DateTime.Parse(dpBirthday.Text), txbAccName.Text, txbCivilID.Text);

            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumbericPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Classes.Validation.isNumber.IsMatch(e.Text);
        }

        private void txbCivilID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }
    }
}
