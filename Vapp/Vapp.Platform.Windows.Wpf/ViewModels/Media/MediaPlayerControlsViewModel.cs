using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Vapp.Core;
using Vapp.Media;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Media
{
    class MediaPlayerControlsViewModel : ViewModelBase, IMediaPlayer, IFileContentProvider
    {
        #region Constructor

        public MediaPlayerControlsViewModel()
        {
            this.OnContentProvided = this.ContentProvided;
            this.RegisterCommands();
            this.TimePlayedText = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
            this.TimeLeftText = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
            Timer.Interval = TimeSpan.FromMilliseconds(200);
            Timer.Tick += this.TimerTick;
        }

        ~MediaPlayerControlsViewModel()
        {
            this.UnregisterCommands();
        }

        #endregion

        #region Properties

        public MediaPlayerViewModel MediaPlayer { get; set; }

        private bool isDragging = false;

        private DispatcherTimer Timer { get; set; } = new DispatcherTimer();

        private string sliderTooltip;
        public string SliderTooltip
        {
            get { return this.sliderTooltip; }
            set { this.Set(ref this.sliderTooltip, value); }
        }

        private string timePlayedText;
        public string TimePlayedText
        {
            get { return this.timePlayedText; }
            set { this.Set(ref this.timePlayedText, value); }
        }

        private string timeLeftText;
        public string TimeLeftText
        {
            get { return this.timeLeftText; }
            set { this.Set(ref this.timeLeftText, value); }
        }

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

        public ICommand PreviousMediaCommand { get; set; }

        public ICommand PlayMediaCommand { get; set; }

        public ICommand PauseMediaCommand { get; set; }

        public ICommand StopMediaCommand { get; set; }

        public ICommand NextMediaCommand { get; set; }

        public ICommand RandomMediaCommand { get; set; }

        public ICommand LoopMediaCommand { get; set; }

        public Queue<string> QueuedMedia { get; private set; } = new Queue<string>();

        public List<string> PlayedMedia { get; private set; } = new List<string>();

        public List<string> Playlist { get; private set; } = new List<string>();

        public string Playing { get; private set; }

        public bool IsShuffle { get; set; } = false;

        public RepeatMode RepeatMode { get; set; } = RepeatMode.None;

        #endregion

        #region Events

        public EventHandler<ContentProvidedEventArgs> OnContentProvided { get; set; }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            this.PlayMediaCommand = new RelayCommand(this.Play);
            this.PauseMediaCommand = new RelayCommand(this.Pause);
            this.StopMediaCommand = new RelayCommand(this.Stop);
            this.PreviousMediaCommand = new RelayCommand(this.Previous);
            this.NextMediaCommand = new RelayCommand(this.Next);
            this.RandomMediaCommand = new RelayCommand(this.Random);
            this.LoopMediaCommand = new RelayCommand(this.Loop);

            App.CommandRegisterService.Hook(new VappCommand(o => this.Play()), "play");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Pause()), "pause");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Stop()), "stop");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Previous()), "previous");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Next()), "next");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Random()), "random");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Loop()), "loop");
        }

        private void UnregisterCommands()
        {
            App.CommandRegisterService.Unhook("play");
            App.CommandRegisterService.Unhook("pause");
            App.CommandRegisterService.Unhook("stop");
            App.CommandRegisterService.Unhook("previous");
            App.CommandRegisterService.Unhook("next");
            App.CommandRegisterService.Unhook("random");
            App.CommandRegisterService.Unhook("loop");
        }

        private void ContentProvided(object sender, ContentProvidedEventArgs args)
        {
            this.OpenSource(args.FilePath);
        }

        private void UpdateTimeTextBlocks()
        {
            if (this.MediaPlayer.RequestMediaHasTimespan.Invoke())
            {
                TimeSpan played = TimeSpan.FromSeconds(this.MediaPlayer.RequestMediaTotalSeconds.Invoke());
                this.TimePlayedText = played.ToString(@"hh\:mm\:ss");
                this.TimeLeftText = this.MediaPlayer.RequestMediaTimespan.Invoke().Subtract(played).ToString(@"hh\:mm\:ss");
            }
            else
            {
                this.TimePlayedText = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
                this.TimeLeftText = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.UpdateTimeTextBlocks();

            if (!this.isDragging)
                this.SliderValue = this.MediaPlayer.RequestMediaTotalSeconds.Invoke();
        }

        public void SetTimeSpan()
        {
            if (this.MediaPlayer.RequestMediaHasTimespan.Invoke())
            {
                TimeSpan timespan = this.MediaPlayer.RequestMediaTimespan.Invoke();
                this.SliderMaxTime = timespan.TotalSeconds;
                this.SliderSmallChange = 1;
                this.SliderLargeChange = Math.Min(10, timespan.Seconds / 10);
            }
        }

        public void OpenSource(string filePath)
        {
            this.MediaPlayer.OpenMediaSource(filePath);
            this.Playing = filePath;
        }

        public void Random()
        {
            this.IsShuffle = !this.IsShuffle;
        }

        public void Loop()
        {
            this.RepeatMode = this.RepeatMode == RepeatMode.Once ? RepeatMode.None : RepeatMode.Once;
        }

        public void Play()
        {
            if (this.MediaPlayer.GetSource.Invoke() == null && this.Playlist.Count > 0)
                this.Next();

            this.MediaPlayer.PlayMedia();

            if (this.MediaPlayer.GetSource.Invoke() != null)
            {
                this.SetTimeSpan();
                this.Timer.Start();
            }
        }

        public void Pause()
        {
            this.MediaPlayer.PauseMedia();
            this.Timer.Stop();
        }

        public void Stop()
        {
            this.MediaPlayer.StopMedia();
            this.Timer.Stop();
            this.SliderValue = 0.0;
            this.UpdateTimeTextBlocks();
        }

        public void SliderDragEnter()
        {
            this.isDragging = true;
        }

        public void SliderDragExit()
        {
            this.isDragging = false;
            this.MediaPlayer.SetMediaPosition.Invoke(TimeSpan.FromSeconds(this.SliderValue));
        }

        public void Previous()
        {
            if (this.PlayedMedia.Count > 0)
            {
                this.QueuedMedia.Enqueue(this.Playing);
                string last = this.PlayedMedia.Last();
                this.OpenSource(last);
                this.PlayedMedia.RemoveAt(this.PlayedMedia.Count - 1);
            }
        }

        public void Next()
        {
            if (this.Playlist.Count == 0)
                return;

            string filepath = null;
            if (this.MediaPlayer.GetSource.Invoke() != null)
                filepath = this.MediaPlayer.GetSource.Invoke().AbsoluteUri.Replace("file:///", "").Replace("/", "\\").Replace("%20", " ");

            if (this.QueuedMedia.Count > 0)
                this.OpenSource(this.QueuedMedia.Dequeue());
            else if (this.IsShuffle)
                this.OpenSource(this.Playlist[(int) (new Random().NextDouble() * this.Playlist.Count)]);
            else if (this.RepeatMode == RepeatMode.Once && filepath != null)
                this.OpenSource(filepath);
            else
            {
                int index = filepath == null ? 0 : this.Playlist.IndexOf(this.Playlist.Where(t => t.Equals(filepath)).FirstOrDefault()) + 1;

                if (index >= this.Playlist.Count && this.RepeatMode == RepeatMode.None)
                    return;

                if (index >= this.Playlist.Count && this.RepeatMode != RepeatMode.None)
                    this.OpenSource(this.Playlist[0]);
                else
                    this.OpenSource(this.Playlist[index]);

            }

            if (this.MediaPlayer.GetSource.Invoke() != null && filepath != null)
                this.PlayedMedia.Add(filepath);
        }

        public void Last()
        {
            if (this.Playlist != null && this.Playlist.Count > 0)
            {
                while (this.Playlist.Last() != this.Playing)
                    this.Next();
            }
            else if (this.QueuedMedia != null && this.QueuedMedia.Count > 0)
            {
                while (this.Playlist.Last() != this.Playing)
                    this.Next();
            }
        }

        public void AddToPlaylist(string filePath)
        {
            this.Playlist.Add(filePath);
        }

        public void AddToPlaylist(IEnumerable<string> filePath)
        {
            this.Playlist.AddRange(filePath);
        }

        public void RemoveFromPlaylist(string filePath)
        {
            this.Playlist.Remove(filePath);
        }

        #endregion
    }
}
