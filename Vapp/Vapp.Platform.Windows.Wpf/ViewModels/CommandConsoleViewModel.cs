using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Vapp.Core;
using Vapp.Platform.Windows.Wpf.Models;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class CommandConsoleViewModel : ViewModelBase
    {
        #region Constructor

        public CommandConsoleViewModel()
        {
            this.ReadInputCommand = new RelayCommand(this.OnReadInputCommand);

            this.RegisterCommands();
            this.Output += "Vapp beta console\n";

            if (SpecialCharacterList == null)
                RegisterSpecialCharacters();
        }

        #endregion

        #region Properties

        public ICommand CloseWindow { get; set; }

        public ICommand ScrollIntoBottom { get; set; }

        public ICommand ReadInputCommand { get; set; }

        public string input;
        public string Input
        {
            get { return this.input; }
            set { this.Set(ref this.input, value); }
        }

        public string output;
        public string Output
        {
            get { return this.output; }
            set { this.Set(ref this.output, value); }
        }

        private static IEnumerable<string> SpecialCharacterList { get; set; }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            App.CommandRegisterService.Hook(new VappCommand(o => this.Close()), "close console", "close cmd");
            App.CommandRegisterService.Hook(new VappCommand(o => this.Help()), "help");
        }

        internal void UnregisterCommands()
        {
            App.CommandRegisterService.Unhook("close console", "close cmd");
            App.CommandRegisterService.Unhook("help");
        }

        private void Close()
        {
            this.CloseWindow.Execute(null);
            this.UnregisterCommands();
        }

        private void Help()
        {
            foreach (KeyValuePair<string, VappCommand> keyPair in App.CommandRegisterService.AsEnumerable())
                this.WriteLine(string.Format("Command: {0}, arguments: {1}", keyPair.Key, keyPair.Value.Parameters));
        }

        private static void RegisterSpecialCharacters()
        {
            List<string> special = new List<string>();
            special.Add("\\\"");
            SpecialCharacterList = special;
        }

        internal static string RemoveSpecialCharacters(string input)
        {
            string output = input;
            foreach (string character in SpecialCharacterList)
                output = output.Replace(character, character.Remove(0, 1));

            return output;
        }

        internal static IEnumerable<string> GetArguments(string input)
        {
            List<string> args = new List<string>();
            int start = 0;
            bool block = false;
            bool reading = false;
            char p = ' ';

            for (int index = 0; index < input.Length; index++)
            {
                char c = input.ElementAt(index);

                if (c == ' ' && !block && reading)
                {
                    args.Add(input.Substring(start, index - start));
                    reading = false;
                }
                else if (c == '"' && p != '\\')
                {
                    if (block && reading)
                    {
                        args.Add(RemoveSpecialCharacters(input.Substring(start + 1, index - start - 1)));
                        block = reading = false;
                    }
                    else if (!block && !reading)
                    {
                        start = index;
                        block = reading = true;
                    }
                }
                else if (c != ' ' && !reading)
                {
                    start = index;
                    reading = true;
                }

                if (index == input.Length - 1 && reading && input.Length != 1)
                    args.Add(input.Substring(block ? start + 1 : start, block ? index - start - 1 : index - start + 1));
                else if (input.Length == 1)
                    args.Add(input);

                p = c;
            }

            return args;
        }

        private void WriteLine(string line)
        {
            this.Output += string.Format("\n{0}", line);
        }

        public void OnReadInputCommand()
        {
            string input = this.Input;
            this.Input = "";
            this.WriteLine(string.Format(">{0}", input));
            if (input != null && input.Length > 0)
            {
                IEnumerable<string> args = GetArguments(input);
                string key = args.FirstOrDefault();
                args = args.Where(arg => args.ElementAt(0) != arg);

                if (!string.IsNullOrEmpty(key) && App.CommandRegisterService.Contains(key))
                {
                    App.CommandRegisterService[key].Invoke(args);
                    this.WriteLine("Command Executed");
                }
                else
                    this.WriteLine(string.Format("'{0}' is not recognized as a command\n", key));
            }
        }

        #endregion
    }
}
