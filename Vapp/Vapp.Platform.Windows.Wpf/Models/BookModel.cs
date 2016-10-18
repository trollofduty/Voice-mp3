using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs.Text;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class BookModel : ObservableObject
    {
        #region Constructor

        public BookModel(Book book)
        {
            this.Book = book;
        }

        #endregion

        #region Properties

        private Book book;
        public Book Book
        {
            get { return this.book; }
            set
            {
                this.Set(ref this.book, value);
                this.RaisePropertyChanged("Order");
                this.RaisePropertyChanged("Name");
                this.RaisePropertyChanged("ChaptersCount");
                this.RaisePropertyChanged("VersesCount");
            }
        }

        public int Order
        {
            get { return this.Book.Order; }
            set
            {
                this.Book.Order = value;
                this.RaisePropertyChanged("Order");
            }
        }

        public string Name
        {
            get { return this.Book.Name; }
            set
            {
                this.Book.Name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public int ChaptersCount
        {
            get
            {
                int count = 0;
                foreach (VerseToken token in this.Book.VerseList.Select(t => t.Key))
                    count = token.ChapterNumber > count ? token.ChapterNumber : count;

                return count;
            }
        }

        public int VersesCount
        {
            get
            {
                return this.Book.VerseList.Count;
            }
        }

        #endregion
    }
}
