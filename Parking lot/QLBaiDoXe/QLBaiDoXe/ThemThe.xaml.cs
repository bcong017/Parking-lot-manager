using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QLBaiDoXe.Classes;
using QLBaiDoXe.ParkingLotModel;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for ThemThe.xaml
    /// </summary>
    public partial class ThemThe : Window
    {
        public ThemThe()
        {
            InitializeComponent();
            txbCardID.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                int type = 0;
            if (cbxType.SelectedIndex == 1)
                type= 1;
            if (txbCardID.Text.Length == 10)
            {
                long temp = long.Parse(txbCardID.Text);
                if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == temp)) {
                    MessageBox.Show("Mã số thẻ này đã tồn tại", "Lỗi!", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                }
                else
                {
                    DBClasses.Cards.AddCard(long.Parse(txbCardID.Text),type);
                    MessageBox.Show("Thêm thẻ thành công", "Thông báo!");
                    txbCardID.Clear();
                }
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CardID_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Classes.Validation.isNumber.IsMatch(e.Text);
        }

        private void CardID_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }
    }
}
