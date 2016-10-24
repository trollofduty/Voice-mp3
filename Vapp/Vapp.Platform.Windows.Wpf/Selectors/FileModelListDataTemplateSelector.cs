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

        public DataTemplate ChapterFileModelTemplate { get; set; }

        public DataTemplate VerseFileModelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ChapterFileModel)
                return this.ChapterFileModelTemplate;
            else if (item is VerseFileModel)
                return this.VerseFileModelTemplate;
            else if (item is FileModel)
                return this.FileModelTemplate;

            return null;
        }
    }
}
