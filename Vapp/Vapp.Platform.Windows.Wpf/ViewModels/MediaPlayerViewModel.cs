﻿using GalaSoft.MvvmLight;
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
using Vapp.Media;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MediaPlayerViewModel : ViewModelBase, IMediaPlayer
    {
        #region Constructor

        public MediaPlayerViewModel()
        {
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

        public ICommand PreviousMediaCommand { get; set; }

        public ICommand PlayMediaCommand { get; set; }

        public ICommand PauseMediaCommand { get; set; }

        public ICommand StopMediaCommand { get; set; }

        public ICommand NextMediaCommand { get; set; }

        public Func<TimeSpan> RequestMediaTimespan { get; set; }

        public Func<bool> RequestMediaHasTimespan { get; set; }

        public Func<double> RequestMediaTotalSeconds { get; set; }

        public Action<TimeSpan> SetMediaPosition { get; set; }

        public Queue<string> QueuedMedia { get; private set; } = new Queue<string>();

        public List<string> PlayedMedia { get; private set; } = new List<string>();

        public List<string> Playlist { get; private set; } = new List<string>();

        public bool IsShuffle { get; set; } = false;

        public RepeatMode RepeatMode { get; set; } = RepeatMode.None;

        #endregion

        #region Methods

        private void TimerTick(object sender, EventArgs e)
        {
            if (!this.isDragging)
                this.SliderValue = this.RequestMediaTotalSeconds.Invoke();
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

        public void PlayMedia()
        {
            this.LoadedBehaviour = MediaState.Play;
            this.SetTimeSpan();
            this.timer.Start();
        }

        public void PauseMedia()
        {
            this.LoadedBehaviour = MediaState.Pause;
            this.timer.Stop();
        }

        public void StopMedia()
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

        public void PreviousMedia()
        {
            if (this.PlayedMedia.Count > 0)
            {
                string last = this.PlayedMedia.Last();
                this.OpenMediaSource(last);
                this.PlayedMedia.RemoveAt(this.PlayedMedia.Count - 1);
                this.QueuedMedia.Enqueue(last);
            }
        }

        public void NextMedia()
        {
            string filepath = null;
            if (this.MediaSource != null)
            {
                filepath = this.MediaSource.AbsolutePath;
                this.PlayedMedia.Add(filepath);
            }

            if (this.QueuedMedia.Count > 0)
                this.OpenMediaSource(this.QueuedMedia.Dequeue());
            else if (this.IsShuffle)
                this.OpenMediaSource(this.Playlist[(int) (new Random().NextDouble() * this.Playlist.Count)]);
            else if (this.RepeatMode == RepeatMode.Once && filepath != null)
                this.OpenMediaSource(filepath);
            else
            {
                int index = filepath == null ? 0 : this.Playlist.IndexOf(filepath);

                if (this.Playlist.Count > 0)
                    this.OpenMediaSource(this.Playlist[index >= this.Playlist.Count && this.RepeatMode != RepeatMode.None ? 0 : index]);
            }
        }

        public void AddToPlaylist(string filePath)
        {
            this.Playlist.Add(filePath);
        }

        public void RemoveFromPlaylist(string filePath)
        {
            this.Playlist.Remove(filePath);
        }

        #endregion
    }
}
