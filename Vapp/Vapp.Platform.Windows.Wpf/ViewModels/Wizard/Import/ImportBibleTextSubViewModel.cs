using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Vapp.IO.Codecs.Text;
using Vapp.Platform.Windows.Wpf.Models;
using Vapp.Extensions;

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard.Import
{
    class ImportBibleTextSubViewModel : WizardSubViewModelBase
    {
        #region Constructor

        public ImportBibleTextSubViewModel()
        {
            this.OpenFileCommand = new RelayCommand(this.OpenFile);
            this.RemoveCommand = new RelayCommand(this.RemoveBook);
            this.ClearCommand = new RelayCommand(this.ClearList);
        }

        #endregion

        #region Properties
        
        public string BookName
        {
            get { return this.ImportBibleSubViewModel.BookName; }
        }

        public ImportWizardViewModel ImportWizardViewModel { get; set; }
        
        public ImportBibleSubViewModel ImportBibleSubViewModel
        {
            get { return this.ImportWizardViewModel.ImportBibleSubViewModel; }
        }

        public ObservableCollection<BookModel> BookList { get; private set; } = new ObservableCollection<BookModel>();

        public ICommand OpenFileCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public BookModel selectedBook;
        public BookModel SelectedBook
        {
            get { return this.selectedBook; }
            set
            {
                this.Set(ref this.selectedBook, value);
                this.RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsSelected
        {
            get { return this.SelectedBook != null; }
        }

        public bool HasValues
        {
            get { return this.BookList != null && this.BookList.Count > 0; }
        }

        private BibleDecoder decoder = new BibleDecoder();

        private Bible bible;

        #endregion

        #region Methods

        private void OnBookModelChanged(object sender, EventArgs e)
        {
            BookModel selected = this.SelectedBook;
            IEnumerable<BookModel> tBookList = this.BookList.OrderBy(b => b.Order).ToList();
            App.Current.Dispatcher.Invoke(() =>
            {
                this.BookList.Clear();
                this.BookList.AddRange(tBookList);
                this.SelectedBook = selected;
                this.ImportWizardViewModel.RaisePropertyChanged("HasNext");
                this.RaisePropertyChanged("HasValues");
            });
        }

        private BookModel CreateBookModel(Book book)
        {
            BookModel model = new BookModel(book);
            model.OrderChanged += this.OnBookModelChanged;
            return model;
        }

        public void OpenFile()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = @"All Files(*.*) | *.*";
                dlg.Multiselect = false;
                DialogResult result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Stream stream = File.OpenRead(dlg.FileName);

                    if (this.decoder.TryDecode(stream, out this.bible) && bible.BookList.Count > 0)
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            this.BookList.AddRange(this.bible.BookList.Select(t => this.CreateBookModel(t.Value)).OrderBy(b => b.Order));
                            this.ImportWizardViewModel.RaisePropertyChanged("HasNext");
                            this.RaisePropertyChanged("HasValues");
                        });
                    else
                        MessageBox.Show("File has invalid format");
                }
            }

            GC.Collect();
            GC.Collect();
        }

        public void ClearList()
        {
            foreach (BookModel model in this.BookList)
                model.OrderChanged -= this.OnBookModelChanged;

            App.Current.Dispatcher.Invoke(() =>
            {
                this.BookList.Clear();
                this.ImportWizardViewModel.RaisePropertyChanged("HasNext");
                this.RaisePropertyChanged("HasValues");
            });
        }

        public void RemoveBook()
        {
            if (this.BookList.Contains(this.SelectedBook))
            {
                this.SelectedBook.OrderChanged -= this.OnBookModelChanged;
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.BookList.Remove(this.SelectedBook);
                    this.ImportWizardViewModel.RaisePropertyChanged("HasNext");
                    this.RaisePropertyChanged("HasValues");
                });
            }
        }

        public override void Loaded()
        {
            this.RaisePropertyChanged("BookName");
            this.RaisePropertyChanged("HasValues");
            this.ImportWizardViewModel.RaisePropertyChanged("HasNext");
        }

        public override void Closed()
        {
            this.ClearList();
        }

        public override void Finish()
        {
            // Skip
        }

        #endregion
    }
}
