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

        public BaoCaoDoanhThu()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
           

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
            bool isNumber = int.TryParse(YearTextbox.Text, out int year);
            if (isNumber)
                return;
            else
            {
                return;
            }
        }

        private void GetReportButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

       
    }
}
