using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Vapp.Media;
using Vapp.Platform.Windows.Wpf.Views;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Constructor

        public MainWindowViewModel()
        {
            this.OpenCommand = new RelayCommand(this.Open);
        }

        #endregion

        #region Properties

        private IMediaPlayer MediaPlayer
        {
            get
            {
                return (MediaPlayerControlsViewModel) (((MediaPlayerGroupViewModel) this.MediaPlayerControls.DataContext).MediaControls.DataContext);
            }
        }

        public UserControl MediaPlayerControls { get; set; } = new MediaPlayerGroupView();

        public ICommand OpenCommand { get; set; }

        #endregion

        #region Methods

        private void Open()
        {
            using (System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog())
            {
                dlg.Filter = @"All Files(*.*) | *.*";
                dlg.Multiselect = true;
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    this.MediaPlayer.AddToPlaylist(dlg.FileNames);
            }
        }

        #endregion
    }
}
