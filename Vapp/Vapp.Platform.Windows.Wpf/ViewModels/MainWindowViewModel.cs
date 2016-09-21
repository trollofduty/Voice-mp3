using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            this.IsFullscreen = false;
            this.OpenCommand = new RelayCommand(this.Open);
            this.SetFullscreenCommand = new RelayCommand(() => this.IsFullscreen = !this.IsFullscreen);
            this.MediaPlayerRow = 1;
            this.MediaPlayerRowSpan = 1;
        }

        #endregion

        #region Properties

        private WindowStyle windowStyle;
        public WindowStyle WindowStyle
        {
            get { return this.windowStyle; }
            set { this.Set(ref this.windowStyle, value); }
        }

        private ResizeMode windowResizeMode;
        public ResizeMode WindowResizeMode
        {
            get { return this.windowResizeMode; }
            set { this.Set(ref this.windowResizeMode, value); }
        }

        private WindowState windowState;
        public WindowState WindowState
        {
            get { return this.windowState; }
            set { this.Set(ref this.windowState, value); }
        }

        public ICommand SetFullscreenCommand { get; set; }

        private bool isFullscreen;
        public bool IsFullscreen
        {
            get { return this.isFullscreen; }
            set
            {
                this.Set(ref this.isFullscreen, value);

                if (this.isFullscreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowResizeMode = ResizeMode.NoResize;
                    this.WindowState = WindowState.Maximized;
                    this.MediaPlayerControlsViewModel.IsFullscreen = true;
                    this.MediaPlayerRow = 0;
                    this.MediaPlayerRowSpan = 2;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowResizeMode = ResizeMode.CanResize;
                    this.WindowState = WindowState.Normal;
                    this.MediaPlayerControlsViewModel.IsFullscreen = false;
                    this.MediaPlayerRow = 1;
                    this.MediaPlayerRowSpan = 1;
                }
            }
        }

        private int mediaPlayerRow;
        public int MediaPlayerRow
        {
            get { return this.mediaPlayerRow; }
            set { this.Set(ref this.mediaPlayerRow, value); }
        }

        private int mediaPlayerRowSpan;
        public int MediaPlayerRowSpan
        {
            get { return this.mediaPlayerRowSpan; }
            set { this.Set(ref this.mediaPlayerRowSpan, value); }
        }

        private IMediaPlayer MediaPlayer
        {
            get
            {
                return (MediaPlayerControlsViewModel) this.MediaPlayerControlsViewModel.MediaControls.DataContext;
            }
        }

        public UserControl MediaPlayerControls { get; set; } = new MediaPlayerGroupView();

        public MediaPlayerGroupViewModel MediaPlayerControlsViewModel
        {
            get { return (MediaPlayerGroupViewModel) this.MediaPlayerControls.DataContext; }
        }

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
