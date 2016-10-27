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
using Vapp.IO.Codecs.Text;
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

        public ObservableCollection<GapFormatModel> GapList { get; private set; } = new ObservableCollection<GapFormatModel>();

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
                    {
                        GapFormat gap = GapFormat.CreateFrom(pcm, this.Threshold, this.MinPause);
                        Book book = this.ImportBibleTextSubViewModel.BookList.Where(b => b.Name == model.ExpectedName).Select(b => b.Book).FirstOrDefault();

                        GapFormatModel gModel = null;
                        if (model is ChapterFileModel)
                            gModel = new ChapterGapFormatModel((ChapterFileModel) model, book, gap);
                        else if (model is BookFileModel)
                            gModel = new BookGapFormatModel((BookFileModel) model, book, gap);

                        if (gModel != null)
                            App.Current.Dispatcher.Invoke(() => this.GapList.Add(gModel));
                    }
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
