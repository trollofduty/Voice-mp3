using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Platform.Windows.Wpf.Models;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    class ImportBibleChaptersSubViewModel : ViewModelBase, IWizardResult
    {
        #region Properties

        private ObservableCollection<ImportFileItemModel> selectedFiles;
        public ObservableCollection<ImportFileItemModel> SelectedFiles
        {
            get { return this.selectedFiles; }
            set { this.Set(ref this.selectedFiles, value); }
        }

        #endregion

        #region Methods

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
