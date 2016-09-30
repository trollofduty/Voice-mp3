using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Vapp.Platform.Windows.Wpf.Models;
using Vapp.Extensions;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class ExplorerViewModel : ViewModelBase
    {
        #region Constructor

        public ExplorerViewModel()
        {
            this.RefreshCommand = new RelayCommand(() => Task.Run(() => this.Validate()));
            this.TreeItems = this.GetAllItems(this.Path);
            this.watcher = new FileSystemWatcher(this.Path);
            this.watcher.Changed += this.OnValidate;
            this.watcher.Created += this.OnValidate;
            this.watcher.Deleted += this.OnValidate;
            this.watcher.Renamed += this.OnValidate;
            this.watcher.EnableRaisingEvents = true;
        }

        #endregion

        #region Properties

        private object lObject = new object();
        private object iObject = new object();

        private FileSystemWatcher watcher { get; set; }

        public ICommand RefreshCommand { get; set; }

        private ObservableCollection<TreeItemViewModel> treeItems;
        public ObservableCollection<TreeItemViewModel> TreeItems
        {
            get { return this.treeItems; }
            set { this.Set(ref this.treeItems, value); }
        }

        private string Path { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public IFileContentProvider FileContentProvider { get; set; }

        #endregion

        #region Methods
        
        private void OnValidate(object sender, EventArgs e)
        {
            this.Validate();
        }

        // Still slow, lots of directories and files = slow response time.
        public void Validate()
        {
            lock (lObject)
            {
                lock (iObject)
                {
                    IEnumerable<TreeItemViewModel> dExist = this.TreeItems.ToList().Where(i => !i.Exists);
                    App.Current.Dispatcher.Invoke(() => this.TreeItems.RemoveRange(dExist));

                    IEnumerable<TreeItemViewModel> nItems = this.GetItems(this.Path).Where(i => !this.TreeItems.ToList().Select(o => o.Path).Contains(i.Path));
                    App.Current.Dispatcher.Invoke(() => this.TreeItems.AddRange(nItems));
                }

                IEnumerable<TreeDirectoryItemViewModel> dirs = this.TreeItems.ToList().Where(i => i.GetType() == typeof(TreeDirectoryItemViewModel)).Select(d => (TreeDirectoryItemViewModel) d);
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

        private ObservableCollection<TreeItemViewModel> GetAllItems(string path)
        {
            ObservableCollection<TreeItemViewModel> items = new ObservableCollection<TreeItemViewModel>();
            DirectoryInfo dInfo = new DirectoryInfo(path);

            if (!dInfo.Exists)
                dInfo.Create();

            foreach (DirectoryInfo directory in dInfo.GetDirectories())
                items.Add(new TreeDirectoryItemViewModel(directory.Name, directory.FullName, GetAllItems(directory.FullName)));

            foreach (FileInfo file in dInfo.GetFiles())
                items.Add(new TreeFileItemViewModel(file.Name, file.FullName));

            return items;
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.watcher.Changed -= this.OnValidate;
            this.watcher.Created -= this.OnValidate;
            this.watcher.Deleted -= this.OnValidate;
            this.watcher.Renamed -= this.OnValidate;
            this.watcher.Dispose();
        }

        #endregion
    }
}
