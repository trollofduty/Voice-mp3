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
        #region Enum

        private enum ComboBoxType
        {
            Books,
            Chapters
        }

        #endregion

        #region Constructor

        public ImportBibleSubViewModel()
        {
            this.SelectedModels = new ObservableCollection<object>();
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
            this.ClearListCommand = new RelayCommand(this.ClearList);
            this.RemoveItemCommand = new RelayCommand(this.RemoveItem);
            this.UseItemCommand = new RelayCommand(this.UseItem);
        }

        #endregion

        #region Properties

        public List<FileModel> Files { get; set; }

        public List<FileModel> SelectedFiles { get; set; }

        public List<FileModel> UnusedFiles { get; set; }

        public ObservableCollection<object> selectedModels;
        public ObservableCollection<object> SelectedModels
        {
            get { return this.selectedModels; }
            set
            {
                this.Set(ref this.selectedModels, value);
                this.RaisePropertyChanged("HasValues");
            }
        }

        public string RootDirectory { get; set; }

        public ICommand SelectFolderCommand { get; private set; }

        public ICommand ClearListCommand { get; private set; }

        public ICommand RemoveItemCommand { get; private set; }

        public ICommand UseItemCommand { get; private set; }

        private string bookName;
        public string BookName
        {
            get { return this.bookName; }
            set { this.Set(ref this.bookName, value); }
        }

        private object selectedItemList;
        public object SelectedItemList
        {
            get { return this.selectedItemList; }
            set
            {
                this.Set(ref this.selectedItemList, value);
                this.RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsSelected
        {
            get { return this.SelectedItemList != null; }
        }

        public bool HasValues
        {
            get { return this.SelectedModels != null && this.SelectedModels.Count > 0; }
        }

        private string selectedItemCombo;
        public string SelectedItemCombo
        {
            get { return this.selectedItemCombo; }
            set
            {
                this.Set(ref this.selectedItemCombo, value);
                this.RefreshSelectedModels();
            }
        }

        private ComboBoxType ComboBoxTypeValue
        {
            get
            {
                if (this.SelectedItemCombo == "System.Windows.Controls.ComboBoxItem: Books")
                    return ComboBoxType.Books;
                else
                    return ComboBoxType.Chapters;
            }
        }

        private FileHeaderModel SelectedHeader { get; set; } = new FileHeaderModel("Selected Files", 1);

        private FileHeaderModel UnusedHeader { get; set; } = new FileHeaderModel("Unused Files", 0);

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

        private bool WhereSubRootFiles(FileModel file, int level = 0)
        {
            string[] split = file.FullPath.Split('\\', '/');
            string output = split[0];
            for (int index = 1; index < split.Length - 1 - level; index++)
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

        private BookFileModel HookBookFileModel(BookFileModel cModel)
        {
            cModel.BookChanged += this.OnBookModelChanged;
            return cModel;
        }

        private ChapterFileModel CreateChapterFileModel(FileModel model)
        {
            ChapterFileModel vModel = new ChapterFileModel(model);
            vModel.BookChanged += this.OnChapterModelChanged;
            vModel.BookChanged += this.OnChapterModelChanged;
            return vModel;
        }

        private void OnBookModelChanged(object sender, EventArgs e)
        {
            this.SelectedFiles = this.SelectedFiles.OrderBy(f => f.ExpectedName).OrderBy(b => ((BookFileModel) b).Book).ToList();
            this.PushSelectedModelsList();
            App.Current.Dispatcher.Invoke(() => this.SelectedItemList = sender);
        }

        private void OnChapterModelChanged(object sender, EventArgs e)
        {
            this.SelectedFiles = this.SelectedFiles.OrderBy(f => f.ExpectedName).OrderBy(c => ((ChapterFileModel) c).Chapter).OrderBy(c => ((ChapterFileModel) c).Book).ToList();
            this.PushSelectedModelsList();
            App.Current.Dispatcher.Invoke(() => this.SelectedItemList = sender);
        }

        private void UnhookEvents()
        {
            if (this.SelectedFiles != null)
            {
                foreach (FileModel model in this.SelectedFiles)
                    this.UnhookEvent(model);
            }
        }

        private void UnhookEvent(FileModel model)
        {
            if (model is BookFileModel)
            {
                BookFileModel cModel = (BookFileModel) model;
                cModel.BookChanged -= this.OnBookModelChanged;
            }
            else if (model is ChapterFileModel)
            {
                ChapterFileModel vModel = (ChapterFileModel) model;
                vModel.BookChanged -= this.OnChapterModelChanged;
                vModel.BookChanged -= this.OnChapterModelChanged;
            }
        }

        private void PushSelectedModelsList()
        {
            List<object> list = new List<object>();
            list.Add(this.SelectedHeader);
            list.AddRange(this.SelectedFiles);
            list.Add(this.UnusedHeader);
            list.AddRange(this.UnusedFiles);

            App.Current.Dispatcher.Invoke(() =>
            {
                this.SelectedModels.Clear();
                this.SelectedModels.AddRange(list);
                this.RaisePropertyChanged("HasValues");
            });
        }

        private void RefreshSelectedModels()
        {
            this.UnhookEvents();

            if (this.Files != null)
            {
                if (this.ComboBoxTypeValue == ComboBoxType.Books)
                {
                    this.SelectedFiles = this.Files.Where(f => this.WhereRootFiles(f)).Select(f => (FileModel) new BookFileModel(this.SelectWithOrder(f, 1))).ToList();

                    for (int index = 0; index < this.SelectedFiles.Count(); index++)
                    {
                        BookFileModel cModel = (BookFileModel) this.SelectedFiles.ElementAt(index);
                        cModel.Book = index;
                    }

                    this.SelectedFiles = this.SelectedFiles.Select(f => this.HookBookFileModel((BookFileModel) f)).OrderBy(b => b.ExpectedName).OrderBy(b => b.Book).Select(b => (FileModel) b).ToList();
                    this.UnusedFiles = this.Files.Where(f => !this.WhereRootFiles(f)).Select(f => this.SelectWithOrder(f, 0)).OrderBy(f => f.ExpectedName).ToList();
                }
                else
                {
                    this.SelectedFiles = this.Files.Where(f => this.WhereSubRootFiles(f, 1)).Select(f => this.CreateChapterFileModel(this.SelectWithOrder(f, 1))).OrderBy(c => c.ExpectedName).OrderBy(c => c.Chapter).OrderBy(c => c.Book).Select(c => (FileModel) c).ToList();
                    this.UnusedFiles = this.Files.Where(f => !this.WhereSubRootFiles(f, 1)).Select(f => this.SelectWithOrder(f, 0)).OrderBy(f => f.ExpectedName).ToList();
                }

                this.PushSelectedModelsList();
            }
        }

        public void SelectFolder()
        {
            using (System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.RootDirectory = dlg.SelectedPath;
                    this.Files = this.EnumerateSubDirectories(new DirectoryInfo(dlg.SelectedPath)).Where(f => DecoderServices.AudioDecoderRegisterService.Contains(f.Extension.Replace(".", ""))).Select(f => new FileModel(f)).ToList();
                    this.RefreshSelectedModels();
                }
            }
        }

        public void ClearList()
        {
            this.UnhookEvents();
            this.Files = null;
            App.Current.Dispatcher.Invoke(() =>
            {
                this.SelectedModels.Clear();
                this.RaisePropertyChanged("HasValues");
            });
        }

        public void UseItem()
        {
            if (!(this.SelectedItemList is FileModel) && !(this.SelectedItemList is BookFileModel) && !(this.SelectedItemList is ChapterFileModel))
                return;

            if (this.SelectedFiles.Contains(this.SelectedItemList))
            {
                this.UnhookEvent((FileModel) this.SelectedItemList);
                this.SelectedFiles.Remove((FileModel) this.SelectedItemList);
                this.UnusedFiles.Add(this.Files.Where(f => f.FullPath == ((FileModel) this.SelectedItemList).FullPath).FirstOrDefault());
                this.UnusedFiles = this.UnusedFiles.OrderBy(f => f.ExpectedName).ToList();
            }
            else if (this.UnusedFiles.Contains(this.SelectedItemList))
            {
                FileModel model = this.Files.Where(f => f.FullPath == ((FileModel) this.SelectedItemList).FullPath).FirstOrDefault();
                this.UnusedFiles.Remove((FileModel) this.SelectedItemList);

                if (this.ComboBoxTypeValue == ComboBoxType.Books)
                {
                    this.SelectedFiles.Add(this.HookBookFileModel(new BookFileModel(model)));
                    this.SelectedFiles = this.SelectedFiles.OrderBy(f => f.ExpectedName).OrderBy(b => ((BookFileModel) b).Book).ToList();
                }
                else
                {
                    this.SelectedFiles.Add(this.CreateChapterFileModel(model));
                    this.SelectedFiles = this.SelectedFiles.OrderBy(c => c.ExpectedName).OrderBy(c => ((ChapterFileModel) c).Chapter).OrderBy(c => ((ChapterFileModel) c).Book).ToList();
                }
            }

            this.PushSelectedModelsList();
        }

        public void RemoveItem()
        {
            if (!(this.SelectedItemList is FileModel) && !(this.SelectedItemList is BookFileModel) && !(this.SelectedItemList is ChapterFileModel))
                return;

            if (this.SelectedModels.Contains(this.SelectedItemList))
            {
                this.UnhookEvent((FileModel) this.SelectedItemList);
                IEnumerable<FileModel> files = this.Files.Where(f => f.FullPath == ((FileModel) this.SelectedItemList).FullPath).ToArray();
                this.Files.RemoveRange(files);
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.SelectedModels.Remove(this.SelectedItemList);
                    this.RaisePropertyChanged("HasValues");
                });
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
