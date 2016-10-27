using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs.Text;
using Vapp.Media.Gaps;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class BookGapFormatModel : GapFormatModel
    {
        #region Constructor

        public BookGapFormatModel(BookFileModel model, Book book, GapFormat gap)
            : base(gap)
        {
            this.BookFileModel = model;
            this.Book = book;
            this.RaisePropertyChanged("BookName");
            this.RaisePropertyChanged("BookNumber");
            this.RaisePropertyChanged("VerseCount");
        }

        #endregion

        #region Properties

        public BookFileModel BookFileModel { get; private set; }

        public Book Book { get; private set; }

        public string BookName
        {
            get { return this.BookFileModel.ExpectedName; }
        }

        public int BookNumber
        {
            get { return this.BookFileModel.Book; }
        }

        public int VerseCount
        {
            get { return this.Book.VerseList.Count; }
        }

        #endregion
    }
}
