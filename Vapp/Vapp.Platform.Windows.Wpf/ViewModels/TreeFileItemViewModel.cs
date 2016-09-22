using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class TreeFileItemViewModel : TreeItemViewModel
    {
        #region Method

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(TreeFileItemViewModel))
                return this.Equals((TreeFileItemViewModel) obj);

            return base.Equals(obj);
        }

        public bool Equals(TreeFileItemViewModel obj)
        {
            return this.Name == obj.Name && this.Path == obj.Path;
        }

        #endregion
    }
}
