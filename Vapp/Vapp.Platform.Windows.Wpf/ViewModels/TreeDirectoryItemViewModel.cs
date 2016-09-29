using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Vapp.Extensions;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class TreeDirectoryItemViewModel : TreeItemViewModel
    {
        #region Constructor

        public TreeDirectoryItemViewModel(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        #endregion

        #region Properties
        

        private ObservableCollection<TreeItemViewModel> items;
        public ObservableCollection<TreeItemViewModel> Items
        {
            get { return this.items; }
            set { this.Set(ref this.items, value); }
        }

        public override bool Exists
        {
            get { return new DirectoryInfo(this.Name).Exists; }
        }

        #endregion

        #region Method

        public void Validate(ObservableCollection<TreeItemViewModel> nItems)
        {
            IEnumerable<TreeItemViewModel> dExist = items.Where(i => !i.Exists);

        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(TreeDirectoryItemViewModel))
                return this.Equals((TreeDirectoryItemViewModel) obj);

            return base.Equals(obj);
        }

        public bool Equals(TreeDirectoryItemViewModel obj)
        {
            return this.Name == obj.Name && this.Path == obj.Path;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
