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
            string[] split = expectedName.Split('.');
            string name = "";

            if (split.Length > 1)
            {
                for (int index = 0; index < split.Length - 1; index++)
                    name += split[index];
            }
            else if (split.Length == 1)
                name = split[0];

            this.FullPath = fullPath;
            this.ExpectedName = name;
            this.FileSize = fileSize;
        }

        public FileModel(FileInfo fInfo)
        {
            string[] split = fInfo.Name.Split('.');
            string name = split[0];
            for (int index = 1; index < split.Length - 1; index++)
                name += string.Format(".{0}", split[index]);

            this.FullPath = fInfo.FullName;
            this.ExpectedName = name;
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
            get { return this.FullPath.Split('\\', '/').LastOrDefault(); }
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
