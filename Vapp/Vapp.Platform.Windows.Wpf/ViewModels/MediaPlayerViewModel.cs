using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Vapp.Media;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MediaPlayerViewModel : ViewModelBase
    {
        #region Constructor

        public MediaPlayerViewModel()
        {
            this.LoadedBehaviour = MediaState.Manual;
        }

        #endregion

        #region Properties

        private MediaState loadedBehaviour;

        public MediaState LoadedBehaviour
        {
            get { return this.loadedBehaviour; }
            set { this.Set(ref this.loadedBehaviour, value); }
        }

        public Func<TimeSpan> RequestMediaTimespan { get; set; }

        public Func<bool> RequestMediaHasTimespan { get; set; }

        public Func<double> RequestMediaTotalSeconds { get; set; }

        public Action<TimeSpan> SetMediaPosition { get; set; }

        public Action<Uri> SetSource { get; set; }

        public Func<Uri> GetSource { get; set; }

        #endregion

        #region Methods
        
        public void OpenMediaSource(string filePath)
        {
            this.SetSource.Invoke(new Uri(filePath));
        }

        public void PlayMedia()
        {
            if (this.GetSource.Invoke() != null)
                this.LoadedBehaviour = MediaState.Play;
        }

        public void PauseMedia()
        {
            this.LoadedBehaviour = MediaState.Pause;
        }

        public void StopMedia()
        {
            this.LoadedBehaviour = MediaState.Stop;
        }

        #endregion
    }
}
