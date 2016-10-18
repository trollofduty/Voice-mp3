using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    public abstract class WizardSubViewModelBase : ViewModelBase
    {
        public abstract void Loaded();

        public abstract void Closed();

        public abstract void Finish();
    }
}
