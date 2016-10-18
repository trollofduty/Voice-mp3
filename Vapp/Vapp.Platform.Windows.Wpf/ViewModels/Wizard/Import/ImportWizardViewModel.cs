﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vapp.Platform.Windows.Wpf.Views.Wizard.Import;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import
{
    class ImportWizardViewModel : WizardViewModelBase
    {
        #region Constructor

        public ImportWizardViewModel()
            : base()
        {
            this.Add(new ImportBibleSubView());
            this.Add(new ImportBibleTextSubView());
            this.ImportBibleTextSubViewModel.ImportBibleSubViewModel = this.ImportBibleSubViewModel;
            this.Start();
        }

        #endregion

        #region Properties

        public ImportBibleSubViewModel ImportBibleSubViewModel
        {
            get { return (ImportBibleSubViewModel) this.WizardControls[0].DataContext; }
        }

        public ImportBibleTextSubViewModel ImportBibleTextSubViewModel
        {
            get { return (ImportBibleTextSubViewModel) this.WizardControls[1].DataContext; }
        }

        #endregion
    }
}