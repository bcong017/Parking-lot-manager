using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for BaoCaoDoanhThu.xaml
    /// </summary>
    public partial class BaoCaoDoanhThu : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public bool first = true;

        public BaoCaoDoanhThu()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            UpdateReport(DateTime.Now.Year);
            txbYear.Text = DateTime.Now.Year.ToString();
            Labels = new[] { "Thg 1", "Thg 2", "Thg 3", "Thg 4", "Thg 5", "Thg 6", "Thg 7", "Thg 8", "Thg 9", "Thg 10", "Thg 11", "Thg 12" };
            YFormatter = value => value.ToString("N0");
            DataContext = this;
        }

        private void YearTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (first)
            {
                first = false; return;
            }
            
            bool isNumber = int.TryParse(txbYear.Text, out int year);
            if (isNumber && year >= (DateTime.Now.Year-10) && year <= (DateTime.Now.Year))
            {
                SeriesCollection.Clear();
                UpdateReport(year);
            }

            else
            {
                return;
            }
        }

        private void GetReportButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateReport(int year)
        {
            List<VehicleType> vehicleTypes = Regulation.GetAllVehicleTypes();
            List<VehicleType> vehicleTypeTemp = Regulation.GetAllVehicleTypes();
            List<LineSeries> lineSeries = new List<LineSeries>();
            int total = 0;

            for (int i = 0; i < vehicleTypes.Count; i++)
            {
                lineSeries.Add(new LineSeries()
                {
                    Title = vehicleTypes[i].VehicleTypeName,
                    Values = new ChartValues<int>()
                });
            }

            for ( int m = 1; m <= 12; m++)
            {
                if (ParkingVehicle.GetAllParkedOutVehicle(m,year).Count == 0)
                {
                    for (int j = 0; j < lineSeries.Count; j++)
                    {
                        lineSeries[j].Values.Add(0);
                    }
                }
                else
                {
                    for (int j = 0;j < lineSeries.Count; j++)
                    {
                        int income = 0;
                        var vehicleFee = ParkingVehicle.GetAllParkedOutVehicle(m, year);

                        lineSeries[j].Values.Add(vehicleFee.Where(x => x.VehicleType.VehicleTypeName == lineSeries[j].Title).Count()); //Thêm dòng với điều kiện:   Loại xe đó giống với tên loại xe trên đồ thị (chuyển qua thành List rồi sau đó Count để đếm trong List có bao nhiêu thằng )

                        for (int k = 0; k< vehicleFee.Count; k++)
                        {
                            if (lineSeries[j].Title == vehicleFee[k].VehicleType.VehicleTypeName)
                            {
                                income += vehicleFee[k].Fee;
                            }
                        }
                        total += income;

                    }
                }
            }
            txbIncome.Text = total.ToString() + " đồng";
            SeriesCollection.Clear();
            SeriesCollection.AddRange(lineSeries);
        }

        private void YearTextbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Classes.Validation.isNumber.IsMatch(e.Text);
        }

        private void YearTextbox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }
    }
}
