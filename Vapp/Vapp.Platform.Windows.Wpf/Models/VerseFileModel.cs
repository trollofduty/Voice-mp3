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
            :   base(fullPath, expectedName, fileSize)
        {
            // Skip
        }

        public VerseFileModel(FileInfo fInfo)
            : base(fInfo)
        {
            // Skip
        }

        #endregion
    }
}
