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
            this.SelectedModels = new ObservableCollection<object>();
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
            this.ClearListCommand = new RelayCommand(this.ClearList);
            this.RemoveItemCommand = new RelayCommand(this.RemoveItem);
        }

        #endregion

        #region Properties

        public IEnumerable<FileModel> Files { get; set; }

        public ObservableCollection<object> selectedModels;
        public ObservableCollection<object> SelectedModels
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

        private FileModel selectedItemList;
        public FileModel SelectedItemList
        {
            get { return this.selectedItemList; }
            set { this.Set(ref this.selectedItemList, value); }
        }

        private string selectedItemCombo;
        public string SelectedItemCombo
        {
            get { return this.selectedItemCombo; }
            set { this.Set(ref this.selectedItemCombo, value); }
        }

        #endregion

        #region Methods

        private bool WhereRootFiles(FileModel file)
        {
            string[] split = file.FullPath.Split('\\', '/');
            string output = split[0];
            for (int index = 1; index < split.Length - 1; index++)
                output += string.Format("\\{0}", split[index]);

            if (output == this.RootDirectory)
                return true;
            else
                return false;
        }

        private FileModel SelectWithOrder(FileModel file, int order = 0)
        {
            file.Order = order;
            return file;
        }

        public void SelectFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.RootDirectory = dlg.SelectedPath;
                    this.Files = this.EnumerateSubDirectories(new DirectoryInfo(dlg.SelectedPath)).Where(f => DecoderServices.AudioDecoderRegisterService.Contains(f.Extension.Replace(".", ""))).Select(f => new FileModel(f));
                    IEnumerable<FileModel> sFiles = this.Files.Where(f => this.WhereRootFiles(f)).Select(f => SelectWithOrder(f, 1));
                    IEnumerable<FileModel> uFiles = this.Files.Where(f => !this.WhereRootFiles(f)).Select(f => SelectWithOrder(f, 0));

                    List<object> list = new List<object>();
                    list.Add(new FileHeaderModel("Selected Files", 1));
                    list.AddRange(sFiles);
                    list.Add(new FileHeaderModel("Unused Files", 0));
                    list.AddRange(uFiles);

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.SelectedModels.Clear();
                        this.SelectedModels.AddRange(list);
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
            if (this.SelectedModels.Contains(this.SelectedItemList))
                App.Current.Dispatcher.Invoke(() => this.SelectedModels.Remove(this.SelectedItemList));
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
            // Skip
        }

        public override void Finish()
        {
            // Skip
        }

        #endregion
    }
}
