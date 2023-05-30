using ControlzEx.Standard;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;


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
            if ( DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 1).Count() == 0 && DataProvider.Ins.DB.Vehicles.Where(x => x.VehicleState == 0).Count() == 0)
            {
                var msg = "Chưa có xe trong cơ sở dữ liệu";
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(msg,"Lỗi")));
            }
            else
            {
                int pos = ParkingVehicle.GetLastDayThatHaveCar().IndexOf(' ');
                LastDayThatHaveCar = new DateTime();
                LastDayThatHaveCar = DateTime.Parse(ParkingVehicle.GetLastDayThatHaveCar().Substring(0, pos));
                cbxDay.Text = LastDayThatHaveCar.Day.ToString();
                cbxMonth.Text = LastDayThatHaveCar.Month.ToString();
                cbxYear.Text = LastDayThatHaveCar.Year.ToString();
                List<Vehicle> result = new List<Vehicle>();

                result = ParkingVehicle.SearchVehicle_TimeIn_DateOnly(LastDayThatHaveCar);
                lvResult.ItemsSource = result;
            }
        }
        private void Nullify()
        {
            cbxDay.Text = null;
            cbxMonth.Text = null;
            cbxYear.Text = null;
        }
        private bool CheckInput()
        {

            int testDay;
            int.TryParse(cbxDay.Text, out testDay);


            int testMonth;
            int.TryParse(cbxMonth.Text, out testMonth);


            int testYear;
            int.TryParse(cbxYear.Text, out testYear);


            if (testMonth == 2)
            {
                if (DateTime.IsLeapYear(testYear))
                {
                    if (testDay > 29)
                    {
                        MessageBox.Show("Bạn đã nhập ngày không phù hợp","Lỗi!");
                        Nullify();
                        return false;
                    }
                }
                else
                {
                    if (testDay > 28)
                    {
                        MessageBox.Show("Bạn đã nhập ngày không phù hợp", "Lỗi!");
                        Nullify();
                        return false;
                    }
                }
            }
            if (testMonth == 4 || testMonth == 6 || testMonth == 9 || testMonth == 11)
                if (testDay > 30)
                {
                    MessageBox.Show("Bạn đã nhập ngày không phù hợp", "Lỗi!");
                    Nullify();
                    return false;
                }
            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            List<Vehicle> result = new List<Vehicle>();
            string hour = string.Empty;
            if (TimePicker.Text != null)
            {
                if (TimePicker.Text.Length == 4)
                    hour = TimePicker.Text.Substring(0, 1);
                else
                    hour = TimePicker.Text.Substring(0, 2);
                DateTime datesearch = new DateTime(int.Parse(cbxYear.Text), int.Parse(cbxMonth.Text), int.Parse(cbxDay.Text), int.Parse(hour), 0, 0);
                result = ParkingVehicle.SearchVehicle_TimeIn_DateAndHour(datesearch);
            }
            else
            {
                DateTime datesearch = new DateTime(int.Parse(cbxYear.Text), int.Parse(cbxMonth.Text), int.Parse(cbxDay.Text));
                result = ParkingVehicle.SearchVehicle_TimeIn_DateOnly(datesearch);

            }
            if (CheckInput() == false) { return; }
              
            if (result.Count == 0)
            {
                MessageBox.Show("Trong khoảng thời gian bạn đã nhập không có xe trong bãi!","Lỗi!");
            }
            lvResult.ItemsSource = null;
            lvResult.ItemsSource = result;

        }
    }
}
