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
using Vapp.Platform.Windows.Wpf.ViewModels;

namespace Vapp.Platform.Windows.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ExplorerView.xaml
    /// </summary>
    public partial class ExplorerView : UserControl
    {
        #region Constructor

        public ExplorerView()
        {
            InitializeComponent();
            this.DataContext = new ExplorerViewModel();
        }

        #endregion

        #region Properties
        
        internal IFileContentProvider FileContentProvider
        {
            get { return ((ExplorerViewModel) this.DataContext).FileContentProvider; }
            set { ((ExplorerViewModel) this.DataContext).FileContentProvider = value; }
        }

        #endregion
    }
}
