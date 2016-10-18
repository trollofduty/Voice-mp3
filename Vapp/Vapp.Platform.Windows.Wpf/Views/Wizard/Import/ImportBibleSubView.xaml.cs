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
using Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import;

namespace Vapp.Platform.Windows.Wpf.Views.Wizard.Import
{
    /// <summary>
    /// Interaction logic for ImportBibleSubView.xaml
    /// </summary>
    public partial class ImportBibleSubView : UserControl
    {
        public ImportBibleSubView()
        {
            InitializeComponent();
            this.DataContext = new ImportBibleSubViewModel();
        }
    }
}
