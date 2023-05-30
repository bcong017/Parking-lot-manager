using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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

            YearTextbox.Text = DateTime.Now.Year.ToString();
            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            YFormatter = value => value + "";

            //modifying the series collection will animate and update the chart
            /*SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });*/

            //modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        private void YearTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (first)
            {
                first = false; return;
            }
            
            bool isNumber = int.TryParse(YearTextbox.Text, out int year);
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
                                income += vehicleFee[k].VehicleType.ParkingFee;
                            }
                        }
                        total += income;

                    }
                }
            }
            IncomeTextbox.Text = total.ToString() + " đồng";
            SeriesCollection.Clear();
            SeriesCollection.AddRange(lineSeries);
        }

    }
}
