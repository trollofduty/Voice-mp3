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
            this.DataContext = this.viewModel;
            this.viewModel.RequestMediaTimespan += () => this.MediaElement.NaturalDuration.TimeSpan;
            this.viewModel.RequestMediaHasTimespan += () => this.MediaElement.NaturalDuration.HasTimeSpan;
            this.viewModel.RequestMediaTotalSeconds += () => this.MediaElement.Position.TotalSeconds;
            this.viewModel.SetMediaPosition += t => this.MediaElement.Position = t;
            this.viewModel.SetSource += t => this.MediaElement.Source = t;
            this.viewModel.GetSource += () => this.MediaElement.Source;
        }

        private MediaPlayerViewModel viewModel;
    }
}
