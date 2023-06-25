using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static QLBaiDoXe.DBClasses.ParkingVehicle;



namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for TraCuuXe.xaml
    /// </summary>
    public partial class TraCuuXe : UserControl
    {
        public DateTime LastDayThatHaveCar;
        public TraCuuXe()
        {
            InitializeComponent();
            if (DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).Count() == 0 && DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 0).Count() == 0)
            {
                var msg = "Bãi xe trống";
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(msg, "Thông báo")));
            }
            else
            {
                LastDayThatHaveCar = GetLastDayThatHaveCar();
                cbxDay.Text = LastDayThatHaveCar.Day.ToString();
                cbxMonth.Text = LastDayThatHaveCar.Month.ToString();
                cbxYear.Text = LastDayThatHaveCar.Year.ToString();
                Button_Click(null, null);
            }
        }

        private void Nullify()
        {
            cbxDay.Text = null;
            cbxMonth.Text = null;
            cbxYear.Text = null;
        }

        private DateTime? CheckInput()
        {
            string format = string.IsNullOrWhiteSpace(tpTime.Text) == true ? "d/M/yyyy" : "d/M/yyyy H:mm";
            string parse = string.Format("{0}/{1}/{2}{3}", cbxDay.Text, cbxMonth.Text, cbxYear.Text, string.IsNullOrWhiteSpace(tpTime.Text) == true ? string.Empty : $" {tpTime.Text}");
            if (DateTime.TryParseExact(parse, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<TempParkingVehicle> result = new List<TempParkingVehicle>();
            DateTime? checkDate = CheckInput();
            if (checkDate == null) { return; }
            if (string.IsNullOrWhiteSpace(tpTime.Text) == false)
            {
                result = SearchVehicle_TimeIn_DateAndHour(checkDate.Value);
            }
            else
            {
                result = SearchVehicle_TimeIn_DateOnly(checkDate.Value);
            }

            if (result.Count == 0)
            {
                MessageBox.Show("Trong khoảng thời gian bạn đã nhập không có xe trong bãi!", "Lỗi!");
            }
            lvResult.ItemsSource = null;
            lvResult.ItemsSource = result;
        }
    }
}
