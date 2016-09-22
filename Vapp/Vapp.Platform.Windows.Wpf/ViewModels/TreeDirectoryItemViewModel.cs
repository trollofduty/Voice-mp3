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

        #endregion

        #region Method

        public void Validate(ObservableCollection<TreeItemViewModel> nItems)
        {
            List<TreeItemViewModel> toAdd = new List<TreeItemViewModel>();
            List<TreeItemViewModel> toRemove = new List<TreeItemViewModel>();

            foreach (TreeItemViewModel iItem in this.Items)
            {
                if (nItems.Where(i => i.Equals(iItem)).FirstOrDefault() == null)
                    toRemove.Add(iItem);
            }

            this.Items.RemoveRange(toRemove);

            IEnumerable<TreeDirectoryItemViewModel> dirs = this.Items.Where(i => i.GetType() == typeof(TreeDirectoryItemViewModel)).Select(i => (TreeDirectoryItemViewModel) i);
            foreach (TreeDirectoryItemViewModel dir in dirs)
                dir.Validate(nItems.Where(d => d.Equals(dir)).Select(d => (TreeDirectoryItemViewModel) d).First().Items);
            
            foreach (TreeItemViewModel iItem in nItems)
            {
                if (this.Items.Where(i => i.Equals(iItem)).FirstOrDefault() == null)
                    toAdd.Add(iItem);
            }

            this.Items.AddRange(nItems);
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

        #endregion
    }
}
