using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Vapp.Core;

namespace Vapp.Platform.Windows.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RegisterService<string, VappCommand> CommandRegisterService { get; set; }
    }
}
