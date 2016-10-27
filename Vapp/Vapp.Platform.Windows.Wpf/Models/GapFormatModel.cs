using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Media.Gaps;

namespace Vapp.Platform.Windows.Wpf.Models
{
    abstract class GapFormatModel : ObservableObject
    {
        #region Constructor

        public GapFormatModel(GapFormat gap)
        {
            this.GapFormat = gap;
            this.RaisePropertyChanged("PauseCount");
        }

        #endregion

        #region Properties

        public GapFormat GapFormat { get; private set; }

        public int PauseCount
        {
            get { return this.GapFormat.Pauses.Count; }
        }

        #endregion
    }
}
