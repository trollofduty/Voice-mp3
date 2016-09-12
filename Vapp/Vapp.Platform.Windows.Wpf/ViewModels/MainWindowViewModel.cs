using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Vapp.Platform.Windows.Wpf.Views;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        public UserControl MediaPlayerControl { get; set; } = new MediaPlayerView();

        #endregion

        #region Methods

        #endregion
    }
}
