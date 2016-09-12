using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MediaPlayerViewModel : ViewModelBase
    {
        #region Constructor

        public MediaPlayerViewModel()
        {
            this.OpenMediaCommand = new RelayCommand(this.OpenMedia);
            this.PlayMediaCommand = new RelayCommand(this.PlayMedia);
            this.PauseMediaCommand = new RelayCommand(this.PauseMedia);
            this.StopMediaCommand = new RelayCommand(this.StopMedia);
            this.PreviousMediaCommand = new RelayCommand(this.PreviousMedia);
            this.NextMediaCommand = new RelayCommand(this.NextMedia);
            this.LoadedBehaviour = MediaState.Manual;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += this.TimerTick;
        }

        #endregion

        #region Properties

        private bool isDragging = false;

        private DispatcherTimer timer;

        private double sliderMaxTime;
        public double SliderMaxTime
        {
            get { return this.sliderMaxTime; }
            set { this.Set(ref this.sliderMaxTime, value); }
        }
        
        private double sliderSmallChange;
        public double SliderSmallChange
        {
            get { return this.sliderSmallChange; }
            set { this.Set(ref this.sliderSmallChange, value); }
        }

        private double sliderLargeChange;
        public double SliderLargeChange
        {
            get { return this.sliderLargeChange; }
            set { this.Set(ref this.sliderLargeChange, value); }
        }

        private double sliderValue;
        public double SliderValue
        {
            get { return this.sliderValue; }
            set { this.Set(ref this.sliderValue, value); }
        }

        private Uri mediaSource;
        public Uri MediaSource
        {
            get { return this.mediaSource; }
            set { this.Set(ref this.mediaSource, value); }
        }

        private MediaState loadedBehaviour;

        public MediaState LoadedBehaviour
        {
            get { return this.loadedBehaviour; }
            set { this.Set(ref this.loadedBehaviour, value); }
        }

        public ICommand OpenMediaCommand { get; set; }

        public ICommand PreviousMediaCommand { get; set; }

        public ICommand PlayMediaCommand { get; set; }

        public ICommand PauseMediaCommand { get; set; }

        public ICommand StopMediaCommand { get; set; }

        public ICommand NextMediaCommand { get; set; }

        public Func<TimeSpan> RequestMediaTimespan { get; set; }

        public Func<bool> RequestMediaHasTimespan { get; set; }

        public Func<double> RequestMediaTotalSeconds { get; set; }

        public Action<TimeSpan> SetMediaPosition { get; set; }

        #endregion

        #region Methods

        private void TimerTick(object sender, EventArgs e)
        {
            if (!this.isDragging)
                this.SliderValue = this.RequestMediaTotalSeconds.Invoke();
        }

        private void OpenMedia()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = @"All Files(*.*) | *.*";
                dlg.Multiselect = false;
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                    this.OpenMediaSource(dlg.FileName);
            }
        }

        public void SetTimeSpan()
        {
            if (this.RequestMediaHasTimespan.Invoke())
            {
                TimeSpan timespan = this.RequestMediaTimespan.Invoke();
                this.SliderMaxTime = timespan.TotalSeconds;
                this.SliderSmallChange = 1;
                this.SliderLargeChange = Math.Min(10, timespan.Seconds / 10);
            }
        }

        public void OpenMediaSource(string filePath)
        {
            this.MediaSource = new Uri(filePath);
        }

        private void PlayMedia()
        {
            this.LoadedBehaviour = MediaState.Play;
            this.SetTimeSpan();
            this.timer.Start();
        }

        private void PauseMedia()
        {
            this.LoadedBehaviour = MediaState.Pause;
            this.timer.Stop();
        }

        private void StopMedia()
        {
            this.LoadedBehaviour = MediaState.Stop;
            this.timer.Stop();
            this.SliderValue = 0.0;
        }

        public void SliderDragEnter()
        {
            this.isDragging = true;
        }

        public void SliderDragExit()
        {
            this.isDragging = false;
            this.SetMediaPosition.Invoke(TimeSpan.FromSeconds(this.SliderValue));
        }

        private void PreviousMedia()
        {
        }

        private void NextMedia()
        {
        }

        #endregion
    }
}
