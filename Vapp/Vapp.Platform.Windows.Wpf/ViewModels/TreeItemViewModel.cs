using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    abstract class TreeItemViewModel : ViewModelBase
    {
        #region Constructor

        public TreeItemViewModel(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        #endregion

        #region Properties

        private string name;
        public string Name
        {
            get { return this.name; }
            set { this.Set(ref this.name, value); }
        }

        private string path;
        public string Path
        {
            get { return this.path; }
            set { this.Set(ref this.path, value); }
        }

        public abstract bool Exists { get; }

        #endregion

        #region Method

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(TreeItemViewModel))
                return this.Equals((TreeItemViewModel) obj);

            return base.Equals(obj);
        }

        public bool Equals(TreeItemViewModel obj)
        {
            return this.Name == obj.Name && this.Path == obj.Path;
        }

        #endregion
    }
}
