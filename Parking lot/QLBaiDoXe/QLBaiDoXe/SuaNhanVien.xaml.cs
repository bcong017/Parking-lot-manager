using QLBaiDoXe.Classes;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for SuaNhanVien.xaml
    /// </summary>
    public partial class SuaNhanVien : Window
    {
        Staff staff;
        public SuaNhanVien()
        {
            InitializeComponent();

        }
        private string civilID;
        public string CivilID
        {
            get { return civilID; }
            set
            {
                civilID = value;
                staff = Staffing.GetStaffByCivilID(civilID);
            }
        }

        private void TextBoxName_Loaded(object sender, RoutedEventArgs e)
        {
            txbName.Text = staff.StaffName;
        }

        private void txbPhoneNumb_Loaded(object sender, RoutedEventArgs e)
        {
            txbPhoneNumb.Text = staff.PhoneNumber;
        }


        private void txbCivilID_Loaded(object sender, RoutedEventArgs e)
        {
            txbCivlID.Text = staff.CivilID;
        }

        private void datePicker_Loaded(object sender, RoutedEventArgs e)
        {
            dpBirthday.Text = staff.DateOfBirth.ToString();
        }

        private void cbxRole_Loaded(object sender, RoutedEventArgs e)
        {
            if (staff.RoleID == 1)
                cbxRole.Text = "Nhân viên";
            else
                cbxRole.Text = "Quản trị viên";
        }

        private void txbAddress_Loaded(object sender, RoutedEventArgs e)
        {
            txbAddress.Text = staff.StaffAddress;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdjust_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txbPhoneNumb.Text))
            {
                if (!(Classes.Validation.isPhoneNumber.IsMatch(txbPhoneNumb.Text) && !(txbPhoneNumb.Text.IndexOf("0", 0, 1) == -1)))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!");
                    return;
                }
            }
            if (!Validation.isCivilId.IsMatch(txbCivlID.Text))
            {
                MessageBox.Show("CMND/CCCD không hợp lệ!");
                return;
            }
            if (String.IsNullOrEmpty(dpBirthday.Text))
            {
                if (cbxRole.Text == "Quản trị viên")
                    Staffing.ChangeStaffInfo(staff.StaffID, txbName.Text, txbCivlID.Text, "admin", txbPhoneNumb.Text, txbAddress.Text, null);
                else
                    Staffing.ChangeStaffInfo(staff.StaffID, txbName.Text, txbCivlID.Text, "staff", txbPhoneNumb.Text, txbAddress.Text, null);
            }
            else
            {
                if (cbxRole.Text == "Quản trị viên")
                    Staffing.ChangeStaffInfo(staff.StaffID, txbName.Text, txbCivlID.Text, "admin", txbPhoneNumb.Text, txbAddress.Text, DateTime.Parse(dpBirthday.Text));
                else
                    Staffing.ChangeStaffInfo(staff.StaffID, txbName.Text, txbCivlID.Text, "staff", txbPhoneNumb.Text, txbAddress.Text, DateTime.Parse(dpBirthday.Text));
            }
        }

        private void NumbericPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validation.isNumber.IsMatch(e.Text);
        }

        private void txbCivilID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }

        private void btnResetPwd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đặt lại mật khẩu cho nhân viên này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }
            Staffing.ResetPassword(Staffing.GetAccountNameByCivilID(civilID));
        }
    }
}
