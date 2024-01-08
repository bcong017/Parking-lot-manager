using System;
using System.Windows;
using System.Windows.Controls;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;
using static QLBaiDoXe.DBClasses.Staffing;

namespace QLBaiDoXe
{
    
    /// <summary>
    /// Interaction logic for DS_NhanVien.xaml
    /// </summary>
    public partial class DS_NhanVien : UserControl
    {
		public interface IStaffOperationStrategy
		{
			void ExecuteStrategy(dynamic selectedItem);
		}

		public class ThoiViecStrategy : IStaffOperationStrategy
		{
			private readonly DS_NhanVien _dsNhanVien;

			public ThoiViecStrategy(DS_NhanVien dsNhanVien)
			{
				_dsNhanVien = dsNhanVien;
			}

			public void ExecuteStrategy(dynamic selectedItem)
			{
				if (MessageBox.Show("Bạn có muốn thôi việc nhân viên đã chọn?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
				{
					return;
				}
				Staffing.DeleteStaff(selectedItem.StaffID);
				MessageBox.Show("Thôi việc nhân viên thành công!", "Thông báo!");
				_dsNhanVien.StateCbx_SelectionChanged(null, null);
			}
		}

		public class KhoiPhucStrategy : IStaffOperationStrategy
		{
			private readonly DS_NhanVien _dsNhanVien;

			public KhoiPhucStrategy(DS_NhanVien dsNhanVien)
			{
				_dsNhanVien = dsNhanVien;
			}

			public void ExecuteStrategy(dynamic selectedItem)
			{
				if (MessageBox.Show("Bạn có muốn khôi phục nhân viên đã chọn?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
				{
					return;
				}
				Staffing.RestoreStaff(selectedItem.StaffID);
				MessageBox.Show("Khôi phục nhân viên thành công!", "Thông báo!");
				_dsNhanVien.StateCbx_SelectionChanged(null, null);
			}
		}

		private IStaffOperationStrategy _staffOperationStrategy;

		public DS_NhanVien()
        {
            InitializeComponent();
            cbxState.SelectedIndex = 0;
            lvNhanVien.ItemsSource = Staffing.GetAllStaff(false);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemNhanVien add = new ThemNhanVien();
            add.ShowDialog();
            StateCbx_SelectionChanged(null, null);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StateCbx_SelectionChanged(null,null);
        }
        private void cbxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StateCbx_SelectionChanged(null, null);
        }
        private void btnFix_Click(object sender, RoutedEventArgs e)
        {
            if (lvNhanVien == null)
                return;
            
            if (lvNhanVien.SelectedItem == null || lvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn thông tin nhân viên để sửa!", "Lỗi!");
                return;
            }
            var selectedItem = (dynamic)lvNhanVien.SelectedItems[0];
            if (MainWindow.currentUser.StaffID == selectedItem.StaffID)
            {
                MessageBox.Show("Không thể sửa nhân viên đang sử dụng ứng dụng!", "Lỗi!");
                return;
            }
            SuaNhanVien snv = new SuaNhanVien();
            snv.CivilID = selectedItem.CivilID;
            snv.ShowDialog();
            StateCbx_SelectionChanged(null, null);
        }
		private void btnDel_Click(object sender, RoutedEventArgs e)
		{
			if (lvNhanVien == null)
				return;

			if (lvNhanVien.SelectedItem == null || lvNhanVien.SelectedItems.Count == 0)
			{
				MessageBox.Show("Hãy chọn thông tin nhân viên!", "Lỗi!");
				return;
			}

			var selectedItem = (dynamic)lvNhanVien.SelectedItems[0];

			// Decide which strategy to use based on the operation
            if (btnDel.Content.ToString() == "Thôi việc")
            {
                _staffOperationStrategy = new ThoiViecStrategy(this);

			}
            else
            {
				_staffOperationStrategy = new KhoiPhucStrategy(this);
			}

			// Execute the selected strategy
			_staffOperationStrategy.ExecuteStrategy(selectedItem);
		}

		private void StateCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxState == null)
            {
                return;
            }    
            switch (cbxState.SelectedIndex)
            {
                case 0:
                    lvNhanVien.ItemsSource = FindStaff(cbxItem.Text, false, txbSearch.Text);
                    return;
                case 1:
                    lvNhanVien.ItemsSource = FindStaff(cbxItem.Text, true, txbSearch.Text);
                    return;
                case 2:
                    lvNhanVien.ItemsSource = FindStaff(cbxItem.Text, null, txbSearch.Text);
                    return;
                default:
                    lvNhanVien.ItemsSource = FindStaff(cbxItem.Text, null, txbSearch.Text);
                    return;


            }

        }

        private void lvNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNhanVien == null)
                return;
            if (lvNhanVien.SelectedItems.Count == 0)
                return;
            var selectedItem = (dynamic)lvNhanVien.SelectedItems[0];
            Staff selectedstaff = GetStaffByCivilID(selectedItem.CivilID);
            if (selectedstaff.IsDeleted == true)
            {
                btnDel.Content = "Khôi phục";
            }    
            else
            {
                btnDel.Content = "Thôi việc";
            }    
        }
	}
}
