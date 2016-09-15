using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Vapp.Platform.Windows.Wpf.Views;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MediaPlayerGroupViewModel : ViewModelBase
    {
        #region Constructor

        public MediaPlayerGroupViewModel()
        {
            MediaPlayerView mediaPlayerView = new MediaPlayerView();
            this.MediaOutput = mediaPlayerView;
            this.MediaControls = new MediaPlayerControlsView(ref mediaPlayerView);
        }

        #endregion

        #region Properties

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
    }
}
