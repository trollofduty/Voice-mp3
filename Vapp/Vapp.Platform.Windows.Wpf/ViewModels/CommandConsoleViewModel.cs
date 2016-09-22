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
using Vapp.Platform.Windows.Wpf.Models;

namespace Vapp.Platform.Windows.Wpf.ViewModels
{
    class CommandConsoleViewModel : ViewModelBase
    {
        #region Constructor

        public CommandConsoleViewModel()
        {
            this.ReadInputCommand = new RelayCommand(this.OnReadInputCommand);
        }

        #endregion

        #region Properties

        public string input;
        public string Input
        {
            get { return this.input; }
            set { this.Set(ref this.input, value); }
        }

        public ICommand ReadInputCommand { get; set; }

        public ObservableCollection<ConsoleBlockModel> Buffer { get; set; } = new ObservableCollection<ConsoleBlockModel>();

        private static IEnumerable<string> SpecialCharacterList { get; set; }

        public ICommand CloseWindow { get; set; }

        public ICommand ScrollIntoBottom { get; set; }

        #endregion

        #region Methods

        private static string RemoveSpecialCharacters(string input)
        {
            string output = input;
            foreach (string character in SpecialCharacterList)
                output = output.Replace(character, character.Remove(0, 1));

            return output;
        }

        private static IEnumerable<string> GetArguments(string input)
        {
            string key = input.Split(' ').FirstOrDefault();
            input = input.Remove(0, key.Length).Trim();

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

                if (index == input.Length - 1 && reading)
                    args.Add(input.Substring(block ? start + 1 : start, block ? index - start - 1 : index - start + 1));

                p = c;
            }

            return args;
        }

        private void WriteLine(string line, Color colour)
        {
            this.Buffer.Add(new ConsoleBlockModel() { Text = line, Colour = colour });
            this.ScrollIntoBottom.Execute(null);
        }

        private void ErrorWriteLine(string line)
        {
            this.Buffer.Add(new ConsoleBlockModel() { Text = line, Colour = Colors.Red });
            this.ScrollIntoBottom.Execute(null);
        }

        public void OnReadInputCommand()
        {
            string input = this.Input;
            this.Input = "";
            if (input != null && input.Length > 0)
            {
                string key = input.Split(' ').FirstOrDefault();
                IEnumerable<string> args = GetArguments(input);

                if (key == "exit")
                    this.CloseWindow.Execute(null);
                else
                    ErrorWriteLine("Command does not exist");
            }
            else
                ErrorWriteLine("No input provided");
        }

        #endregion
    }
}
