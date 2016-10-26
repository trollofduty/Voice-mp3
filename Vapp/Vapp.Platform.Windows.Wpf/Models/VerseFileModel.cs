using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class VerseFileModel : FileModel
    {
        #region Constructor

        internal VerseFileModel(FileModel model)
            : base(model.FullPath, model.ExpectedName, model.FileSize)
        {
            // Skip
        }

        internal VerseFileModel(string fullPath, string expectedName, long fileSize)
            : base(fullPath, expectedName, fileSize)
        {
            // Skip
        }

        public VerseFileModel(FileInfo fInfo)
            : base(fInfo)
        {
            // Skip
        }

        #endregion

        #region Properties

        private int chapter;
        public int Chapter
        {
            get { return this.chapter; }
            set
            {
                this.Set(ref this.chapter, value);
                this.ChapterChanged?.Invoke(this, null);
            }
        }

        private int verse;
        public int Verse
        {
            get { return this.verse; }
            set
            {
                this.Set(ref this.chapter, value);
                this.VerseChanged?.Invoke(this, null);
            }
        }

        #endregion

        #region Events

        public EventHandler<EventArgs> ChapterChanged { get; set; }

        public EventHandler<EventArgs> VerseChanged { get; set; }

        #endregion
    }
}
