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
            vm.ScrollIntoBottom = new RelayCommand(() =>
            {
                if (VisualTreeHelper.GetChildrenCount(this.BufferListBox) > 0)
                {
                    Border border = (Border) VisualTreeHelper.GetChild(this.BufferListBox, 0);
                    ScrollViewer scrollViewer = (ScrollViewer) VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToBottom();
                }
            });
        }
    }
}
