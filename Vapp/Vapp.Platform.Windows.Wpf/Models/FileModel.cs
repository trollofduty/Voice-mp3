using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class FileModel : ObservableObject
    {
        #region Constructor

        internal FileModel(string fullPath, string expectedName, long fileSize)
        {
            this.FullPath = fullPath;
            this.ExpectedName = expectedName;
            this.FileSize = fileSize;
        }

        public FileModel(FileInfo fInfo)
        {
            this.FullPath = fInfo.FullName;
            this.ExpectedName = this.FileName;
            this.FileSize = fInfo.Length;
        }

        #endregion

        #region Properties

        private string fullPath;
        public string FullPath
        {
            get { return this.fullPath; }
            set
            {
                this.Set(ref this.fullPath, value);
                this.RaisePropertyChanged("FileName");
            }
        }

        public string FileName
        {
            get { return this.FullPath.Split('\\', '/').FirstOrDefault(); }
        }

        private string expectedName;
        public string ExpectedName
        {
            get { return this.expectedName; }
            set { this.Set(ref this.expectedName, value); }
        }

        private long fileSize;
        public long FileSize
        {
            get { return this.fileSize; }
            set { this.Set(ref this.fileSize, value); }
        }

        private int order;
        public int Order
        {
            get { return this.order; }
            set { this.Set(ref this.order, value); }
        }

        #endregion
    }
}
