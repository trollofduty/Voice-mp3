using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Vapp.Platform.Windows.Wpf.Views.Media;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MediaPlayerGroupViewModel : ViewModelBase
    {
        #region Constructor

        public MediaPlayerGroupViewModel()
        {
            MediaPlayerView mediaPlayerView = new MediaPlayerView();
            this.MediaOutput = mediaPlayerView;
            this.MediaControls = new MediaPlayerControlsView();
            ((MediaPlayerControlsView) this.MediaControls).MediaPlayerView = mediaPlayerView;
            this.IsFullscreen = false;
            this.Timer.Tick += TimerTick;
            this.Timer.Interval = TimeSpan.FromMilliseconds(200);
        }

        #endregion

        #region Properties

        private DispatcherTimer Timer { get; set; } = new DispatcherTimer();

        private TimeSpan WaitTime { get; set; } = TimeSpan.FromSeconds(5);

        private bool isFullscreen;
        public bool IsFullscreen
        {
            get { return this.isFullscreen; }
            set
            {
                this.Set(ref this.isFullscreen, value);

                if (this.isFullscreen)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    Timer.Start();
                    this.RowSpan = 2;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    Timer.Stop();
                    this.RowSpan = 1;
                }
            }
        }

        private int rowSpan;
        public int RowSpan
        {
            get { return this.rowSpan; }
            set { this.Set(ref rowSpan, value); }
        }
        

        private UserControl mediaOutput;
        public UserControl MediaOutput
        {
            get { return this.mediaOutput; }
            set { this.Set(ref this.mediaOutput, value); }
        }

        private UserControl mediaControls;
        public UserControl MediaControls
        {
            get { return this.mediaControls; }
            set { this.Set(ref this.mediaControls, value); }
        }

        #endregion

        #region Methods

        private void TimerTick(object sender, EventArgs e)
        {
            if (this.IsFullscreen)
            {
                if (GetLastInputTime() > WaitTime)
                    Mouse.OverrideCursor = Cursors.None;
                else
                    Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private static TimeSpan GetLastInputTime()
        {
            var plii = new LastInputInfo();
            plii.cbSize = (uint) Marshal.SizeOf(plii);

            if (GetLastInputInfo(ref plii))
                return TimeSpan.FromMilliseconds(Environment.TickCount - plii.dwTime);
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        #endregion

        #region Import

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetLastInputInfo(ref LastInputInfo plii);

        private struct LastInputInfo
        {
            public uint cbSize;
            public uint dwTime;
        }

        #endregion
    }
}
