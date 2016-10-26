using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import
{
    class ImportBibleGapSubViewModel : WizardSubViewModelBase
    {
        #region Properties

        public ImportBibleSubViewModel ImportBibleSubViewModel { get; set; }

        public ImportBibleTextSubViewModel ImportBibleTextSubViewModel { get; set; }

        #endregion 

        #region Methods

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
