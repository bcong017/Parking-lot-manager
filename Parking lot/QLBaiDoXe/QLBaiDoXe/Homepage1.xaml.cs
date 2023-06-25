using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using QLBaiDoXe.Classes;
using AForge.Video;
using AForge.Video.DirectShow;
using QLBaiDoXe.ParkingLotModel;
using QLBaiDoXe.DBClasses;
using System.IO;

namespace QLBaiDoXe
{
    /// <summary>
    /// Interaction logic for Homepage1.xaml
    /// </summary>
    public partial class Homepage1 : Window
    {
        DateTime date = DateTime.Now;
        public Homepage1()
        {
            InitializeComponent();
            GetVideoDevices();
            StartCamera();
            this.Closing += Homepage1_Closing;
            txblAmoutIn.Text = ParkingVehicle.GetVehicleInNumber(date).ToString();
            txblAmountOut.Text = ParkingVehicle.GetVehicleOutNumber(date).ToString();
            txblAmountParked.Text = ParkingVehicle.GetParkedVehicleNumber().ToString();
        }

        private void Homepage1_Closing(object sender, CancelEventArgs e)
        {
            StopCamera();
        }

        #region Public properties
        public ObservableCollection<FilterInfo> VideoDevicesList { get; set; }

        public FilterInfo CurrentDevice { get; set; }

        public FilterInfo CurrentDevice1 { get; set; }
        #endregion

        #region Private fields

        private IVideoSource _videoSource;

        private IVideoSource _videoSource1;

        #endregion

        #region Code
        public void GetVideoDevices()
        {
            VideoDevicesList = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevicesList.Add(filterInfo);
            }
            if (VideoDevicesList.Any())
            {
                if (VideoDevicesList.Count == 2)
                {
                    CurrentDevice = VideoDevicesList[0];
                    CurrentDevice1 = VideoDevicesList[1];
                }
                else if (VideoDevicesList.Count == 1)
                {
                    CurrentDevice = VideoDevicesList[0];
                }
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void StartCamera()
        {
            if (CurrentDevice != null && CurrentDevice1 != null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource1 = new VideoCaptureDevice(CurrentDevice1.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource1.NewFrame += video_NewFrame1;
                _videoSource.Start();
                _videoSource1.Start();
            }
            else if (CurrentDevice != null && CurrentDevice1 == null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
            }
        }
        private void video_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { imgCarIn.Source = bi; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Phần mềm bị lỗi:" + exc.Message + ". Vui lòng liên hệ nhân viên bảo trì để biết thêm chi tiết", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }
        private void video_NewFrame1(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { imgCarOut.Source = bi; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Phần mềm bị lỗi:" + exc.Message + ". Vui lòng liên hệ nhân viên bảo trì để biết thêm chi tiết", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning && _videoSource1 != null && _videoSource1.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
                _videoSource1.SignalToStop();
                _videoSource1.NewFrame -= new NewFrameEventHandler(video_NewFrame1);
            }

            else if (_videoSource != null && _videoSource.IsRunning && _videoSource1 == null)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgCarOutPlate.Source = null;
            if (txbCardIn.Text.Length == 10)
            {
                string ID = (txbCardIn.Text).Trim();
                long ID_temp = long.Parse(ID);
                if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == ID_temp))
                {
                    if (DBClasses.Cards.CheckCardState(long.Parse(ID)) == 1)
                    {
                        Dispatcher.BeginInvoke
                            (
                            new Action(() => MessageBox.Show("Thẻ đã được sử dụng", "Lưu ý",
                            MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification)), DispatcherPriority.ApplicationIdle
                            );
                        txbCardIn.Clear();
                    }
                    else
                    {
                        try
                        {
                            txbCardIn.Clear();

                            imgCarInPlate.Source = imgCarIn.Source;
                            BitmapImage temp = (BitmapImage)imgCarInPlate.Source;
                            Bitmap temp1 = BitmapImageConvert.BitmapImage2Bitmap(temp);

                            txblDateIn.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                            txblTimeIn.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString(); txblDateOut.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                            txblDateOut.Text = "--/--/----";
                            txblTimeOut.Text = "00:00:00";
                            txblDateInCheckOut.Text = "--/--/----";
                            txblTimeInCheckOut.Text = "00:00:00";
                            txblTypeOut.Text = "------";
                            txblPriceTagOut.Text = "------" + " đồng";

                            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                            string carPicFolderPath = Path.Combine(projectDirectory, "CarPic");

                            //string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", $"DA{DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss tt")}.jpg");
                            string fileName = $"DA{DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss tt")}.jpg";
                            string path = Path.Combine(carPicFolderPath, fileName);

                            DBClasses.ParkingVehicle.VehicleIn(txblTypeIn.Text.ToString().Trim(), long.Parse(ID), fileName);
                            temp1.Save(path);

                            txblAmoutIn.Text = ParkingVehicle.GetVehicleInNumber(date).ToString();
                            txblAmountParked.Text = ParkingVehicle.GetParkedVehicleNumber().ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Phần mềm bị lỗi: " + ex.Message + " Vui lòng liên hệ nhân viên bảo trì để biết thêm chi tiết", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
                else
                {
                    Dispatcher.BeginInvoke
                            (
                            new Action(() => MessageBox.Show("Thẻ không tồn tại", "Lỗi!", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification)),
                            DispatcherPriority.ApplicationIdle
                            );
                    txbCardIn.Clear();
                }

            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgCarInPlate.Source = null;
            if (txbCardOut.Text.Length == 10)
            {
                string ID = (txbCardOut.Text).Trim();
                long ID_temp = long.Parse(ID);
                if (DataProvider.Ins.DB.ParkingCards.Any(x => x.ParkingCardID == ID_temp))
                {
                    if (DBClasses.Cards.CheckCardState(long.Parse(ID)) == 0)
                    {
                        Dispatcher.BeginInvoke
                            (
                            new Action(() => MessageBox.Show("Thẻ chưa được sử dụng", "Lỗi!", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification)),
                            DispatcherPriority.ApplicationIdle
                            );
                        txbCardOut.Clear();
                    }
                    else
                    {
                        //try
                        {
                            txbCardOut.Clear();

                            
                            var vehicle = DataProvider.Ins.DB.Vehicles.FirstOrDefault(x => x.ParkingCardID == ID_temp && x.VehicleState == 1);
                            string fileName = vehicle.VehicleImage;
                            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                            string path = Path.Combine(projectDirectory, "CarPic");
                            string pathWithName = Path.Combine(path, fileName);
                            Uri imgDir = new Uri(pathWithName);
                            imgCarOutPlate.Source = new BitmapImage(imgDir);

                            txblDateOut.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                            txblTimeOut.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                            txblDateInCheckOut.Text = vehicle.TimeStartedParking.Day.ToString() + "/" + vehicle.TimeStartedParking.Month.ToString() + "/" + vehicle.TimeStartedParking.Year.ToString();
                            txblTimeInCheckOut.Text = vehicle.TimeStartedParking.Hour.ToString() + ":" + vehicle.TimeStartedParking.Minute.ToString() + ":" + vehicle.TimeStartedParking.Second.ToString();
                            txblDateIn.Text = "--/--/----";
                            txblTimeIn.Text = "00:00:00";
                            txblTypeOut.Text = vehicle.VehicleType.VehicleTypeName;
                            txblPriceTagOut.Text = vehicle.VehicleType.ParkingFee.ToString() + " đồng";

                            DBClasses.ParkingVehicle.VehicleOut(long.Parse(ID), MainWindow.currentUser.StaffID);

                            txblAmountOut.Text = ParkingVehicle.GetVehicleOutNumber(date).ToString();
                            txblAmountParked.Text = ParkingVehicle.GetParkedVehicleNumber().ToString();
                        }
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show("Phần mềm bị lỗi: " + ex.Message + " Vui lòng liên hệ nhân viên bảo trì để biết thêm chi tiết", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                        //}
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                if (txblTypeIn.Text == "Xe máy")
                {
                    Dispatcher.BeginInvoke(new Action(() => txblTypeIn.Text = "Xe hơi"));
                }
                else if (txblTypeIn.Text == "Xe hơi")
                {
                    Dispatcher.BeginInvoke(new Action(() => txblTypeIn.Text = "Xe đạp"));
                }
                else if (txblTypeIn .Text == "Xe đạp")
                {
                    Dispatcher.BeginInvoke(new Action(() => txblTypeIn.Text = "Xe máy"));
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Staffing.LogOut(MainWindow.currentAccount.AccountName);
            MainWindow.currentUser = null;
            MainWindow.currentAccount = null;
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
        #endregion

        private void ChangePasswordlink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ThayDoiMatKhau tdmk = new ThayDoiMatKhau();
            tdmk.ShowDialog();
        }

        private void ChangePasswordlink_MouseEnter(object sender, MouseEventArgs e)
        {
            txblChangePassword.Cursor = Cursors.Hand;
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Classes.Validation.isNumber.IsMatch(e.Text);
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                e.Handled = true;
        }
    }
}
