using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vapp.Media.Audio;
using Vapp.Media.Gaps;
using Vapp.Platform.Windows.Wpf.Models;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import
{
    class ImportBibleGapSubViewModel : WizardSubViewModelBase
    {
        #region Constructor

        public ImportBibleGapSubViewModel()
        {
            this.GapGenCommand = new RelayCommand(this.GapGen);
        }

        #endregion

        #region Properties

        public ObservableCollection<GapFormat> GapList { get; private set; } = new ObservableCollection<GapFormat>();

        private decimal threshold = 0.08m;
        public decimal Threshold
        {
            get { return this.threshold; }
            set { this.Set(ref this.threshold, value); }
        }

        private double minPause = 0.2;
        public double MinPause
        {
            get { return this.minPause; }
            set { this.Set(ref this.minPause, value); }
        }

        public ImportBibleSubViewModel ImportBibleSubViewModel { get; set; }

        public ImportBibleTextSubViewModel ImportBibleTextSubViewModel { get; set; }

        public ICommand GapGenCommand { get; set; }

        #endregion 

        #region Methods

        public void GapGen()
        {
            foreach (FileModel model in this.ImportBibleSubViewModel.SelectedFiles)
            {
                using (Stream stream = File.OpenRead(model.FullPath))
                {
                    Waveformat pcm;
                    if (DecoderServices.AudioDecoderRegisterService.TryDecode(out pcm, stream))
                        App.Current.Dispatcher.Invoke(() => this.GapList.Add(GapFormat.CreateFrom(pcm, this.Threshold, this.MinPause)));

                    stream.Close();
                }

                GC.Collect();
                GC.Collect();
            }

            GC.Collect();
            GC.Collect();
        }

        public override void Closed()
        {
            // Skip
        }

        public override void Finish()
        {
            // Skip
        }

        public override void Loaded()
        {
            // Skip
        }

        #endregion
    }
}
