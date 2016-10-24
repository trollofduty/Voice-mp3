using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vapp.Media;
using Vapp.Platform.Windows.Wpf.ViewModels;
using Vapp.Platform.Windows.Wpf.ViewModels.Media;

namespace Vapp.Platform.Windows.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            viewModel.RequestMediaPlayer = this.GetMediaPlayerControlsViewModel;
            viewModel.RequestMediaPlayerGroupViewModel = this.GetMediaPlayerGroupViewModel;
            viewModel.IsFullscreen = false;
            this.DataContext = viewModel;

            this.Closing += (sender, e) =>
            {
                viewModel.Cleanup();
                this.GetExplorerViewModel().Cleanup();
                this.GetMediaPlayerControlsViewModel().Cleanup();
                this.GetMediaPlayerGroupViewModel().Cleanup();
                App.Current.Shutdown();
            };
        }

        #endregion

        #region Methods

        private ExplorerViewModel GetExplorerViewModel()
        {
            return (ExplorerViewModel) this.ExplorerView.DataContext;
        }

        private MediaPlayerControlsViewModel GetMediaPlayerControlsViewModel()
        {
            return (MediaPlayerControlsViewModel) this.GetMediaPlayerGroupViewModel().MediaControls.DataContext;
        }

        private MediaPlayerGroupViewModel GetMediaPlayerGroupViewModel()
        {
            return (MediaPlayerGroupViewModel) this.MainMediaPlayerGroupView.DataContext;
        }

        #endregion
    }
}
