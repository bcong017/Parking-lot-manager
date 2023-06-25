using Microsoft.SqlServer.Server;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ViewModel;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for Bang_Cham_Cong.xaml
    /// </summary>
    public partial class Bang_Cham_Cong : UserControl
    {
        public Bang_Cham_Cong()
        {
            InitializeComponent();
            dpStartDate.Text = dpEndDate.Text = DateTime.Now.Date.ToString();
            dpStartDate.DisplayDateStart = Staffing.GetFirstLogin();
            dpStartDate.DisplayDateEnd = Staffing.GetLastLogin();
            dpEndDate.DisplayDateEnd = dpStartDate.DisplayDateEnd;
        }

        private void StaffNameTxb_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(dpStartDate.Text) || string.IsNullOrEmpty(dpEndDate.Text))
                lvTimekeep.ItemsSource = Staffing.GetTimekeepForStaff(txbStaffName.Text);
            else if (DateTime.TryParseExact(dpStartDate.Text + " 00:00:00", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate)
                && DateTime.TryParseExact(dpEndDate.Text + " 23:59:59", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                lvTimekeep.ItemsSource = Staffing.GetSpecificTimekeeps(txbStaffName.Text, startDate, endDate);
            }
        }

        private void StartDateDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dpStartDate.Text) == false
                && DateTime.TryParseExact(dpStartDate.Text + " 00:00:00", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                dpEndDate.DisplayDateStart = startDate;
                if (dpEndDate.SelectedDate?.CompareTo(startDate) < 0)
                {
                    dpEndDate.SelectedDate = startDate.Date;
                }
                DateTime endDate = DateTime.MinValue;
                bool hasEndDate = string.IsNullOrWhiteSpace(dpEndDate.Text) == false
                    && DateTime.TryParseExact(dpEndDate.Text + " 23:59:59", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                if (string.IsNullOrWhiteSpace(txbStaffName.Text) == false)
                {
                    lvTimekeep.ItemsSource = hasEndDate ? Staffing.GetSpecificTimekeeps(txbStaffName.Text, startDate, endDate) : Staffing.GetTimekeepForStartDateAndName(txbStaffName.Text, startDate);
                }
                else
                {
                    lvTimekeep.ItemsSource = hasEndDate ? Staffing.GetTimekeepForDate(startDate, endDate) : Staffing.GetTimekeepForStartDate(startDate);
                }
            }
        }

        private void EndDateDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dpEndDate.Text) == false
                && DateTime.TryParseExact(dpEndDate.Text + " 23:59:59", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                DateTime startDate = DateTime.MinValue;
                bool hasStartDate = string.IsNullOrWhiteSpace(dpStartDate.Text) == false
                    && DateTime.TryParseExact(dpStartDate.Text + " 00:00:00", "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                if (string.IsNullOrWhiteSpace(txbStaffName.Text) == false)
                {
                    lvTimekeep.ItemsSource = hasStartDate ? Staffing.GetSpecificTimekeeps(txbStaffName.Text, startDate, endDate) : Staffing.GetTimekeepForEndDateAndName(txbStaffName.Text, endDate);
                }
                else
                {
                    lvTimekeep.ItemsSource = hasStartDate ? Staffing.GetTimekeepForDate(startDate, endDate) : Staffing.GetTimekeepForEndDate(endDate);
                }
            }
        }
    }
}
