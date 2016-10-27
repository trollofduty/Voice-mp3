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
    class GapFormatListDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate BookGapFormatModelTemplate { get; set; }

        public DataTemplate ChapterGapFormatModelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BookGapFormatModel)
                return this.BookGapFormatModelTemplate;
            else if (item is ChapterGapFormatModel)
                return this.ChapterGapFormatModelTemplate;

            return null;
        }
    }
}
