using GalaSoft.MvvmLight.CommandWpf;
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
using System.Windows.Shapes;
using Vapp.Platform.Windows.Wpf.ViewModels;

namespace Vapp.Platform.Windows.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CommandConsoleWindow.xaml
    /// </summary>
    public partial class CommandConsoleView : Window
    {
        public CommandConsoleView()
        {
            InitializeComponent();

            CommandConsoleViewModel vm = new CommandConsoleViewModel();
            this.DataContext = vm;
            vm.CloseWindow = new RelayCommand(this.Close);
            vm.ScrollIntoBottom = new RelayCommand(() => this.Scroller.ScrollToBottom());
            this.Closing += (sender, e) => vm.UnregisterCommands();

            this.MouseLeftButtonDown += (sender, e) => this.InputTextBox.Focus();
            this.MouseLeftButtonUp += (sender, e) => this.InputTextBox.Focus();
        }
    }
}
