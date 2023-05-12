using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            CardID.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int type = 0;
            if (TypeCbx.SelectedIndex == 1)
                type= 1;
            if (CardID.Text.Length == 10)
            {
                long temp = long.Parse(CardID.Text);
                if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == temp)) {
                    MessageBox.Show("Mã số thẻ này đã tồn tại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                }
                else
                {
                    DBClasses.Cards.AddCard(long.Parse(CardID.Text),type);
                    MessageBox.Show("Thêm thẻ thành công");
                    CardID.Clear();
                }
            }
        }
    }
}
