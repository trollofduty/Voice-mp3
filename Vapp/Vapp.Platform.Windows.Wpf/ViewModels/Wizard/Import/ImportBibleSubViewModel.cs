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
            Chapters,
            Verses
        }

        #endregion

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

        public IEnumerable<FileModel> SelectedFiles { get; set; }

        public IEnumerable<FileModel> UnusedFiles { get; set; }

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

        private object selectedItemList;
        public object SelectedItemList
        {
            get { return this.selectedItemList; }
            set { this.Set(ref this.selectedItemList, value); }
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
                if (this.SelectedItemCombo == "System.Windows.Controls.ComboBoxItem: Chapters")
                    return ComboBoxType.Chapters;
                else
                    return ComboBoxType.Verses;
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

        private ChapterFileModel HookChapterFileModel(ChapterFileModel cModel)
        {
            cModel.ChapterChanged += this.OnChapterModelChanged;
            return cModel;
        }

        private VerseFileModel CreateVerseFileModel(FileModel model)
        {
            VerseFileModel vModel = new VerseFileModel(model);
            vModel.ChapterChanged += this.OnVerseModelChanged;
            vModel.VerseChanged += this.OnVerseModelChanged;
            return vModel;
        }

        private void OnChapterModelChanged(object sender, EventArgs e)
        {
            this.SelectedFiles = this.SelectedFiles.OrderBy(c => ((ChapterFileModel) c).Chapter);

            List<object> list = new List<object>();
            list.Add(this.SelectedHeader);
            list.AddRange(this.SelectedFiles);
            list.Add(this.UnusedHeader);
            list.AddRange(this.UnusedFiles);

            App.Current.Dispatcher.Invoke(() =>
            {
                this.SelectedModels.Clear();
                this.SelectedModels.AddRange(list);
                this.SelectedItemList = sender;
            });
        }

        private void OnVerseModelChanged(object sender, EventArgs e)
        {
            this.SelectedFiles = this.SelectedFiles.OrderBy(v => ((VerseFileModel) v).Verse).OrderBy(v => ((VerseFileModel) v).Chapter);

            List<object> list = new List<object>();
            list.Add(this.SelectedHeader);
            list.AddRange(this.SelectedFiles);
            list.Add(this.UnusedHeader);
            list.AddRange(this.UnusedFiles);

            App.Current.Dispatcher.Invoke(() =>
            {
                this.SelectedModels.Clear();
                this.SelectedModels.AddRange(list);
                this.SelectedItemList = sender;
            });
        }

        private void UnhookEvents()
        {
            if (this.SelectedFiles != null)
            {
                foreach (FileModel model in this.SelectedFiles)
                {
                    if (model is ChapterFileModel)
                    {
                        ChapterFileModel cModel = (ChapterFileModel) model;
                        cModel.ChapterChanged -= this.OnChapterModelChanged;
                    }
                    else if (model is VerseFileModel)
                    {
                        VerseFileModel vModel = (VerseFileModel) model;
                        vModel.ChapterChanged -= this.OnVerseModelChanged;
                        vModel.VerseChanged -= this.OnVerseModelChanged;
                    }
                }
            }
        }

        private void RefreshSelectedModels()
        {
            this.UnhookEvents();

            if (this.Files != null)
            {
                if (this.ComboBoxTypeValue == ComboBoxType.Chapters)
                {
                    this.SelectedFiles = this.Files.Where(f => this.WhereRootFiles(f)).Select(f => new ChapterFileModel(this.SelectWithOrder(f, 1)));

                    for (int index = 0; index < this.SelectedFiles.Count(); index++)
                    {
                        ChapterFileModel cModel = (ChapterFileModel) this.SelectedFiles.ElementAt(index);
                        cModel.Chapter = index;
                    }

                    this.SelectedFiles.Select(c => this.HookChapterFileModel((ChapterFileModel) c)).OrderBy(c => c.Chapter);
                    this.UnusedFiles = this.Files.Where(f => !this.WhereRootFiles(f)).Select(f => this.SelectWithOrder(f, 0));
                }
                else
                {
                    this.SelectedFiles = this.Files.Where(f => this.WhereSubRootFiles(f, 1)).Select(f => this.CreateVerseFileModel(this.SelectWithOrder(f, 1))).OrderBy(v => v.Verse).OrderBy(v => v.Chapter);
                    this.UnusedFiles = this.Files.Where(f => !this.WhereSubRootFiles(f)).Select(f => this.SelectWithOrder(f, 0));
                }

                List<object> list = new List<object>();
                list.Add(this.SelectedHeader);
                list.AddRange(this.SelectedFiles);
                list.Add(this.UnusedHeader);
                list.AddRange(this.UnusedFiles);

                App.Current.Dispatcher.Invoke(() =>
                {
                    this.SelectedModels.Clear();
                    this.SelectedModels.AddRange(list);
                });
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
                    this.Files = this.EnumerateSubDirectories(new DirectoryInfo(dlg.SelectedPath)).Where(f => DecoderServices.AudioDecoderRegisterService.Contains(f.Extension.Replace(".", ""))).Select(f => new FileModel(f));
                    this.RefreshSelectedModels();
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
