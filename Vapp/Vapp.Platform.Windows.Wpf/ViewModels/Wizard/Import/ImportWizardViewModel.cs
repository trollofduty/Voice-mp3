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
            this.Add(new ImportBibleGapSubView());
            this.ImportBibleSubViewModel.ImportWizardViewModel = this;
            this.ImportBibleTextSubViewModel.ImportWizardViewModel = this;
            this.ImportBibleGapSubViewModel.ImportBibleSubViewModel = this.ImportBibleSubViewModel;
            this.ImportBibleGapSubViewModel.ImportBibleTextSubViewModel = this.ImportBibleTextSubViewModel;
            this.Start();
        }

        #endregion

        #region Properties

        public override bool HasNext
        {
            get
            {
                if (this.CurrentSubView.DataContext == this.ImportBibleSubViewModel)
                {
                    return this.ImportBibleSubViewModel.SelectedModels != null && this.ImportBibleSubViewModel.SelectedModels.Count > 0
                        && this.ImportBibleSubViewModel.SelectedFiles != null && this.ImportBibleSubViewModel.SelectedFiles.Count > 0
                        && this.ImportBibleSubViewModel.BookName != null && this.ImportBibleSubViewModel.BookName.Length > 0;
                }
                else if (this.CurrentSubView.DataContext == this.ImportBibleTextSubViewModel)
                {
                    return this.ImportBibleTextSubViewModel.BookList != null && this.ImportBibleTextSubViewModel.BookList.Count() > 0;
                }
                return base.HasNext;
            }
            set { base.HasNext = value; }
        }

        public ImportBibleSubViewModel ImportBibleSubViewModel
        {
            get { return (ImportBibleSubViewModel) this.WizardControls[0].DataContext; }
        }

        public ImportBibleTextSubViewModel ImportBibleTextSubViewModel
        {
            get { return (ImportBibleTextSubViewModel) this.WizardControls[1].DataContext; }
        }

        public ImportBibleGapSubViewModel ImportBibleGapSubViewModel
        {
            get { return (ImportBibleGapSubViewModel) this.WizardControls[2].DataContext; }
        }

        #endregion
    }
}
