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
    /// Interaction logic for ImportBibleChaptersSubView.xaml
    /// </summary>
    public partial class ImportBibleChaptersSubView : UserControl
    {
        public ImportBibleChaptersSubView()
        {
            InitializeComponent();
            this.DataContext = new ImportBibleChaptersSubViewModel();
        }
    }
}
