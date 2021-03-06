﻿using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vapp.Platform.Windows.Wpf.ViewModels.Wizard;

namespace Vapp.Platform.Windows.Wpf.Views.Wizard
{
    /// <summary>
    /// Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView : Window
    {
        public WizardView()
        {
            InitializeComponent();
            this.CloseWindowCommand = new RelayCommand(this.Close);
        }

        private ICommand CloseWindowCommand { get; set; }

        internal void SetDataContext(WizardViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            viewModel.CloseWindowCommand = this.CloseWindowCommand;
        }
    }
}
