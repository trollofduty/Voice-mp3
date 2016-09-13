using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vapp.Platform.Windows.Wpf.ViewModels;

namespace Vapp.Platform.Windows.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MediaPlayerView.xaml
    /// </summary>
    public partial class MediaPlayerView : UserControl
    {
        public MediaPlayerView()
        {
            InitializeComponent();
            this.viewModel = new MediaPlayerViewModel();
            this.viewModel.RequestMediaTimespan += () => this.MediaElement.NaturalDuration.TimeSpan;
            this.viewModel.RequestMediaHasTimespan += () => this.MediaElement.NaturalDuration.HasTimeSpan;
            this.viewModel.RequestMediaTotalSeconds += () => this.MediaElement.Position.TotalSeconds;
            this.viewModel.SetMediaPosition += t => this.MediaElement.Position = t;
            this.MediaElement.MediaOpened += (sender, e) => this.viewModel.SetTimeSpan();
            this.MediaElement.MediaEnded += (sender, e) => this.viewModel.NextMedia();
            this.DataContext = viewModel;
        }

        private MediaPlayerViewModel viewModel;
        private Track sliderTrack;

        private void DragStartedEventHandler(object sender, RoutedEventArgs e)
        {
            this.viewModel.SliderDragEnter();
        }

        private void DragCompletedEventHandler(object sender, RoutedEventArgs e)
        {
            this.viewModel.SliderDragExit();
        }

        private void SliderLoaded(object sender, RoutedEventArgs e)
        {
            this.sliderTrack = (Track) this.Slider.Template.FindName("PART_Track", this.Slider);
        }

        // Calculate the tooltip value based on the mouse position and the slider's size, and values.
        void SliderMouseMove(object sender, MouseEventArgs e)
        {
            Point positionUnderMouse = e.GetPosition(this.sliderTrack);
            double predictedValue = PixelsToValue(positionUnderMouse.X, this.Slider.Minimum, this.Slider.Maximum, this.sliderTrack.ActualWidth);
            this.viewModel.SliderTooltip = TimeSpan.FromSeconds(predictedValue).ToString(@"hh\:mm\:ss");
        }

        private double PixelsToValue(double pixels, double minValue, double maxValue, double width)
        {
            var range = maxValue - minValue;
            double percentage = ((double) pixels / width) * 100;
            return ((percentage / 100) * range) + minValue;
        }
    }
}
