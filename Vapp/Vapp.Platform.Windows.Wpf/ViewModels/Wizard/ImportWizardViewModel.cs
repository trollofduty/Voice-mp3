using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vapp.Platform.Windows.Wpf.Views.Wizard;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    class ImportWizardViewModel : WizardViewModelBase
    {
        #region Constructor

        public ImportWizardViewModel()
            : base()
        {
            this.Add(new ImportBibleSubView());
            this.Add(new ImportBibleTextSubView());
            this.CurrentSubView = this.WizardControls.FirstOrDefault();
        }

        #endregion
    }
}
