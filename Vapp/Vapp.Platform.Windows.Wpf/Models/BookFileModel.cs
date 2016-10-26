using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class BookFileModel : FileModel
    {
        #region Constructor

        internal BookFileModel(FileModel model)
            : base(model.FullPath, model.ExpectedName, model.FileSize)
        {
            // Skip
        }

        internal BookFileModel(string fullPath, string expectedName, long fileSize)
            :   base(fullPath, expectedName, fileSize)
        {
            // Skip
        }

        public BookFileModel(FileInfo fInfo)
            : base(fInfo)
        {
            // Skip
        }

        #endregion

        #region Properties

        private int book;
        public int Book
        {
            get { return this.book; }
            set
            {
                this.Set(ref this.book, value);
                this.ChapterChanged?.Invoke(this.book, null);
            }
        }

        #endregion

        #region Events

        public EventHandler<EventArgs> ChapterChanged { get; set; }

        #endregion
    }
}
