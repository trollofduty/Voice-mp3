using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class ImportChapterItemModel : ObservableObject
    {
        #region Properties

        private ObservableCollection<ImportFileItemModel> verses;
        public ObservableCollection<ImportFileItemModel> Verses
        {
            get { return this.verses; }
            set { this.Set(ref this.verses, value); }
        }

        #endregion
    }
}
