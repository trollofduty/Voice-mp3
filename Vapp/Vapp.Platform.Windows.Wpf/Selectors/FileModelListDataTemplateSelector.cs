using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Vapp.Platform.Windows.Wpf.Models;

namespace Vapp.Platform.Windows.Wpf.Selectors
{
    class FileModelListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileModelTemplate { get; set; }

        public DataTemplate FileModelHeaderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FileModel)
                return this.FileModelTemplate;
            else if (item is FileHeaderModel)
                return this.FileModelHeaderTemplate;

            return null;
        }
    }
}
