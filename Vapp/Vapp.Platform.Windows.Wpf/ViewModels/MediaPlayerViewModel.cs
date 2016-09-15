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
        #region Properties

        public Func<TimeSpan> RequestMediaTimespan { get; set; }

        public Func<bool> RequestMediaHasTimespan { get; set; }

        public Func<double> RequestMediaTotalSeconds { get; set; }

        public Action<TimeSpan> SetMediaPosition { get; set; }

        public Action<Uri> SetSource { get; set; }

        public Func<Uri> GetSource { get; set; }

        public Action Play { get; set; }

        public Action Pause { get; set; }

        public Action Stop { get; set; }

        #endregion

        #region Methods
        
        public void OpenMediaSource(string filePath)
        {
            this.SetSource.Invoke(new Uri(filePath));
        }

        public void PlayMedia()
        {
            if (this.GetSource.Invoke() != null)
                this.Play();
        }

        public void PauseMedia()
        {
            this.Pause();
        }

        public void StopMedia()
        {
            this.Stop();
        }

        #endregion
    }
}
