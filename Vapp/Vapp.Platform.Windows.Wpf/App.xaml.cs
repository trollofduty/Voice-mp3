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
        public static RegisterService<string, VappCommand> CommandRegisterService { get; set; } = new RegisterService<string, VappCommand>();

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Current.Windows.OfType<T>().Any() : Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        public static string RootPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Vapp";
    }
}
