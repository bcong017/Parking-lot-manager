using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Converters;
using MaterialDesignThemes.Wpf;
using QLBaiDoXe.DBClasses;
using QLBaiDoXe.ParkingLotModel;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for DSThe.xaml
    /// </summary>
    public partial class DSThe : UserControl
    {
        public DSThe()
        {
            InitializeComponent();
            ListThe.ItemsSource = Cards.GetAllParkingCards();
            cbxState.SelectedIndex = 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemThe add = new ThemThe();
            add.ShowDialog();
            ListThe.ItemsSource = Cards.GetAllParkingCards();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ListThe.Items.Count == 0)
            {
                MessageBox.Show("Danh sách thẻ rỗng!");
                return;
            }
            if (ListThe.SelectedItems == null)
            {
                MessageBox.Show("Hãy chọn thẻ cần xóa!");
            }
            else
            {
                if (MessageBox.Show("Bạn có muốn xóa thẻ đã chọn?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
                var selectedItem = (dynamic)ListThe.SelectedItems[0];
                if (selectedItem.ParkingCardID is long value)
                {
                    if (Cards.CheckCardState(value) == 1)
                    {
                        MessageBox.Show("Thẻ đang được sử dụng", "Lỗi!");
                        return;
                    }
                    Cards.DeleteCard(value);
                }
                else
                {
                    MessageBox.Show("Không nhận dạng được mã thẻ", "Lỗi");
                    return;
                }
                MessageBox.Show("Đã xóa thẻ thành công!", "Thông báo!");
                ListThe.ItemsSource = null;
                ListThe.ItemsSource = Cards.GetAllParkingCards();
            }
        }

        private void CardSearchTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(txbCardSearch.Text) && long.TryParse(txbCardSearch.Text, out long cardid))
            {
                id = cardid;
            }
            ListThe.ItemsSource = Cards.FindCards(id, cbxState.SelectedIndex);
        }

        private void StateCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardSearchTxb_TextChanged(null, null);
        }

        private void CardSearchTxb_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Classes.Validation.isNumber.IsMatch(e.Text);
        }

        private void CardSearchTxb_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }
    }
}
