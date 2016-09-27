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
        #region Constructor

        public MediaPlayerControlsView()
        {
            InitializeComponent();
            this.viewModel = new MediaPlayerControlsViewModel();
            this.DataContext = this.viewModel;
        }

        #endregion

        #region Properties

        private MediaPlayerView mediaView;
        private MediaPlayerControlsViewModel viewModel;

        private Track sliderTrack;

        internal MediaPlayerView MediaPlayerView
        {
            get { return this.mediaView; }
            set
            {
                if (this.mediaView != null)
                {
                    this.mediaView.MediaElement.MediaOpened -= this.SetTimespanDelegate;
                    this.mediaView.MediaElement.MediaEnded -= this.NextDelegate;
                }

                this.mediaView = value;

                if (this.mediaView != null)
                {
                    this.mediaView.MediaElement.MediaOpened += this.SetTimespanDelegate;
                    this.mediaView.MediaElement.MediaEnded += this.NextDelegate;
                    this.MediaPlayerViewModel = (MediaPlayerViewModel) this.mediaView.DataContext;
                }
                else
                    this.MediaPlayerViewModel = null;
            }
        }

        internal MediaPlayerViewModel MediaPlayerViewModel
        {
            get { return ((MediaPlayerControlsViewModel) this.DataContext).MediaPlayer; }
            set { ((MediaPlayerControlsViewModel) this.DataContext).MediaPlayer = value; }
        }

        #endregion

        #region Methods

        private void SetTimespanDelegate(object sender, EventArgs e)
        {
            this.viewModel.SetTimeSpan();
        }

        private void NextDelegate(object sender, EventArgs e)
        {
            this.viewModel.Next();
        }

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

        #endregion
    }
}
