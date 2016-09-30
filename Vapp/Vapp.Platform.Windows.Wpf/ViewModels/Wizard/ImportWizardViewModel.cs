using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    class ImportWizardViewModel : ViewModelBase
    {
        #region Constructor

        public ImportWizardViewModel()
        {
            this.RegisterCommands();
        }

        #endregion

        #region Properties

        public ICommand ImportCommand { get; private set; }

        public ICommand SelectFolderCommand { get; private set; }

        private string bookName;
        public string BookName
        {
            get { return this.bookName; }
            set { this.Set(ref this.bookName, value); }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.Set(ref this.isEnabled, value); }
        }

        private bool isChaptersSelected;
        public bool IsChaptersSelected
        {
            get { return this.isChaptersSelected; }
            set { this.Set(ref this.isChaptersSelected, value); }
        }

        private bool isVersesSelected;
        public bool IsVersesSelected
        {
            get { return this.isVersesSelected; }
            set { this.Set(ref this.isVersesSelected, value); }
        }

        #endregion

        #region Methods

        public void Import()
        {

        }

        public void SelectFolder()
        {

        }

        public void RegisterCommands()
        {
            this.ImportCommand = new RelayCommand(this.Import);
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
        }

        public void UnregisterCommands()
        {

        }

        public override void Cleanup()
        {
            base.Cleanup();
            this.UnregisterCommands();
        }

        #endregion
    }
}
