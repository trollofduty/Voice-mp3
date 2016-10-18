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
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
            ((ImportBibleVersesSubViewModel) this.versesView.DataContext).SelectedFiles = this.SelectedFiles;
            ((ImportBibleChaptersSubViewModel) this.chaptersView.DataContext).SelectedFiles = this.SelectedFiles;
        }

        #endregion

        #region Properties

        private UserControl versesView = new ImportBibleVersesSubView();
        private UserControl chaptersView = new ImportBibleChaptersSubView();

        private UserControl subView;
        public UserControl SubView
        {
            get { return this.subView; }
            set { this.Set(ref this.subView, value); }
        }

        public ObservableCollection<ImportFileItemModel> SelectedFiles { get; set; } = new ObservableCollection<ImportFileItemModel>();

        public string RootDirectory { get; set; }

        public ICommand SelectFolderCommand { get; private set; }

        private string bookName;
        public string BookName
        {
            get { return this.bookName; }
            set { this.Set(ref this.bookName, value); }
        }

        private string selectedItem;
        public string SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.Set(ref this.selectedItem, value);

                if (this.selectedItem == "System.Windows.Controls.ComboBoxItem: Chapters")
                    this.SubView = chaptersView;
                else if (this.selectedItem == "System.Windows.Controls.ComboBoxItem: Verses")
                    this.SubView = versesView;
                else
                    this.SubView = null;
            }
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
                    IEnumerable<ImportFileItemModel> files = this.EnumerateSubDirectories(new DirectoryInfo(dlg.SelectedPath)).Select(f => new ImportFileItemModel(f));
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        this.SelectedFiles.Clear();
                        this.SelectedFiles.AddRange(files);
                    });
                }
            }
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
            throw new NotImplementedException();
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
