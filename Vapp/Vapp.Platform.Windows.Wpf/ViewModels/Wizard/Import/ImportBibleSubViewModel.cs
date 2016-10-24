using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Vapp.Platform.Windows.Wpf.Models;
using Vapp.Platform.Windows.Wpf.Views.Wizard.Import;
using Vapp.Extensions;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import
{
    class ImportBibleSubViewModel : WizardSubViewModelBase
    {
        #region Constructor

        public ImportBibleSubViewModel()
        {
            this.SelectedModels = new ObservableCollection<FileModel>();
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
            this.ClearListCommand = new RelayCommand(this.ClearList);
            this.RemoveItemCommand = new RelayCommand(this.RemoveItem);
        }

        #endregion

        #region Properties

        public ObservableCollection<FileModel> selectedModels;
        public ObservableCollection<FileModel> SelectedModels
        {
            get { return this.selectedModels; }
            set { this.Set(ref this.selectedModels, value); }
        }

        public string RootDirectory { get; set; }

        public ICommand SelectFolderCommand { get; private set; }

        public ICommand ClearListCommand { get; private set; }

        public ICommand RemoveItemCommand { get; private set; }

        private string bookName;
        public string BookName
        {
            get { return this.bookName; }
            set { this.Set(ref this.bookName, value); }
        }

        private FileModel selectedItem;
        public FileModel SelectedItem
        {
            get { return this.selectedItem; }
            set { this.Set(ref this.selectedItem, value); }
        }

        #endregion

        #region Methods

        public void SelectFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.RootDirectory = dlg.SelectedPath;
                    IEnumerable<FileModel> files = this.EnumerateSubDirectories(new DirectoryInfo(dlg.SelectedPath)).Select(f => new FileModel(f));
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.SelectedModels.Clear();
                        this.SelectedModels.AddRange(files);
                    });
                }
            }
        }

        public void ClearList()
        {
            App.Current.Dispatcher.Invoke(this.SelectedModels.Clear);
        }

        public void RemoveItem()
        {
            if (this.SelectedModels.Contains(this.SelectedItem))
                App.Current.Dispatcher.Invoke(() => this.SelectedModels.Remove(this.SelectedItem));
        }

        public List<FileInfo> EnumerateSubDirectories (DirectoryInfo dInfo, List<FileInfo> result = null)
        {
            if (result == null)
                result = new List<FileInfo>();

            result.AddRange(dInfo.EnumerateFiles());
            IEnumerable<DirectoryInfo> dirs = dInfo.EnumerateDirectories();

            foreach (DirectoryInfo dir in dirs)
                this.EnumerateSubDirectories(dir, result);

            return result;
        }

        public override void Loaded()
        {
            // Skip
        }

        public override void Closed()
        {
            throw new NotImplementedException();
        }

        public override void Finish()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
