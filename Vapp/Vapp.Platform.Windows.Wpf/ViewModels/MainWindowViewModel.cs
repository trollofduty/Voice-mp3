﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vapp.Core;
using Vapp.Media;
using Vapp.Platform.Windows.Wpf.ViewModels.Media;
using Vapp.Platform.Windows.Wpf.ViewModels.Wizard;
using Vapp.Platform.Windows.Wpf.Views;
using Vapp.Platform.Windows.Wpf.Views.Wizard;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Constructor

        public MainWindowViewModel()
        {
            this.RegisterCommands();
            this.MediaPlayerRow = 1;
            this.MediaPlayerRowSpan = 1;
        }

        ~MainWindowViewModel()
        {
            this.UnregisterCommands();
        }

        #endregion

        #region Properties
        
        public ICommand ImportWizardCommand { get; set; }

        public ICommand SetFullscreenCommand { get; set; }

        public ICommand OpenConsoleCommand { get; set; }

        public ICommand OpenCommand { get; set; }

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

        private WindowState previousState = WindowState.Normal;

        private bool isFullscreen;
        public bool IsFullscreen
        {
            get { return this.isFullscreen; }
            set
            {
                this.Set(ref this.isFullscreen, value);

                if (this.isFullscreen)
                {
                    this.previousState = this.WindowState;
                    this.WindowState = WindowState.Normal;
                    this.WindowStyle = WindowStyle.None;
                    this.WindowResizeMode = ResizeMode.NoResize;
                    this.WindowState = WindowState.Maximized;
                    this.MediaPlayerGroupViewModel.IsFullscreen = true;
                    this.MediaPlayerRow = 0;
                    this.MediaPlayerRowSpan = 2;
                    this.MediaPlayerColumn = 0;
                    this.MediaPlayerColumnSpan = 3;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowResizeMode = ResizeMode.CanResize;
                    this.WindowState = this.previousState;
                    this.MediaPlayerGroupViewModel.IsFullscreen = false;
                    this.MediaPlayerRow = 1;
                    this.MediaPlayerRowSpan = 1;
                    this.MediaPlayerColumn = 2;
                    this.MediaPlayerColumnSpan = 1;
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

        private int mediaPlayerColumn;
        public int MediaPlayerColumn
        {
            get { return this.mediaPlayerColumn; }
            set { this.Set(ref this.mediaPlayerColumn, value); }
        }

        private int mediaPlayerColumnSpan;
        public int MediaPlayerColumnSpan
        {
            get { return this.mediaPlayerColumnSpan; }
            set { this.Set(ref this.mediaPlayerColumnSpan, value); }
        }

        public Func<IMediaPlayer> RequestMediaPlayer { get; set; }

        public Func<MediaPlayerGroupViewModel> RequestMediaPlayerGroupViewModel { get; set; }
        
        private IMediaPlayer MediaPlayer
        {
            get { return this.RequestMediaPlayer.Invoke(); }
        }

        public MediaPlayerGroupViewModel MediaPlayerGroupViewModel
        {
            get { return this.RequestMediaPlayerGroupViewModel.Invoke(); }
        }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            this.OpenCommand = new RelayCommand(this.Open);
            this.SetFullscreenCommand = new RelayCommand(this.ToggleFullscreen);
            this.OpenConsoleCommand = new RelayCommand(this.OpenConsole);
            this.ImportWizardCommand = new RelayCommand(this.OpenImportWizard);

            App.CommandRegisterService.Hook(new VappCommand(o => this.ToggleFullscreen()), "toggle fullscreen", "t fullscreen", "t full");
            App.CommandRegisterService.Hook(new VappCommand(o => this.OpenConsole()), "open console", "open cmd");
            App.CommandRegisterService.Hook(new VappCommand(o => this.OpenConsole()), "import", "import wizard", "open import wizard");
        }

        private void UnregisterCommands()
        {
            App.CommandRegisterService.Unhook("toggle fullscreen", "t fullscreen", "t full");
            App.CommandRegisterService.Unhook("open console", "open cmd");
            App.CommandRegisterService.Unhook("import", "import wizard", "open import wizard");
        }

        private void ToggleFullscreen()
        {
            this.IsFullscreen = !this.IsFullscreen;
        }

        private void OpenConsole()
        {
            if (!App.IsWindowOpen<CommandConsoleView>())
                new CommandConsoleView().Show();
        }

        private void OpenImportWizard()
        {
            WizardView view = this.CreateImportWizard();
            view.Show();
        }

        private WizardView CreateImportWizard()
        {
            WizardView view = new WizardView();
            view.DataContext = new ImportWizardViewModel();
            return view;
        }

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
