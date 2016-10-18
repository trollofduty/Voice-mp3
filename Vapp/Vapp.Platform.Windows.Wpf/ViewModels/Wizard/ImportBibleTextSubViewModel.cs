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

namespace Vapp.Platform.Windows.Wpf.ViewModels.Wizard
{
    class ImportBibleTextSubViewModel : ViewModelBase, IWizardResult
    {
        #region Constructor

        public ImportBibleTextSubViewModel()
        {
            this.OpenFileCommand = new RelayCommand(this.OpenFile);
            this.RemoveCommand = new RelayCommand(this.RemoveBook);
        }
        
        #endregion

        #region Properties

        public ObservableCollection<BookModel> BookList { get; private set; } = new ObservableCollection<BookModel>();

        public ICommand OpenFileCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public BookModel selectedBook;
        public BookModel SelectedBook
        {
            get { return this.selectedBook; }
            set { this.Set(ref this.selectedBook, value); }
        }

        private BibleDecoder decoder = new BibleDecoder();

        private Bible bible;

        #endregion

        #region Methods

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
                        App.Current.Dispatcher.Invoke(() => this.BookList.AddRange(this.bible.BookList.Select(t => new BookModel(t.Value))));
                    else
                        MessageBox.Show("File has invalid format");
                }
            }

            GC.Collect();
            GC.Collect();
        }

        public void RemoveBook()
        {
            if (this.BookList.Contains(this.SelectedBook))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    this.BookList.Remove(this.SelectedBook);
                });
            }
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
