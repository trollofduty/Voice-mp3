using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class FileHeaderModel : ObservableObject
    {
        #region Constructor

        public FileHeaderModel(string text, int order = 0)
        {
            this.Text = text;
            this.Order = order;
        }

        #endregion

        #region Properties

        private string text;
        public string Text
        {
            get { return this.text; }
            set { this.Set(ref this.text, value); }
        }

        private int order;
        public int Order
        {
            get { return this.order; }
            set { this.Set(ref this.order, value); }
        }

        #endregion
    }
}
