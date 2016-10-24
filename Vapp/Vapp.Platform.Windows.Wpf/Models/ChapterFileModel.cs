using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class ChapterFileModel : FileModel
    {
        #region Constructor

        internal ChapterFileModel(FileModel model)
            : base(model.FullPath, model.ExpectedName, model.FileSize)
        {
            // Skip
        }

        internal ChapterFileModel(string fullPath, string expectedName, long fileSize)
            :   base(fullPath, expectedName, fileSize)
        {
            // Skip
        }

        public ChapterFileModel(FileInfo fInfo)
            : base(fInfo)
        {
            // Skip
        }

        #endregion
    }
}
