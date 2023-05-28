using QLBaiDoXe.DBClasses;
using System.Windows;
using System.Windows.Controls;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for GiaGuiXe.xaml
    /// </summary>
    public partial class GiaGuiXe : UserControl
    {
        public GiaGuiXe()
        {
            InitializeComponent();
            MotorbikeFeeTxb.Text = Regulation.GetParkingFeeType(MotorbikeRun.Text.Substring(0, MotorbikeRun.Text.Length - 1)).ToString();
            BikeFeeTxb.Text = Regulation.GetParkingFeeType(BikeRun.Text.Substring(0, BikeRun.Text.Length - 1)).ToString();
            CarFeeTxb.Text = Regulation.GetParkingFeeType(CarRun.Text.Substring(0, CarRun.Text.Length - 1)).ToString();
        }

        private void SaveFeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MotorbikeFeeTxb.Text, out int motorbikeFee) && int.TryParse(BikeFeeTxb.Text, out int bikeFee)
                && int.TryParse(CarFeeTxb.Text, out int carFee))
            {
                Regulation.ChangeParkingFee(MotorbikeRun.Text.Substring(0, MotorbikeRun.Text.Length - 1), motorbikeFee);
                Regulation.ChangeParkingFee(BikeRun.Text.Substring(0, BikeRun.Text.Length - 1), bikeFee);
                Regulation.ChangeParkingFee(CarRun.Text.Substring(0, CarRun.Text.Length - 1), carFee);
                MessageBox.Show("Thay đổi giá gửi xe thành công", "Thông báo!");
            }
            else
            {
                MessageBox.Show("Dữ liệu nhập vào không hợp lệ.\nVui lòng chỉ nhập số","Lỗi!");
            }
        }
    }
}
