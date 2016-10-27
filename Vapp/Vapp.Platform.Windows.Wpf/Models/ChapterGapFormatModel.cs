using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.IO.Codecs.Text;
using Vapp.Media.Gaps;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class ChapterGapFormatModel : GapFormatModel
    {
        #region Constructor

        public ChapterGapFormatModel(ChapterFileModel model, Book book, GapFormat gap)
            : base(gap)
        {
            this.ChapterFileModel = model;
            this.Book = book;
            this.RaisePropertyChanged("BookName");
            this.RaisePropertyChanged("BookNumber");
            this.RaisePropertyChanged("ChapterNumber");
            this.RaisePropertyChanged("VerseCount");
        }

        #endregion

        #region Properties

        public ChapterFileModel ChapterFileModel { get; private set; }

        public Book Book { get; private set; }

        public string BookName
        {
            get { return this.ChapterFileModel.ExpectedName; }
        }

        public int BookNumber
        {
            get { return this.ChapterFileModel.Book; }
        }

        public int ChapterNumber
        {
            get { return this.ChapterFileModel.Chapter; }
        }

        public int VerseCount
        {
            get { return this.Book.VerseList.Where(t => t.Key.ChapterNumber == this.ChapterNumber).Count(); }
        }

        #endregion
    }
}
