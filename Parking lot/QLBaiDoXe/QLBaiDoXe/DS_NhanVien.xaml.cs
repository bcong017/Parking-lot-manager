﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using QLBaiDoXe.ViewModel;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for DS_NhanVien.xaml
    /// </summary>
    public partial class DS_NhanVien : UserControl
    {
        private bool checkfocus = false;
        public DS_NhanVien()
        {
            InitializeComponent();
            lvNhanVien.ItemsSource = Staffing.GetAllStaff();
            this.DataContext = new DSNhanVienViewModel();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemNhanVien add = new ThemNhanVien();
            add.ShowDialog();
            lvNhanVien.ItemsSource = Staffing.GetAllStaff();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Staff> result = new List<Staff>();
            lvNhanVien.ItemsSource = null;
            switch (cbxItem.Text)
            {
                case "Tên":
                    result = Staffing.FindStaffByName(txbSearch.Text);
                    break;
                case "Số điện thoại":
                    result = Staffing.FindStaffByPhoneNumber(txbSearch.Text);
                    break;
                case "Số CCCD":
                    result = Staffing.FindStaffByCivilID(txbSearch.Text);
                    break;
                case "Chức vụ":
                    result = Staffing.FindStaffByRoleID(txbSearch.Text);
                    break;
                case "Địa chỉ":
                    result = Staffing.FindStaffByStaffAddress(txbSearch.Text);
                    break;
            }
            if (result != null)
            {
                lvNhanVien.ItemsSource = result;
            }
        }
        private void cbxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txbSearch.IsEnabled = true;
        }
        private void btnFix_Click(object sender, RoutedEventArgs e)
        {
            if (lvNhanVien.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn thông tin nhân viên để sửa!");
                return;
            }
            var selectedItem = (dynamic)lvNhanVien.SelectedItems[0];
            SuaNhanVien snv = new SuaNhanVien();
            snv.CivilID = selectedItem.CivilID;
            snv.ShowDialog();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            lvNhanVien.ItemsSource = Staffing.GetAllStaff();
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            
            if (lvNhanVien.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn thông tin nhân viên cần xóa!");
                return;
            }
            var selectedItem = (dynamic)lvNhanVien.SelectedItems[0];
            if (MainWindow.currentUser.StaffID == selectedItem.StaffID)
            {
                MessageBox.Show("Không thể xóa nhân viên đang sử dụng ứng dụng!");
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa nhân viên đã chọn?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }    
            Staffing.DeleteStaff(selectedItem.StaffID);
            MessageBox.Show("Xóa nhân viên thành công!");
            lvNhanVien.ItemsSource = Staffing.GetAllStaff();
        }
    }
}
