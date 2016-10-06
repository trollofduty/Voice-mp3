using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Vapp.Platform.Windows.Wpf.Models;
using Vapp.Platform.Windows.Wpf.Views.Wizard;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    class ImportBibleSubViewModel : ViewModelBase, IWizardResult
    {
        #region Constructor

        public ImportBibleSubViewModel()
        {
            this.SelectFolderCommand = new RelayCommand(this.SelectFolder);
        }

        #endregion

        #region Properties

        private UserControl verseView = new ImportBibleVersesSubView();
        private UserControl chaptersView = new ImportBibleChaptersSubView();

        private UserControl subView;
        public UserControl SubView
        {
            get { return this.subView; }
            set { this.Set(ref this.subView, value); }
        }

        public ObservableCollection<ImportFileItemModel> SelectedFiles { get; set; } = new ObservableCollection<ImportFileItemModel>();

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
                    this.SubView = verseView;
                else
                    this.SubView = null;
            }
        }

        #endregion

        #region Methods

        public void SelectFolder()
        {

        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
