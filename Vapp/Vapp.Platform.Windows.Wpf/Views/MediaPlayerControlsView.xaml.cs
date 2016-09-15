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
    /// Interaction logic for MediaPlayerControlsView.xaml
    /// </summary>
    public partial class MediaPlayerControlsView : UserControl
    {
        public MediaPlayerControlsView(ref MediaPlayerView mediaView)
        {
            InitializeComponent();
            this.mediaView = mediaView;
            this.viewModel = new MediaPlayerControlsViewModel((MediaPlayerViewModel) this.mediaView.DataContext);
            this.DataContext = this.viewModel; 
            this.mediaView.MediaElement.MediaOpened += (sender, e) => this.viewModel.SetTimeSpan();
            this.mediaView.MediaElement.MediaEnded += (sender, e) => this.viewModel.Next();
            this.mediaView.DataContext = viewModel;
        }

        private MediaPlayerView mediaView;
        private MediaPlayerControlsViewModel viewModel;

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

        private void SliderMouseMove(object sender, MouseEventArgs e)
        {
            Point positionUnderMouse = e.GetPosition(this.sliderTrack);
            double predictedValue = PixelsToValue(positionUnderMouse.X, this.Slider.Minimum, this.Slider.Maximum, this.sliderTrack.ActualWidth);
            this.viewModel.SliderTooltip = TimeSpan.FromSeconds(predictedValue).ToString(@"hh\:mm\:ss");
        }

        private double PixelsToValue(double pixels, double minValue, double maxValue, double width)
        {
            double range = maxValue - minValue;
            double percentage = ((double) pixels / width) * 100;
            return ((percentage / 100) * range) + minValue;
        }

        private void SliderMouseLeftClick(object sender, MouseButtonEventArgs e)
        {
            Point positionUnderMouse = e.GetPosition(this.sliderTrack);
            double predictedValue = PixelsToValue(positionUnderMouse.X, this.Slider.Minimum, this.Slider.Maximum, this.sliderTrack.ActualWidth);
            this.mediaView.MediaElement.Position = TimeSpan.FromSeconds(predictedValue);
        }
    }
}
