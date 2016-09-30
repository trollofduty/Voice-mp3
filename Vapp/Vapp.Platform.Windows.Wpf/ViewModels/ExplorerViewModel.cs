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
            this.RefreshCommand = new RelayCommand(() => this.Validate());
            this.TreeItems = this.GetItems(this.Path);
        }

        #endregion

        #region Properties

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
        
        public void Validate()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                IEnumerable<TreeItemViewModel> dExist = this.TreeItems.ToList().Where(i => !i.Exists);
                this.TreeItems.RemoveRange(dExist);

                IEnumerable<TreeDirectoryItemViewModel> dirs = dExist.Where(i => i.GetType() == typeof(TreeDirectoryItemViewModel)).Select(d => (TreeDirectoryItemViewModel) d);

                foreach (TreeDirectoryItemViewModel dir in dirs)
                    dir.Validate();
            });
        }

        private ObservableCollection<TreeItemViewModel> GetItems(string path)
        {
            ObservableCollection<TreeItemViewModel> items = new ObservableCollection<TreeItemViewModel>();
            DirectoryInfo dInfo = new DirectoryInfo(path);

            if (!dInfo.Exists)
                dInfo.Create();

            foreach (DirectoryInfo directory in dInfo.GetDirectories())
                items.Add(new TreeDirectoryItemViewModel(directory.Name, directory.FullName) { Items = GetItems(directory.FullName) });

            foreach (FileInfo file in dInfo.GetFiles())
            {
                items.Add(new TreeFileItemViewModel()
                {
                    Name = file.Name,
                    Path = file.FullName,
                });
            }

            return items;
        }

        #endregion
    }
}
