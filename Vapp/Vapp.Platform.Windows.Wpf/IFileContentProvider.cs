using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Platform.Windows.Wpf
{
    interface IFileContentProvider
    {
        EventHandler<ContentProvidedEventArgs> OnContentProvided { get; set; }
    }
}
