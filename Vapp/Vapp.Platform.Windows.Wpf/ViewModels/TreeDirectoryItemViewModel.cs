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

        public TreeDirectoryItemViewModel(string name, string path, ObservableCollection<TreeItemViewModel> items = null)
            : base(name, path)
        {
            this.Items = items == null ? new ObservableCollection<TreeItemViewModel>() : items;
        }

        #endregion

        #region Properties
        
        private object lObject = new object();
        private object iObject = new object();

        private ObservableCollection<TreeItemViewModel> items;
        public ObservableCollection<TreeItemViewModel> Items
        {
            get { return this.items; }
            set { this.Set(ref this.items, value); }
        }

        public override bool Exists
        {
            get { return new DirectoryInfo(this.Path).Exists; }
        }

        #endregion

        #region Method

        public void Validate()
        {
            lock (lObject)
            {
                lock (iObject)
                {
                    IEnumerable<TreeItemViewModel> dExist = this.Items.ToList().Where(i => !i.Exists);
                    App.Current.Dispatcher.Invoke(() => this.Items.RemoveRange(dExist));

                    IEnumerable<TreeItemViewModel> nItems = this.GetItems(this.Path).Where(i => !this.Items.ToList().Select(o => o.Path).Contains(i.Path));
                    App.Current.Dispatcher.Invoke(() => this.Items.AddRange(nItems));
                }

                IEnumerable<TreeDirectoryItemViewModel> dirs = this.Items.ToList().Where(i => i.GetType() == typeof(TreeDirectoryItemViewModel)).Select(d => (TreeDirectoryItemViewModel) d);
                foreach (TreeDirectoryItemViewModel dir in dirs)
                    dir.Validate();
            }
        }

        private ObservableCollection<TreeItemViewModel> GetItems(string path)
        {
            ObservableCollection<TreeItemViewModel> items = new ObservableCollection<TreeItemViewModel>();
            DirectoryInfo dInfo = new DirectoryInfo(path);

            if (!dInfo.Exists)
                dInfo.Create();

            foreach (DirectoryInfo directory in dInfo.GetDirectories())
                items.Add(new TreeDirectoryItemViewModel(directory.Name, directory.FullName));

            foreach (FileInfo file in dInfo.GetFiles())
                items.Add(new TreeFileItemViewModel(file.Name, file.FullName));

            return items;
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
