using System.Windows;
using System.Windows.Controls;
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
                var selectedItems = (dynamic)ListThe.SelectedItems[0];
                if ( Cards.CheckCardState((long)selectedItems.ParkingCardID) == 1)
                {
                    MessageBox.Show("Thẻ đang được sử dụng", "Lỗi!");
                    return;
                }
                Cards.DeleteCard((long)selectedItems.ParkingCardID);
                MessageBox.Show("Đã xóa thẻ thành công!", "Thông báo!");
                ListThe.ItemsSource = null;
                ListThe.ItemsSource = Cards.GetAllParkingCards();
            }
        }

        private void CardSearchTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (long.TryParse(CardSearchTxb.Text, out long cardId))
            {
                if (StateCbx.Text == "Đang dùng")                 
                    ListThe.ItemsSource = Cards.GetCards(cardId, 1);
                if (StateCbx.Text == "Chưa dùng")
                    ListThe.ItemsSource = Cards.GetCards(cardId, 0);
                else
                    ListThe.ItemsSource = Cards.GetCardsFromId(cardId);
            }
            
        }
    }
}
