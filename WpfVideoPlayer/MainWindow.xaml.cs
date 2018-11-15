using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfVideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();            
        }
        private void PlayVideo(string path)
        {
            _media.MediaOpened += _media_MediaOpened;
            //_media.Source = new Uri(dlg.FileName, UriKind.Absolute);
            _media.Clock = new MediaTimeline(new Uri(path, UriKind.Absolute)).CreateClock();
            _media.Clock.CurrentTimeInvalidated += Clock_CurrentTimeInvalidated;
        }
        private void AddVideo()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files(*.*) | *.*| Video Files(*.avi) | *.avi | Video Files(*.mp4) | *.mp4";
            dlg.FilterIndex = 2;

            bool? res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                PlayVideo(dlg.FileName);
                fileListBox.Items.Add(dlg.FileName);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs ea)
        {

            AddVideo();
            
        }

        private void Clock_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            _mediaProgress.Value = _media.Position.TotalSeconds;
        }

        private void _media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _mediaProgress.Maximum = _media.NaturalDuration.TimeSpan.TotalSeconds;
           
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            _media.Clock.Controller.Begin();
        }
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_media.Clock.IsPaused)
                _media.Clock.Controller.Resume();
            else
                _media.Clock.Controller.Pause();
        }
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _media.Clock.Controller.Stop();
        }

        private void _mediaProgress_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(_mediaProgress).X;
            double pos = x * 100 / _mediaProgress.ActualWidth;
            TimeSpan ts = TimeSpan.FromSeconds(_media.Clock.NaturalDuration.TimeSpan.TotalSeconds / 100.0 * pos);

            _media.Clock.Controller.Seek(ts, TimeSeekOrigin.BeginTime);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddVideo();
        }

        private void fileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fileListBox.SelectedIndex >= 0)
            {
                PlayVideo(fileListBox.Items[fileListBox.SelectedIndex].ToString());
            }
        }
    }
}
